using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grow.PortalProveedores.App_Start
{
    public class Role
    {
        public string id { get; set; }
        public string principalId { get; set; }
        public string principalOrganizationId { get; set; }
        public string resourceScope { get; set; }
        public string directoryScopeId { get; set; }
        public string roleDefinitionId { get; set; }
        //public string @odata.id { get; set; }


    }
}