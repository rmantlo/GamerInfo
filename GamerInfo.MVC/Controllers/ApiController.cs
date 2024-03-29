﻿using GamerInfo.Models.ApiModels;
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
            List<ApiDisplay> browsePopular = aservice.GetBrowseGames();
            return View(browsePopular);
        }

        //Details
        public ActionResult Details(ApiDisplay item)
        {
            return View(item);
        }

        public ActionResult Search(string search)
        {
            var aservice = CreateApiService();
            List<ApiDisplay> searchGames = aservice.SearchResults(search);
            if (searchGames != null)
            {
                ViewBag.Value = true;
                return View(searchGames);
            }
            else { ViewBag.Value = false; return View(); }
        }
        //AddToLibrary
        public ActionResult AddToLibrary(ApiDisplay item)
        {
            var aService = CreateApiService();
            if (aService.CheckMyLibrary(item))
            {
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
            else
            {
                TempData["FailResult"] = "Game already in library";
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