using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Grow.PortalProveedores.Authentication;
using Microsoft.Owin.Security;

namespace Grow.PortalProveedores
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                var authenticationManager = new AuthenticationManager();
                authenticationManager.SignOut(HttpContext.Current);
            }

        }
    }
}