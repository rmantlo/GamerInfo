using GamerInfo.Models.ApiModels;
using GamerInfo.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamerInfo.MVC.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public ActionResult Index()
        {
            var aservice = CreateApiService();
            List<ApiDisplay> browsePopular = aservice.GetApiGames();
            return View(browsePopular);
        }

        //Details
        public ActionResult Details(ApiDisplay item)
        {
            return View(item);
        }

        //AddToLibrary
        public ActionResult AddToLibrary(ApiDisplay item)
        {
            var aService = CreateApiService();
            if (aService.AddGameToLibrary(item))
            {
                TempData["SaveResult"] = "Game successfully added to library!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["FailResult"] = "Game NOT added to library";
                return RedirectToAction("Index");
            }
        }

        public ApiService CreateApiService()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ApiService(userId);
            return service;
        }
    }
}