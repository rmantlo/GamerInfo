using GamerInfo.Models;
using GamerInfo.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamerInfo.MVC.Controllers.WebAPI
{
    public class StarController : ApiController
    {
        // GET: Star
        private bool SetStarState(int id, bool value)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new SaveService(userId);
            var details = service.getSaveById(id);

            var updatedSave = new SaveDisplay
            {
                SaveID = id,
                IsCurrent = value,
                SaveTitle = details.SaveTitle,
                SaveInformation = details.SaveInformation,
                Hours = details.Hours,
                GameID = details.GameID
            };
            return service.UpdateSaveFile(updatedSave);
        }

        [HttpPut]
        public bool StarToggleOn(int id) => SetStarState(id, true);
        [HttpDelete]
        public bool StarToggleOff(int id) => SetStarState(id, false);
    }
}