using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Claims;
using Grow.PortalProveedores.Authentication;

namespace Grow.PortalProveedores
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (Request.IsAuthenticated)
                {
                    Response.Redirect("Solicitudes.aspx", true);
                }
        }


        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            //Método de charli
            //HttpContext.GetOwinContext().Authentication.Challenge(
            //        new AuthenticationProperties() { RedirectUri = "/Solicitudes.aspx" }, AuthenticationConfig.generalPolicy);


            //Método en RYASAPortals
            var authenticationManager = new AuthenticationManager();
            authenticationManager.SignIn(HttpContext.Current);

        }
    }
}