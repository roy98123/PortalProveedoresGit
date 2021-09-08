using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Grow.Net.GraphConnector;
using Grow.PortalProveedores.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Grow.PortalProveedores.Authentication;
using System.Security.Claims;



namespace Grow.PortalProveedores
{
    public partial class Recepcion : System.Web.UI.Page
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


                //Obtenemos la información de las compras realizados
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
                        @"              ""connStr"":null,
                                    ""EntityName"":""OCEncabezado"",
                                    ""EntityAlias"":""OCEncabezado"",
                                    ""PKId"":0,
                                    ""Action"":0,
                                    ""GroupWheres"":[],
                                    ""Attributes"":[],
                                    ""ChildEntities"":[],
                                    ""getLastIdentity"":false
                        " + "\n" +
                            @"        }
                        " + "\n" +
                            @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                JObject responseContent = JObject.Parse(response.Content);

                //llenamos la tabla con las compras realizadas
                foreach (JObject i in responseContent["RetVal"])
                {
                    tbody_compras.InnerHtml += @"<tr>" +
                                                        @"<td>" + i["Attributes"][0]["AttrValue"] + @"</td>" +
                                                        @"<td>" + i["Attributes"][1]["AttrValue"] + @"</td>" +
                                                        @"<td>" + i["Attributes"][2]["AttrValue"] + @"</td>" +
                                                        @"<td>" + i["Attributes"][3]["AttrValue"] + @"</td>" +
                                                        @"<td>" + i["Attributes"][4]["AttrValue"] + @"</td>" +
                                                        @"<td class='text-center'> 
                                                        <button type='button' class='btn btn-primary btninfo'  data-dismiss='modal'
                                                        onclick ='getInfo(""" + i["Attributes"][0]["AttrValue"] + @""",""" + i["Attributes"][1]["AttrValue"] + @""",""" + i["Attributes"][2]["AttrValue"] + @""",""" + i["Attributes"][3]["AttrValue"] + @""")'> 
                                                            Recibir  
                                                        </button>
                                                    </td>" +
                                                   @"</tr>";
                }
            }
        }


        
        [WebMethod]
        public static string getInfo(App_Start.Recepcion recepcion)
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
                       @"              ""connStr"": null,
                                        ""EntityName"": ""OCLinea"",
                                        ""EntityAlias"": ""OCLinea"",
                                        ""PKId"": 0,
                                        ""Action"": 0,
                                        ""GroupWheres"": [{
                                            ""LogicalOperatorWhere"": 1,
                                            ""logicalOperator"": 1,
                                            ""listWhere"": [{
                                                ""whereOperator"": 1,
                                                ""Attribute"": {
                                                    ""AttrName"": ""OCId"",
                                                    ""AttrValue"": """ + recepcion.Id + @""",
                                                    ""AttrType"": ""string"",
                                                    ""AttrAlias"": ""OCId""
                                                }
                                            }]

                                        }],
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

        [WebMethod]
        public static string getLinea(App_Start.Recepcion recepcion)
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
                       @"              ""connStr"": null,
                                        ""EntityName"": ""OCLinea"",
                                        ""EntityAlias"": ""OCLinea"",
                                        ""PKId"": 0,
                                        ""Action"": 0,
                                        ""GroupWheres"": [{
                                            ""LogicalOperatorWhere"": 1,
                                            ""logicalOperator"": 1,
                                            ""listWhere"": [{
                                                ""whereOperator"": 1,
                                                ""Attribute"": {
                                                    ""AttrName"": ""OCId"",
                                                    ""AttrValue"": """ + recepcion.Id + @""",
                                                    ""AttrType"": ""string"",
                                                    ""AttrAlias"": ""OCId""
                                                }
                                            }]

                                        }],
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

        [WebMethod]  
        public static string insertRecepcion(OCRecepcionEncabezado encabezado)
        {
            Console.WriteLine(encabezado);
            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/MultipleInsert";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            //Empresa

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            String fecha = "1998-03-12";
            var body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""OCRecepcionEncabezado"",
                                        ""EntityAlias"":""OCRecepcionEncabezado"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""OCRecepcionId"",
                                                                ""AttrValue"":""" + "prueba" + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""OCRecepcionId ""
                                                            },
                                                            {
                                                                ""AttrName"":""ProveedorId"",
                                                                ""AttrValue"":""" + "RoyEsAmix" + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""ProveedorId ""
                                                            },
                                                            {
                                                                ""AttrName"":""MonedaId"",
                                                                ""AttrValue"":""" + "MXN" + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""MonedaId ""
                                                            },
                                                            {
                                                                ""AttrName"":""Fecha"",
                                                                ""AttrValue"":""" + "1998-03-12" + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Fecha ""
                                                            },
 {
                                                                ""AttrName"":""Estatus"",
                                                                ""AttrValue"":""" + "1" + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Estatus ""
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
            return response.Content;
        }
    }
}