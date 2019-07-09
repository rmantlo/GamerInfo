using GamerInfo.Models;
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
        public ActionResult Details()
        {
            return View();
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

        public LibraryService CreateGameService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LibraryService(userId);
            return service;
        }
    }
}