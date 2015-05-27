using FixALeak.API.Models;
using FixALeak.API.Models.Auth;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FixALeak.API
{
    public class FixALeakAuthServerProvider : OAuthAuthorizationServerProvider
    {
        UserManager<IdentityUser> _repo;

        public FixALeakAuthServerProvider(UserManager<IdentityUser> repo)
        {
            _repo = repo;
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IdentityUser user = await _repo.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }


                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

                context.Validated(identity);
        }
    }
}