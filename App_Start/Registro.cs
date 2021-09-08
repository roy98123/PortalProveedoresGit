using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grow.PortalProveedores.App_Start
{
    public class Registro
    {
        public string razon { get; set; }
        public string rfc { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public Empresa[] empresas { get; set; }
    }
    public class Empresa
    {
        public int idEmpresa { get; set; }
        public Documento[] documentos { get; set; }
    }
    public class Documento
    {
        public int idDocumento { get; set; }
        public string nombre { get; set; }
        public string contenido { get; set; }
        public string tipo { get; set; }
    }
}