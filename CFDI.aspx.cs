using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Grow.Net.GraphConnector;
using Grow.PortalProveedores.Authentication;
using System.Security.Claims;
using System.IO;
using System.Xml;
using System.Web.Services;
using Grow.PortalProveedores.App_Start;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Grow.PortalProveedores
{
    public partial class CFDI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var authenticationManager = new AuthenticationManager();
                authenticationManager.SignIn(HttpContext.Current);
            }
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            string displayName = ClaimsPrincipal.Current.Identities.First().Claims.ToList()[10].Value; // Reading name Claim Property
            string userId = ClaimsPrincipal.Current.Identities.First().Claims.ToList()[9].Value;  //Reading User Id Claim Property
            NameContent.InnerText = displayName;
            GraphConnector DAC_Graph = new GraphConnector(
                        ConfigurationManager.AppSettings["AppId"],
                        ConfigurationManager.AppSettings["Secret"],
                        ConfigurationManager.AppSettings["Extension"],
                        ConfigurationManager.AppSettings["WebToken"]
                        );

            GraphResult dataGroup = DAC_Graph.getGroups(userId);
            //Convertimos la respuesta en un Array para que este pueda ser iterado
            JArray objects = JArray.Parse(dataGroup.jsonResult);
            //Iteramos el array en busca de la grupo
            string grupo = null;
            foreach (JToken objeto in objects)
            {
                //Si se encuentra le objeto del grupo y este contiene el grupo de amdinistrador asignamos el grupo a una variable y la randerizamos
                if (objeto["@odata.type"].ToString() == ConfigurationManager.AppSettings["GroupObject"] && objeto["displayName"].ToString() == ConfigurationManager.AppSettings["GroupAdministrator"])
                {
                    grupo = objeto["displayName"].ToString();
                    RolContent.InnerHtml = "<a href='#' class='d-block'>" + grupo + "</a>";
                    ContentPlaceHolder menuPlaceHolder = Page.Master.FindControl("MenuPlaceHolder") as ContentPlaceHolder;
                    LiteralControl l = new LiteralControl();
                    l.Text = "<li class='nav-item'><a runat='server' href='Solicitudes.aspx' class='nav-link'><i class='nav-icon fas fa-list'></i><p>Solicitudes</p></a></li>";
                    l.Text += "<li class='nav-item'><a runat='server' href='NuestrasEmpresas.aspx' class='nav-link'><i class='nav-icon fas fa-user-friends'></i><p>Proveedores</p></a></li>";

                    menuPlaceHolder.Controls.Add(l);
                    break;
                }
            }

            if (grupo is null)
            {
                Response.Redirect("CFDI.aspx", false);
            }
        }

        protected void btnRegistarCFDI_Click(object sender, EventArgs e)
        {
            //FileUpload xml = XML_file;
            errores_xml.Value = "";
            success_xml_titulo.Value = "";
            success_xml_texto.Value = "";
            if (!XML_file.HasFile)
            {
                errores_xml.Value = "Seleccionar archivo XML";
            }
            else
            {
                //Obtenemos la extensión del archivo
                string extension = System.IO.Path.GetExtension(XML_file.FileName).ToLower();
                if (extension != ".xml")
                {
                    errores_xml.Value = "Por favor selecciona un archivo tipo XML";
                }
                else
                {
                    HttpPostedFile xml = XML_file.PostedFile;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xml.InputStream);

                    //CFDI:Comprobante
                    //Nota: Estos primeros datos si se pueden tomar sin problema alguno ya que todos los CFDI tienen estos valores
                    try
                    {
                        tipo_comprobante.Value = doc.DocumentElement.Attributes.GetNamedItem("TipoDeComprobante").Value;    //Tipo de Comprobante
                        folio.Value = doc.DocumentElement.Attributes.GetNamedItem("Folio").Value;                           //Folio
                        serie.Value = doc.DocumentElement.Attributes.GetNamedItem("Serie").Value;                           //Serie
                        fecha_emision.Value = doc.DocumentElement.Attributes.GetNamedItem("Fecha").Value;                   //Fecha emision
                        forma_pago.Value = doc.DocumentElement.Attributes.GetNamedItem("FormaPago").Value;                  //Forma de Pago
                        metodo_pago.Value = doc.DocumentElement.Attributes.GetNamedItem("MetodoPago").Value;                //Método de Pago
                        subtotal.Value = doc.DocumentElement.Attributes.GetNamedItem("SubTotal").Value;                     //Subtotal
                        total.Value = doc.DocumentElement.Attributes.GetNamedItem("Total").Value;                           //Total
                    }
                    catch(Exception)
                    {
                        errores_xml.Value = "No fue posible cargar los datos principales.";
                        return;
                    }


                    //Estos datos no siempre vienen con la misma estructura, por lo tanto he decidido poner cada uno en un trycatch independente

                    try
                    {
                        //CFDI:Emisor
                        nombre_emisor.Value = doc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("Nombre").Value;    //Nombre emisor
                        rfc_emisor.Value = doc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("Rfc").Value;          //RFC emisor
                    }
                    catch { }

                    try
                    {
                    //CFDI:Receptor
                    rfc_receptor.Value = doc.DocumentElement.ChildNodes[1].Attributes.GetNamedItem("Rfc").Value;        //Nombre receptor
                    }
                    catch { }

                    try
                    {
                    //CFDI:Impuestos
                    impuestos_retenidos.Value = doc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("TotalImpuestosRetenidos").Value;        //Total de Impuestos Retenidos
                    impuestos_trasladados.Value = doc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("TotalImpuestosTrasladados").Value;    //Total de Impuestos Retenidos
                    }
                    catch { }

                    try
                    {
                    //CFDI:Complemento //
                    uuid.Value = doc.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes.GetNamedItem("UUID").Value;        //Total de Impuestos Retenidos
                    }
                    catch { }

                   
                    // Convertimos el archivo en Base64
                    String file = Convert.ToBase64String(XML_file.FileBytes);

                    success_xml_titulo.Value = "Archivo analizado correctamente";
                    success_xml_texto.Value = "Recuerda subir tu archivo nuevamente";





                }
            }
        }

        protected void enviarXML(object sender, EventArgs e)
        {

        }


        // Método para hacer un Insert en CFDI, este es llamado por una petición AJAX llamada insertCFDI en cfdi.js
        [WebMethod]
        public static string insertCFDI(Factura factura )
        {
            // Preparamos los parametros para realizar la petición
            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/MultipleInsert";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            // Generamos el cuerpo de la petición respecto a los campos en la base de datos
            var body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""CFDI"",
                                        ""EntityAlias"":""CFDI"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""TipoComprobante"",
                                                                ""AttrValue"":""" + factura.tipoComprobante + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""TipoComprobante""
                                                            },{
                                                                ""AttrName"":""Folio"",
                                                                ""AttrValue"":""" + factura.folio + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Folio""
                                                            },{
                                                                ""AttrName"":""serie"",
                                                                ""AttrValue"":""" + factura.serie + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""serie""
                                                            },{
                                                                ""AttrName"":""RFCEmisor"",
                                                                ""AttrValue"":""" + factura.rfcEmisor + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""RFCEmisor""
                                                            },{
                                                                ""AttrName"":""RFCReceptor"",
                                                                ""AttrValue"":""" + factura.rfcReceptor + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""RFCReceptor""
                                                            },{
                                                                ""AttrName"":""NombreEmisor"",
                                                                ""AttrValue"":""" + factura.nombreEmisor + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""NombreEmisor""
                                                            },{
                                                                ""AttrName"":""FechaEmision"",
                                                                ""AttrValue"":""" + factura.fechaEmision.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""FechaEmision""
                                                            },";
                                                                
                                                            var body2 = @"{
                                                                ""AttrName"":""FechaCertificacion"",
                                                                ""AttrValue"":""" + factura.fechaCertificacion?.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""FechaEmision""
                                                                }";

                                                             var body3 = @"{
                                                                ""AttrName"":""FormaPago"",
                                                                ""AttrValue"":""" + factura.formaPago + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""FormaPago""
                                                            },
    {
                                                                ""AttrName"":""MetodoPago"",
                                                                ""AttrValue"":""" + factura.metodoPago + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""MetodoPago""
                                                            },{
                                                                ""AttrName"":""Subtotal"",
                                                                ""AttrValue"":""" + factura.subtotal+ @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Subtotal""
                                                            },{
                                                                ""AttrName"":""Total"",
                                                                ""AttrValue"":""" + factura.total + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Total""
                                                            },{
                                                                ""AttrName"":""ImpuestosRetenidos"",
                                                                ""AttrValue"":""" + factura.impuestosRetenidos + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""ImpuestosRetenidos""
                                                            },{
                                                                ""AttrName"":""ImpuestosTransladados"",
                                                                ""AttrValue"":""" + factura.total + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""ImpuestosTransladados""
                                                            },{
                                                                ""AttrName"":""UUID"",
                                                                ""AttrValue"":""" + factura.uuid + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""UUID""
                                                            },{
                                                                ""AttrName"":""EstatusComprobante"",
                                                                ""AttrValue"":""" + factura.estatusComprobante + @""",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""EstatusComprobante""
                                                            },{
                                                                ""AttrName"":""EstatusSAT"",
                                                                ""AttrValue"":""" + factura.estatusSAT + @""",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""EstatusSAT""
                                                            },{
                                                                ""AttrName"":""UltimaActualizacionSAT"",
                                                                ""AttrValue"":""" + factura.ultimaActualizacionSAT.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""UltimaActualizacionSAT""
                                                            },{
                                                                ""AttrName"":""XML"",
                                                                ""AttrValue"":""" + factura.xml + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""XML""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":false
                        " + "\n" +
                        @"        }]
                        " + "\n" +
                        @"";

            // Dividimos la cadena "body" para decidir si enviar o no la fecha de certificación, en case de ser null esta no se envia y se asigna NULL en la BD,
            // en caso de enviar una cadena vacia de Fecha de Certificación la API asignará una fecha de manera automatica y esa es la razón de este IF. 
            if (factura.fechaCertificacion != null)
            {
                body = body + body2 + body3;
            }
            else
            {
                body = body + body3;
            }

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

    }


    
}