using GamerInfo.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GamerInfo.MVC.Controllers.WebApi
{
    [Authorize]
    public class FriendlyController : ApiController
    {
        //UPDATE USER ISFAMILYFRIEND
        private bool SetFamilyFriendly(bool value)
        {
            var userId = User.Identity.GetUserId();
            var service = new UserService(userId);
            return service.UpdateUserFriendly(value);

        }
        [HttpPut]
        [Route("g")]
        public bool ToggleFriendlyOn() => SetFamilyFriendly(true);
        [HttpDelete]
        [Route("g")]
        public bool ToggleFriendlyOff() => SetFamilyFriendly(false);

        [HttpPost]
        public string SetThemeType()
        {
            return "Theme method touched";
        }
    }
}