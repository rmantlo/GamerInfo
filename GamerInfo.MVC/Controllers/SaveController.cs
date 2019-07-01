using GamerInfo.Data;
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaveData saveData)
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaveData saveData)
        {
            return View();
        }

        public ActionResult Delete(int? id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public bool DeleteConfirm(int id)
        {
            return false;
        }



        public SaveService CreateSaveService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new SaveService(userId);
            return service;
        }
    }
}