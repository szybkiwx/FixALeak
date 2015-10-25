using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.API.Tests
{
    public static class FakeUserFactory
    {
        public static IPrincipal NewFakeUser(string userName)
        {
            return NewFakeUser(Guid.NewGuid(), userName);
        }

        public static IPrincipal NewFakeUser(Guid userId, string userName)
        {
            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId.ToString())
            };
            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);

           return  new GenericPrincipal(genericIdentity, null);
        }
    }
}
