using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Grow.Net.GraphConnector;
using Grow.PortalProveedores.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Grow.PortalProveedores.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;



namespace Grow.PortalProveedores
{
    public partial class Compra : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!Request.IsAuthenticated)
            {
                var authenticationManager = new AuthenticationManager();
                authenticationManager.SignIn(HttpContext.Current);

                
            }
            else
            {
                var claims          = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                string displayName  = ClaimsPrincipal.Current.Identities.First().Claims.ToList()[10].Value; // Reading name Claim Property
                string userId       = ClaimsPrincipal.Current.Identities.First().Claims.ToList()[9].Value;  //Reading User Id Claim Property
                NameContent.InnerText = displayName;
                GraphConnector DAC_Graph = new GraphConnector(
                            ConfigurationManager.AppSettings["AppId"],
                            ConfigurationManager.AppSettings["Secret"],
                            ConfigurationManager.AppSettings["Extension"],
                            ConfigurationManager.AppSettings["WebToken"]
                            );
                GraphResult dataRol = DAC_Graph.getRole(userId);
                

            }

            


        }

        [WebMethod]
        public static string registrarCompra(Encabezado encabezado)
        {

            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/MultipleInsert";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"       [ {
                    " + "\n" +
                    @"                 
                                        ""connStr"":null,
                                        ""EntityName"":""OCEncabezado"",
                                        ""EntityAlias"":""OCEncabezado"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""OCId"",
                                                               ""AttrValue"":""" + encabezado.OCId + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""OCId""
                                                            },
                                                            {
                                                                ""AttrName"":""ProveedorId"",
                                                               ""AttrValue"":""" + encabezado.proveedor + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""ProveedorId""
                                                            },
                                                            {
                                                                ""AttrName"":""MonedaId"",
                                                                ""AttrValue"":""" + encabezado.moneda + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""MonedaId""
                                                            },
                                                            {
                                                                ""AttrName"":""Fecha"",
                                                                ""AttrValue"":""" + encabezado.fecha + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Fecha""
                                                            },
                                                            {
                                                                ""AttrName"":""Estatus"",
                                                                ""AttrValue"":""" + encabezado.estatus + @""",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Estatus""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":false

                        " + "\n" +
                        @"        }]
                        " + "\n" +
                        @"";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            System.Diagnostics.Debug.WriteLine(encabezado.items);

            //Items
            string attributes = "";
            for (int i = 0; i < encabezado.items.Length; i++)
            {
                attributes += @"{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""OCLinea"",
                                        ""EntityAlias"":""OCLinea"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                    {
                                                                ""AttrName"":""OCId"",
                                                                ""AttrValue"":""" + encabezado.OCId + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""OCId""
                                                    },
                                                    {
                                                                ""AttrName"":""ArticuloId"",
                                                                ""AttrValue"":""" + encabezado.items[i].ArticuloId + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""ArticuloId""
                                                    },
                                                    {
                                                                ""AttrName"":""NumLinea"",
                                                                ""AttrValue"":" + i + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""NumLinea""
                                                    },
                                                    {
                                                                ""AttrName"":""Cantidad"",
                                                                 ""AttrValue"":" + encabezado.items[i].Cantidad + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Cantidad""
                                                    },
                                                    {
                                                                ""AttrName"":""PrecioUnitario"",
                                                                 ""AttrValue"":" + encabezado.items[i].PrecioUnitario + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""PrecioUnitario""
                                                    },
                                                    {
                                                                ""AttrName"":""Importe"",
                                                                ""AttrValue"":" + encabezado.items[i].Importe + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Importe""
                                                    },
                                                    {
                                                                ""AttrName"":""Descuento"",
                                                                 ""AttrValue"":" + encabezado.items[i].Descuento + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Descuento""
                                                    },
                                                    {
                                                                ""AttrName"":""ImpuestosTransladados"",
                                                                 ""AttrValue"":" + encabezado.items[i].ImpuestosTransladados + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""ImpuestosTransladados""
                                                    },
                                                    {
                                                                ""AttrName"":""ImpuestosRetenidos"",
                                                                 ""AttrValue"":" + encabezado.items[i].ImpuestosRetenidos + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""ImpuestosRetenidos""
                                                    },
                                                    {
                                                                ""AttrName"":""Estatus"",
                                                                 ""AttrValue"":" + encabezado.items[i].Estatus + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Estatus""
                                                    }   
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":false
                        " + "\n" +
                        @"        }";
                if ((encabezado.items.Length - 1) > i)
                {
                    attributes += ",";
                };
            }
                
            request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            body = @"
                        " + "\n" +
                        @"        [" + attributes + @"]
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);

            string retorno = response.Content;

            return retorno;
        }

        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public static string getProveedores()
        {
            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/GetEntity";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"        {
                    " + "\n" +
                    @"                  ""connStr"": null,
                                        ""EntityName"": ""Empresa"",
                                        ""EntityAlias"": ""Empresa"",
                                        ""PKId"": 0,
                                        ""Action"": 0,
                                        ""GroupWheres"": [],
                                        ""Attributes"": [],
                                        ""ChildEntities"": [],
                                        ""getLastIdentity"": false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string retorno = response.Content;

            return retorno;
        }

        }
}