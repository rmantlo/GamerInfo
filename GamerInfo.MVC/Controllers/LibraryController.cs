﻿using GamerInfo.Models;
using GamerInfo.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamerInfo.MVC.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
        public ActionResult Index()
        {
            var gservice = CreateGameService();
            var gameList = gservice.GetGameList();
            return View(gameList);
        }
        public ActionResult Details(int id)
        {
            var gservice = CreateGameService();
            var gameDetails = gservice.GetGameDetails(id);

            return View(gameDetails);
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game gameModel)
        {
            if (!ModelState.IsValid) return View(gameModel);

            var gservice = CreateGameService();
            if (gservice.CreateNewGame(gameModel))
            {
                TempData["SaveResult"] = "Your game was created.";
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult Edit(int id)
        {
            var gservice = CreateGameService();
            var model = gservice.GetGameDetails(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Game game)
        {
            if (!ModelState.IsValid) return View(game);
            if (game.GameID != id)
            {
                ModelState.AddModelError("", "ID mismatch");
                return View(game);
            }
            var gservice = CreateGameService();
            if (gservice.UpdateGame(game))
            {
                TempData["SaveResult"] = "Game Information updated.";
                return RedirectToAction($"Details/{game.GameID}");
            }
            return View(game);
        }

        public ActionResult Delete(int id)
        {
            var gservice = CreateGameService();
            var model = gservice.GetGameDetails(id);
            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var gservice = CreateGameService();
            if (gservice.RemoveGame(id))
            {
                TempData["SaveResult"] = "Game removed from Library.";
                return RedirectToAction("Index");
            }
            return View();
        }


        public LibraryService CreateGameService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LibraryService(userId);
            return service;
        }
    }
}