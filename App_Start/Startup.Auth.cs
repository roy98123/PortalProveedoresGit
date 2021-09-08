using System;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Grow.PortalProveedores.Utils;
using System.IdentityModel.Tokens;

namespace Grow.PortalProveedores
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Configure OpenId Connect middleware
            app.UseOpenIdConnectAuthentication(CreateOptionsFormPolicy(AuthenticationConfig.generalPolicy));
        }

        private OpenIdConnectAuthenticationOptions CreateOptionsFormPolicy(string policy)
        {
            return new OpenIdConnectAuthenticationOptions
            {
                // Policy Parameters
                MetadataAddress = AuthenticationConfig.metadataAddress,
                AuthenticationType = policy,

                //OpenId Connect Paramenters
                ClientId = AuthenticationConfig.clientId,
                RedirectUri = AuthenticationConfig.redirectUri,
                PostLogoutRedirectUri = AuthenticationConfig.logoutRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications { },
                Scope = AuthenticationConfig.scope,
                TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false, },
                ResponseType = AuthenticationConfig.responseType

            };
        }
    }
}