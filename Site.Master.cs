using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.Owin.Security;
using System.Web.UI.WebControls;
using Grow.PortalProveedores.Authentication;


namespace Grow.PortalProveedores
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void logOut(object sender, EventArgs e)
        {
            var authenticationManager = new AuthenticationManager();
            authenticationManager.SignOut(HttpContext.Current);

        }
    }
}