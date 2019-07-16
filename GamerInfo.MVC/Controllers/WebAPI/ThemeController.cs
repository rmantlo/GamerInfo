using GamerInfo.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamerInfo.MVC.Controllers.WebAPI
{
    public class ThemeController : Controller
    {
        // GET: Theme

        public ActionResult SetThemeValue(int id)
        {
            var userId = User.Identity.GetUserId();
            var service = new UserService(userId);
            bool result = service.UpdateThemeValue(id);

            //return $"Theme method touched {id}, {result}";
            return RedirectToAction("Index", "Manage");
        }
    }
}