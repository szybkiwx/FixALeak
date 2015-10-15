using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using FixALeak.API.Controllers;

namespace FixALeak.API.Extensions
{ 
    public static class ControllerUsereExtensions
    {
        public static Guid CurrentUserId(this CategoryController controller)
        {
            return Guid.Parse(controller.User.Identity.GetUserId());
        }
    }

}