using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Grow.PortalProveedores
{
    public class Items
    {
        public string OCId { get; set; }
        public int NumLinea { get; set; }
        public string ArticuloId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestosTransladados { get; set; }
        public decimal ImpuestosRetenidos { get; set; }
        public int Estatus { get; set; }

    }
}