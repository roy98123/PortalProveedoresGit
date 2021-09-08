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
    public partial class NuestrasEmpresas : System.Web.UI.Page
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

            JObject responseContent = JObject.Parse(response.Content);

            foreach (JObject i in responseContent["RetVal"])
            {
                tbody_empresas.InnerHtml += @"<tr>" +
                                                @"<td>" + i["Attributes"][0]["AttrValue"] + @"</td>" +
                                                @"<td>" + i["Attributes"][1]["AttrValue"] + @"</td>" +
                                                @"<td> 
                                                    <button type='button' id='btninfo" + i["Attributes"][0]["AttrValue"] + @"' class='btn btn-primary btninfo' data-toggle='modal' data-target='#modal-info-solicitud' onclick='getInfo(" + i["Attributes"][0]["AttrValue"] + @")'> 
                                                        Ver más  
                                                    </button>
                                                </td>" +
                                               @"</tr>";
            }


           




        }


        [WebMethod]
        public static string getInfo(Solicitud solicitud)
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
                                        ""EntityName"": ""Documento"",
                                        ""EntityAlias"": ""Documento"",
                                        ""PKId"": 0,
                                        ""Action"": 0,
                                        ""GroupWheres"": [
                                                          {
                                                              ""LogicalOperatorWhere"": 1,
                                                              ""logicalOperator"": 1,
                                                              ""listWhere"": [
                                                                              {
                                                                                  ""whereOperator"": 1,
                                                                                  ""Attribute"": {
                                                                                                  ""AttrName"": ""EmpresaId"",
                                                                                                   ""AttrValue"": """ + solicitud.id + @""",
                                                                                                  ""AttrType"": ""int"",
                                                                                                  ""AttrAlias"": ""EmpresaId""
                                                                                                  }
                                                                              }
                                                                          ]

                                                          }
                                                      ],
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
            //getEmpresasDocs();

            return retorno;
        }



        private void ActualizarEstatus(int op)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                string server = ConfigurationManager.AppSettings["Host"].ToString();
                string _urlApi = "api/IncomingMessage";
                var client = new RestClient(server + _urlApi);
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
                var body = @"
                        " + "\n" +
                            @"        {
                        " + "\n" +
                            @"                 
                                    ""connStr"":null,
                                    ""EntityName"":""SolicitudAlta"",
                                    ""EntityAlias"":""SolicitudAlta"",
                                    ""PKId"":0,
                                    ""Action"":0,
                                    ""GroupWheres"":[{
                                        ""LogicalOperatorWhere"": 1,
                                        ""logicalOperator"": 1,
                                        ""listWhere"": [{
                                            ""whereOperator"": 1,
                                            ""Attribute"": {
                                                ""AttrName"": ""Id"",
                                                ""AttrValue"": " + Request.QueryString["ID"] + @",
                                                ""AttrType"": ""int"",
                                                ""AttrAlias"": ""Id""
                                            }
                                        }]
                                    }],
                                    ""Attributes"":[{
                                        ""AttrName"":""Estatus"",
                                        ""AttrValue"":" + op + @",
                                        ""AttrType"":""int"",
                                        ""AttrAlias"":""Estatus""
                                    }],
                                    ""ChildEntities"":[],
                                    ""getLastIdentity"":false        

                        " + "\n" +
                            @"        }
                        " + "\n" +
                            @"";

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                
                dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
                if (responseContent.Success == true)
                {
                    //pruebas.InnerText = response.Content;
                    Response.Redirect("Solicitudes.aspx");
                }
            }
        }
    }
}