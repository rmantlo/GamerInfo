using GamerInfo.Services;
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
            object obj = aservice.GetApiGames();
            return View(obj);
        }


        public ApiService CreateApiService()
        {
            var service = new ApiService();
            return service;
        }
    }
}