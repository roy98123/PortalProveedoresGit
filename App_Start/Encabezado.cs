using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grow.PortalProveedores.App_Start
{
    public class Encabezado
    {
        public string OCId { get; set; }
        public string moneda { get; set; }
        public string proveedor { get; set; }
        public string fecha { get; set; }
        public string estatus { get; set; }
        public Items[] items { get; set; }
    }
    
}