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
    [RoutePrefix("Friend")]
    public class FriendlyController : ApiController
    {
        //UPDATE USER ISFAMILYFRIEND
        private bool SetFamilyFriendly(bool value)
        {
            //var userId = Guid.Parse(User.Identity.GetUserId());
            //var service = new UserService(userId);
            //return service.UpdateUserFriendly(value);
            return true;

        }
        [Route("g")]
        [HttpPut]
        public bool ToggleFriendlyOn() => SetFamilyFriendly(true);
        [Route("g")]
        [HttpDelete]
        public bool ToggleFriendlyOff() => SetFamilyFriendly(false);
    }
}