using Grow.PortalProveedores.App_Start;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Grow.PortalProveedores.Authentication;
using System.Security.Claims;
using Grow.Net.GraphConnector;
using Newtonsoft.Json.Linq;

namespace Grow.PortalProveedores
{
    public partial class RegisterCompany : System.Web.UI.Page
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



        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public static string getCompanies()
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
                        @"              ""connStr"":null,
                                        ""EntityName"":""Empresa"",
                                        ""EntityAlias"":""Empresa"",
                                        ""PKId"":1,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""RazonSocial"",
                                                                ""AttrValue"":""1"",
                                                                ""AttrType"":"""",
                                                                ""AttrAlias"":""RazonSocial""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public static string insertCompany(Registro empresa, RegistroDocumentos documentos)
        {

            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/MultipleInsert";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            //Empresa

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""Empresa"",
                                        ""EntityAlias"":""Empresa"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""RazonSocial"",
                                                                ""AttrValue"":""" + empresa.razon + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""RazonSocial""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":true
                        " + "\n" +
                        @"        }]
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            int IdEmpresa = 0;
            foreach (dynamic i in responseContent)
            {
                IdEmpresa = i.returnedId;
            }

            //Documentos
            string attributes_documents = "";
            int large = documentos.documentos.Length;
            int index_docs = 0;
            foreach(string doc in documentos.documentos)
            {
                attributes_documents += @"{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""Documento"",
                                        ""EntityAlias"":""Documento"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""EmpresaId"",
                                                                ""AttrValue"":" + IdEmpresa + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""EmpresaId""
                                                            },
                                                            {
                                                                ""AttrName"":""Nombre"",
                                                                ""AttrValue"":"""+ doc +@""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Nombre""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":true
                        " + "\n" +
                        @"        }";
                if (large - 1 > index_docs) 
                {
                    attributes_documents += ",";
                };
                index_docs++;
            }

            request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            body = @"
                        " + "\n" +
                        @"        ["+ attributes_documents + @"]
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);
            return "IdEmpresa: " + IdEmpresa + "Respuesta: " + response.Content;
        }

        protected void btnRegistrarEmpresa_Click(object sender, EventArgs e)
        {
            

        }
    }
}