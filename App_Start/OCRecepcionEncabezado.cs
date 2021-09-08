using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grow.PortalProveedores.App_Start
{
    public class OCRecepcionEncabezado
    {
        public string OCRecepcionId { get; set; }
        public string ProveedorId { get; set; }
        public string MonedaId { get; set; }
        public string Fecha { get; set; }
        public Nullable<int> Estatus { get; set; }
        //public Items[] items { get; set; }     
    }
}