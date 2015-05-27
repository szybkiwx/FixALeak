﻿using FixALeak.API.Models;
using FixALeak.API.Models.Auth;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;


namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private UserManager<IdentityUser> _repo = null;

        public AccountController(UserManager<IdentityUser> repo)
        {
            _repo = repo;
        }


        [AllowAnonymous]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.CreateAsync(new IdentityUser() { UserName = userModel.UserName }, userModel.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
