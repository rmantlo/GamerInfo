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
        public ActionResult Index(string search)
        {
            var aservice = CreateApiService();
            if (search != "" && search != null)
            {
                List<ApiDisplay> searchGames = aservice.SearchResults(search);
                return View(searchGames);
            }
            else
            {
                List<ApiDisplay> browsePopular = aservice.GetBrowseGames();
                return View(browsePopular);
                //List<ApiDisplay> searchGames = new List<ApiDisplay>();
                //return View(searchGames);
            }
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