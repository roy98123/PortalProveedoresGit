using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grow.PortalProveedores.App_Start
{
    public class Factura
    {
        public string xml { get; set; }
        public string tipoComprobante { get; set; }
        public string folio { get; set; }
        public string serie { get; set; }
        public string rfcEmisor { get; set; }
        public string rfcReceptor { get; set; }
        public string nombreEmisor { get; set; }
        public DateTime fechaEmision { get; set; }
        public DateTime? fechaCertificacion { get; set; }
        public string formaPago { get; set; }
        public string metodoPago { get; set; }
        public string subtotal { get; set; }
        public string total { get; set; }
        public string impuestosRetenidos { get; set; }
        public string impuetoTrasladados { get; set; }
        public string uuid { get; set; }
        public string estatusComprobante { get; set; }
        public string estatusSAT { get; set; }
        public DateTime ultimaActualizacionSAT { get; set; }
    }
}