using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Grow.PortalProveedores.Utils;


namespace Grow.PortalProveedores.Authentication
{
    public class AuthenticationManager
    {

        public void SignOut(HttpContext context)
        {
            IEnumerable<AuthenticationDescription> authTypes = context.GetOwinContext().Authentication.GetAuthenticationTypes();
            context.GetOwinContext().Authentication.SignOut(authTypes.Select(t => t.AuthenticationType).ToArray());
        }

        public void SignIn(HttpContext context)
        {
            context.GetOwinContext().Authentication.Challenge(
            new AuthenticationProperties() { RedirectUri = AuthenticationConfig.redirectUri }, AuthenticationConfig.generalPolicy);
        }

            
    }
}