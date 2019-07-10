using GamerInfo.Data;
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
    public class SaveController : Controller
    {
        // GET: Save
        public ActionResult Index(Game game)
        {
            var sservice = CreateSaveService();
            var gameSaves = sservice.GetSaveInfoByGame(game.GameID);
            return View(gameSaves);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaveCreate saveData)
        {
            //saveData.GameID = GameId;
            var sservice = CreateSaveService();
            if (sservice.CreateSave(saveData))
            {
                return RedirectToAction($"Details/{saveData.GameID}", "Library");
            }
            return View();
        }


        public ActionResult Edit(int id)
        {
            var sservice = CreateSaveService();
            var result = sservice.getSaveById(id);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaveDisplay save)
        {
            var sservice = CreateSaveService();
            if (sservice.UpdateSaveFile(save))
            {
                TempData["SaveResult"] = "Save Data Updated.";
                return RedirectToAction($"Details/{save.GameID}", "Library");
            }
            TempData["FailResult"] = "Save Data NOT Updated";
            return View(save);
        }

        public ActionResult Delete(int? id)
        {
            var sservice = CreateSaveService();
            int fixedId = (int)id;
            var saveFile = sservice.getSaveById(fixedId);
            if (saveFile.SaveID != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return RedirectToAction($"Details/{saveFile.GameID}","Library");
            }
            return View(saveFile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var sservice = CreateSaveService();
            var saveFile = sservice.getSaveById(id);
            if (!sservice.RemoveSaveFileById(id))
            {
                TempData["FailResult"] = "Save Data NOT Removed.";
                return View();
            }
            return RedirectToAction($"Details/{saveFile.GameID}", "Library");
        }



        public SaveService CreateSaveService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new SaveService(userId);
            return service;
        }
    }
}