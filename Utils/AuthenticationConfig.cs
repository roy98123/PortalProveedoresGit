using System;
using System.Configuration;

namespace Grow.PortalProveedores.Utils
{
    public static class AuthenticationConfig
    {
        // App config settings
        public static string clientId { get; } = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string aadInstance { get; } = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string tenant { get; } = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string redirectUri { get; } = ConfigurationManager.AppSettings["ida:RedirectUri"];
        public static string logoutRedirectUri { get; } = ConfigurationManager.AppSettings["ida:LogoutRedirectUri"];

        // B2C policy identifiers
        public static string generalPolicy { get; } = ConfigurationManager.AppSettings["ida:GeneralPolicy"];

        // Adress of the Tenant
        public static string metadataAddress { get; } = String.Format(aadInstance, tenant, generalPolicy);

        //Standard OpenId Connector paramentersre
        public static string scope { get; } = "openid";
        public static string responseType { get; } = "id_token";
    }
}