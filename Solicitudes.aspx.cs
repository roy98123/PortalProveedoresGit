using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
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
    public partial class Solicitudes : System.Web.UI.Page
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
                    @"              ""connStr"":null,
                                    ""EntityName"":""SolicitudAlta"",
                                    ""EntityAlias"":""SolicitudAlta"",
                                    ""PKId"":1,
                                    ""Action"":0,
                                    ""GroupWheres"":[{
                                        ""LogicalOperatorWhere"": 1,
                                        ""logicalOperator"": 1,
                                        ""listWhere"": [{
                                            ""whereOperator"": 1,
                                            ""Attribute"": {
                                                ""AttrName"": ""Estatus"",
                                                ""AttrValue"": 0,
                                                ""AttrType"": ""int"",
                                                ""AttrAlias"": ""Estatus""
                                            }
                                        }]
                                    }],
                                    ""Attributes"":[{
                                                        ""AttrName"":""Id"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""Id""
                                                    },
                                                    {
                                                        ""AttrName"":""RazonSocial"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""RazonSocial""
                                                    },
                                                    {
                                                        ""AttrName"":""RFC"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""RFC""
                                                    },
                                                    {
                                                        ""AttrName"":""CorreoElectronico"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""CorreoElectronico""
                                                    }],
                                    ""ChildEntities"":[],
                                    ""getLastIdentity"":false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            JObject responseContent = JObject.Parse(response.Content);
                
            foreach (JObject i in responseContent["RetVal"])
            {
                tbody_solicitudes.InnerHtml += @"<tr>" +
                                                    @"<td>" + i["Attributes"][0]["AttrValue"] +  @"</td>" +
                                                    @"<td>" + i["Attributes"][1]["AttrValue"] + @"</td>" +
                                                    @"<td>" + i["Attributes"][3]["AttrValue"] + @"</td>" +
                                                    @"<td class='text-center'> 
                                                        <button type='button' " + i["Attributes"][0]["AttrValue"] + @"' class='btn btn-primary btninfo' data-toggle='modal' data-target='#modal-info-solicitud' onclick='getInfo(" + i["Attributes"][0]["AttrValue"] + @")'> 
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
                                        ""EntityName"": ""SolicitudAlta"",
                                        ""EntityAlias"": ""SolicitudAlta"",
                                        ""PKId"": 0,
                                        ""Action"": 0,
                                        ""GroupWheres"": [{
                                            ""LogicalOperatorWhere"": 1,
                                            ""logicalOperator"": 1,
                                            ""listWhere"": [{
                                                ""whereOperator"": 1,
                                                ""Attribute"": {
                                                    ""AttrName"": ""Id"",
                                                    ""AttrValue"": """ + solicitud.id + @""",
                                                    ""AttrType"": ""int"",
                                                    ""AttrAlias"": ""Id""
                                                }
                                            }]

                                        }],
                                        ""Attributes"": [{
                                            ""AttrName"": ""Id"",
                                            ""AttrValue"": ""1"",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""Id""
                                        },
                                        {
                                            ""AttrName"": ""RazonSocial"",
                                            ""AttrValue"": ""1"",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""RazonSocial""
                                        },
                                        {
                                            ""AttrName"": ""RFC"",
                                            ""AttrValue"": ""1"",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""RFC""
                                        },
                                        {
                                            ""AttrName"": ""CorreoElectronico"",
                                            ""AttrValue"": ""1"",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""CorreoElectronico""
                                        },
                                        {
                                            ""AttrName"": ""Estatus"",
                                            ""AttrValue"": ""1"",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""Estatus""
                                        }],
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

        [WebMethod]
        public static string getEmpresasDocs(Solicitud solicitud)
        {

            //Empresas

            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/GetList";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"": null,
                                            ""EntityName"": ""SolicitudEmpresa"",
                                            ""EntityAlias"": ""SolicitudEmpresa"",
                                            ""PKId"": 0,
                                            ""Action"": 0,
                                            ""GroupWheres"": [{
                                                ""LogicalOperatorWhere"": 1,
                                                ""logicalOperator"": 1,
                                                ""listWhere"": [{
                                                    ""whereOperator"": 1,
                                                    ""Attribute"": {
                                                        ""AttrName"": ""IdSolicitud"",
                                                        ""AttrValue"": """ + solicitud.id + @""",
                                                        ""AttrType"": ""int"",
                                                        ""AttrAlias"": ""IdSolicitud""
                                                    }
                                                }]
                                            }],
                                            ""Attributes"": [{
                                                ""AttrName"": ""IdEmpresa"",
                                                ""AttrValue"": """",
                                                ""AttrType"": """",
                                                ""AttrAlias"": ""IdEmpresa""
                                            }],
                                            ""ChildEntities"": [{
                                                ""logicOperator"": 1,
                                                ""ChildEntity"": {
                                                    ""EntityName"": ""Empresa"",
                                                    ""EntityAlias"": ""Empresa"",
                                                    ""Attributes"": [
                                                        {
                                                            ""AttrName"": ""RazonSocial"",
                                                            ""AttrValue"": """",
                                                            ""AttrType"": """",
                                                            ""AttrAlias"": ""RazonSocial""
                                                        }
                                                    ],
                                                    ""GroupWheres"": [],
                                                    ""ChildEntities"": [],
                                                    ""getLastIdentity"": false,
                                                    ""connStr"": null,
                                                    ""PKId"": 0,
                                                    ""Action"": 0
                                                },
                                                ""JoinType"": 2,
                                                ""selectJoinList"": [{
                                                    ""logicOperator"": 1,
                                                    ""mainAttr"": {
                                                        ""AttrName"": ""IdEmpresa"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""IdEmpresa""
                                                    },
                                                    ""childAttr"": {
                                                        ""AttrName"": ""Id"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""Id""
                                                    }
                                                }]
                                            }],
                                            ""getLastIdentity"": false
                        " + "\n" +
                        @"        }]
                        " + "\n" +
                        @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            JArray JAListaEmpresas = new JArray();
            foreach (dynamic i in responseContent[0].RetVal)
            {
                JAListaEmpresas.Add(i.Attributes);
            }

            //Documentos

            request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"": null,
                                            ""EntityName"": ""SolicitudDocumento"",
                                            ""EntityAlias"": ""SolicitudDocumento"",
                                            ""PKId"": 0,
                                            ""Action"": 0,
                                            ""GroupWheres"": [{
                                                ""LogicalOperatorWhere"": 1,
                                                ""logicalOperator"": 1,
                                                ""listWhere"": [{
                                                    ""whereOperator"": 1,
                                                    ""Attribute"": {
                                                        ""AttrName"": ""IdSolicitud"",
                                                        ""AttrValue"": """ + solicitud.id + @""",
                                                        ""AttrType"": ""int"",
                                                        ""AttrAlias"": ""IdSolicitud""
                                                    }
                                                }]
                                            }],
                                            ""Attributes"": [{
                                                ""AttrName"": ""NombreDocumento"",
                                                ""AttrValue"": """",
                                                ""AttrType"": """",
                                                ""AttrAlias"": ""NombreDocumento""
                                            },
                                            {
                                                ""AttrName"": ""ContenidoDocumento"",
                                                ""AttrValue"": """",
                                                ""AttrType"": """",
                                                ""AttrAlias"": ""ContenidoDocumento""
                                            }],
                                            ""ChildEntities"": [{
                                                ""logicOperator"": 1,
                                                ""ChildEntity"": {
                                                    ""EntityName"": ""Documento"",
                                                    ""EntityAlias"": ""Documento"",
                                                    ""Attributes"": [
                                                        {
                                                            ""AttrName"": ""EmpresaId"",
                                                            ""AttrValue"": """",
                                                            ""AttrType"": """",
                                                            ""AttrAlias"": ""EmpresaId""
                                                        },
                                                        {
                                                            ""AttrName"": ""Nombre"",
                                                            ""AttrValue"": """",
                                                            ""AttrType"": """",
                                                            ""AttrAlias"": ""Nombre""
                                                        }
                                                    ],
                                                    ""GroupWheres"": [],
                                                    ""ChildEntities"": [],
                                                    ""getLastIdentity"": false,
                                                    ""connStr"": null,
                                                    ""PKId"": 0,
                                                    ""Action"": 0
                                                },
                                                ""JoinType"": 2,
                                                ""selectJoinList"": [{
                                                    ""logicOperator"": 1,
                                                    ""mainAttr"": {
                                                        ""AttrName"": ""IdDocumento"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""IdDocumento""
                                                    },
                                                    ""childAttr"": {
                                                        ""AttrName"": ""Id"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""Id""
                                                    }
                                                }]
                                            }],
                                            ""getLastIdentity"": false
                        " + "\n" +
                        @"        }]
                        " + "\n" +
                        @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);

            responseContent = JsonConvert.DeserializeObject(response.Content);
            JArray JAListaDocumentos = new JArray();
            foreach (dynamic i in responseContent[0].RetVal)
            {
                JAListaDocumentos.Add(i.Attributes);
            }

            //Llenado de empresas en front

            string divLista = "";

            foreach (var emps in JAListaEmpresas)
            {
                string elementsLI = "";
                string elements = "";
                foreach (var docs in JAListaDocumentos)
                {
                    if (docs[2]["AttrValue"].ToString() == emps[0]["AttrValue"].ToString())
                    {
                        elementsLI += "<li>" + docs[3]["AttrValue"].ToString() + ": <a href=\"" + docs[1]["AttrValue"].ToString() + "\" download=\"" + docs[0]["AttrValue"].ToString() + "\">" + docs[0]["AttrValue"].ToString() + "</a></li>";
                    }
                }
                elements += "<div class=\"col-md-12\">";
                elements += "<h5>" + emps[1]["AttrValue"].ToString() + "</h5>";
                elements += "<ul>";
                elements += elementsLI;
                elements += "</ul>";
                elements += "</div>";

                divLista += elements;
            }
            return divLista;
        }

        /*[WebMethod]
        public static string getSolicitudById(Solicitud solicitud)
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
                                        ""EntityName"": ""SolicitudAlta"",
                                        ""EntityAlias"": ""SolicitudAlta"",
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
                                                                                                  ""AttrName"": ""Id"",
                                                                                                  ""AttrValue"": """ + solicitud.id + @""",
                                                                                                  ""AttrType"": ""int"",
                                                                                                  ""AttrAlias"": ""Id""
                                                                                                  }
                                                                              }
                                                                          ]

                                                          }
                                                      ],
                                        ""Attributes"": [
                                          {
                                            ""AttrName"": ""Id"",
                                            ""AttrValue"": """",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""IdSolicitud""
                                          }

                                        ],
                                        ""ChildEntities"": [
                                          {
                                            ""logicOperator"": 1,
                                            ""ChildEntity"": {
                                              ""EntityName"": ""SolicitudEmpresa"",
                                              ""EntityAlias"": ""SolicitudEmpresa"",
                                              ""Attributes"": [],
                                              ""GroupWheres"": [],
                                              ""ChildEntities"": [
                                                {
                                                  ""logicOperator"": 1,
                                                  ""ChildEntity"": {
                                                    ""EntityName"": ""Empresa"",
                                                    ""EntityAlias"": ""Empresa"",
                                                    ""Attributes"": [
                                                      {
                                                         ""AttrName"": ""Id"",
                                                         ""AttrValue"": """",
                                                         ""AttrType"": """",
                                                         ""AttrAlias"": ""IdEmpresa""
                                                       },
                                                       {
                                                         ""AttrName"": ""RazonSocial"",
                                                         ""AttrValue"": """",
                                                         ""AttrType"": """",
                                                         ""AttrAlias"": ""RazonSocial""
                                                       }
                                                    ],
                                                    ""GroupWheres"": [],
                                                    ""ChildEntities"": [],
                                                    ""getLastIdentity"": false,
                                                    ""connStr"": null,
                                                    ""PKId"": 0,
                                                    ""Action"": 0
                                                  },
                                                  ""JoinType"": 2,
                                                  ""selectJoinList"": [
                                                    {
                                                      ""logicOperator"": 1,
                                                      ""mainAttr"": {
                                                        ""AttrName"": ""IdEmpresa"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""IdEmpresa""
                                                      },
                                                      ""childAttr"": {
                                                        ""AttrName"": ""Id"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""Id""
                                                      }
                                                    }
                                                  ]
                                                }
                                              ],
                                              ""getLastIdentity"": false,
                                              ""connStr"": null,
                                              ""PKId"": 0,
                                              ""Action"": 0
                                            },
                                            ""JoinType"": 2,
                                            ""selectJoinList"": [
                                              {
                                                ""logicOperator"": 1,
                                                ""mainAttr"": {
                                                  ""AttrName"": ""Id"",
                                                  ""AttrValue"": """",
                                                  ""AttrType"": """",
                                                  ""AttrAlias"": ""Id""
                                                },
                                                ""childAttr"": {
                                                  ""AttrName"": ""IdSolicitud"",
                                                  ""AttrValue"": """",
                                                  ""AttrType"": """",
                                                  ""AttrAlias"": ""IdSolicitud""
                                                }
                                              }
                                            ]
                                          }
                                        ],
                                        ""getLastIdentity"": false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            return response.Content;

        }*/

        /*[WebMethod]
        public static string getEmpresaById(Documentos empresa, Solicitud solicitud)
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
                       @"                                                                                                                                  
                                    ""connStr"": null,
                                    ""EntityName"": ""SolicitudDocumento"",
                                    ""EntityAlias"": ""SolicitudDocumento"",
                                    ""PKId"": 0,
                                    ""Action"": 0,
                                    ""GroupWheres"": [{
                                        ""LogicalOperatorWhere"": 1,
                                        ""logicalOperator"": 2,
                                            ""listWhere"": [{
                                                    ""whereOperator"": 1,
                                                    ""Attribute"": {
                                                    ""AttrName"": ""IdSolicitud"",
                                                    ""AttrValue"": """ + solicitud.id + @""",
                                                    ""AttrType"": ""int"",
                                                    ""AttrAlias"": ""IdSolicitud""
                                                    }
                                                }]
                                    }],
                                    ""Attributes"": [{
                                            ""AttrName"": ""NombreDocumento"",
                                            ""AttrValue"": """",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""NombreDocumento""
                                        },
                                        {
                                            ""AttrName"": ""ContenidoDocumento"",
                                            ""AttrValue"": """",
                                            ""AttrType"": """",
                                            ""AttrAlias"": ""ContenidoDocumento""
                                    }],
                                    ""ChildEntities"": [{
                                        ""logicOperator"": 1,
                                        ""ChildEntity"": {
                                            ""EntityName"": ""Documento"",
                                            ""EntityAlias"": ""Documento"",
                                            ""Attributes"": [],
                                            ""GroupWheres"": [],
                                            ""ChildEntities"": [{
                                                ""logicOperator"": 1,
                                                ""ChildEntity"": {
                                                    ""EntityName"": ""Empresa"",
                                                    ""EntityAlias"": ""Empresa"",
                                                    ""Attributes"": [{
                                                        ""AttrName"": ""RazonSocial"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""RazonSocial""
                                                    }],
                                                    ""GroupWheres"": [{
                                                        ""LogicalOperatorWhere"": 1,
                                                        ""logicalOperator"": 2,
                                                        ""listWhere"": [{
                                                            ""whereOperator"": 1,
                                                            ""Attribute"": {
                                                                ""AttrName"": ""Id"",
                                                                ""AttrValue"": """ + empresa.empresa + @""",
                                                                ""AttrType"": ""int"",
                                                                ""AttrAlias"": ""Id""
                                                            }
                                                        }]
                                                    }],
                                                    ""ChildEntities"": [],
                                                    ""getLastIdentity"": false,
                                                    ""connStr"": null,
                                                    ""PKId"": 0,
                                                    ""Action"": 0
                                                },
                                                ""JoinType"": 2,
                                                ""selectJoinList"": [{
                                                    ""logicOperator"": 1,
                                                    ""mainAttr"": {
                                                        ""AttrName"": ""EmpresaId"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""EmpresaId""
                                                    },
                                                    ""childAttr"": {
                                                        ""AttrName"": ""Id"",
                                                        ""AttrValue"": """",
                                                        ""AttrType"": """",
                                                        ""AttrAlias"": ""Id""
                                                    }
                                                }]
                                            }],
                                            ""getLastIdentity"": false,
                                            ""connStr"": null,
                                            ""PKId"": 0,
                                            ""Action"": 0
                                        },
                                        ""JoinType"": 2,
                                        ""selectJoinList"": [{
                                            ""logicOperator"": 1,
                                            ""mainAttr"": {
                                                ""AttrName"": ""IdDocumento"",
                                                ""AttrValue"": """",
                                                ""AttrType"": """",
                                                ""AttrAlias"": ""IdDocumento""
                                            },
                                            ""childAttr"": {
                                                ""AttrName"": ""Id"",
                                                ""AttrValue"": """",
                                                ""AttrType"": """",
                                                ""AttrAlias"": ""Id""
                                            }
                                        }]
                                    }],
                                    ""getLastIdentity"": true    
                                        
                        " + "\n" +
                       @"        }
                        " + "\n" +
                       @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }*/


        [WebMethod]
        public static string ActualizarEstatus(Solicitud solicitud)
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
                                            ""AttrValue"": " + solicitud.id + @",
                                            ""AttrType"": ""int"",
                                            ""AttrAlias"": ""Id""
                                        }
                                    }]
                                }],
                                ""Attributes"":[{
                                    ""AttrName"":""Estatus"",
                                    ""AttrValue"":" + solicitud.op + @",
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

            // Tomando los datos de la solicitud para posteriomente mandarlos a B2T
            // Realizo la petición hacia a la API
            if(solicitud.op == 1)
            {
                server = ConfigurationManager.AppSettings["Host"].ToString();
                _urlApi = "api/IncomingMessage/GetEntity";
                client = new RestClient(server + _urlApi);
                client.Timeout = -1;
                request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
                 body = @"
                        " + "\n" +
                            @"        {
                    " + "\n" +
                        @"              ""connStr"":null,
                                    ""EntityName"":""SolicitudAlta"",
                                    ""EntityAlias"":""SolicitudAlta"",
                                    ""PKId"":1,
                                    ""Action"":0,
                                    ""GroupWheres"":[{
                                        ""LogicalOperatorWhere"": 1,
                                        ""logicalOperator"": 1,
                                        ""listWhere"": [{
                                            ""whereOperator"": 1,
                                            ""Attribute"": {
                                                ""AttrName"": ""Id"",
                                                ""AttrValue"": " + solicitud.id + @",
                                                ""AttrType"": ""int"",
                                                ""AttrAlias"": ""Estatus""
                                            }
                                        }]
                                    }],
                                    ""Attributes"":[{
                                                        ""AttrName"":""Id"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""Id""
                                                    },
                                                    {
                                                        ""AttrName"":""RazonSocial"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""RazonSocial""
                                                    },
                                                    {
                                                        ""AttrName"":""RFC"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""RFC""
                                                    },
                                                    {
                                                        ""AttrName"":""CorreoElectronico"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""CorreoElectronico""
                                                    },
                                                    {
                                                        ""AttrName"":""Contraseña"",
                                                        ""AttrValue"":""1"",
                                                        ""AttrType"":"""",
                                                        ""AttrAlias"":""Contraseña""
                                                    }],
                                    ""ChildEntities"":[],
                                    ""getLastIdentity"":false
                        " + "\n" +
                            @"        }
                        " + "\n" +
                            @"";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                response = client.Execute(request);


                JObject responseContent = JObject.Parse(response.Content);
                var razonSocial = (String)responseContent["RetVal"][0]["Attributes"][1]["AttrValue"];
                var rfc = (String)responseContent["RetVal"][0]["Attributes"][2]["AttrValue"];
                var correoElectronico = (String)responseContent["RetVal"][0]["Attributes"][3]["AttrValue"];
                var contrasena = (String)responseContent["RetVal"][0]["Attributes"][4]["AttrValue"];

                // Registrando al usuario en B2C
                try
                {
                    GraphEntity testEntity = new GraphEntity(GraphEntity.GraphEntityType.User);

                    GraphField testField = new GraphField("DisplayName", false, razonSocial, GraphField.GraphFieldType.String);
                    testEntity.EntityFields.Add(testField);
                    //testField = new GraphField("Password", false, "8567.Adios.4", GraphField.GraphFieldType.Password); // Cambiar nivel de dificultad
                    // Cambiar nivel de dificultad
                    testField = new GraphField("Password", false, contrasena, GraphField.GraphFieldType.Password); 

                    testEntity.EntityFields.Add(testField);
                    //testField = new GraphField("GRWB2C.onmicrosoft.com", false, ConfigurationManager.AppSettings["Conection"].ToString(), GraphField.GraphFieldType.Username);
                    //testEntity.EntityFields.Add(testField);
                    testField = new GraphField("RazonSocial", true, razonSocial, GraphField.GraphFieldType.String);
                    testEntity.EntityFields.Add(testField);
                    testField = new GraphField("RFC", true, rfc, GraphField.GraphFieldType.String);
                    testEntity.EntityFields.Add(testField);
                    testField = new GraphField(ConfigurationManager.AppSettings["Issuer"].ToString(), false, correoElectronico, GraphField.GraphFieldType.Username);
                    testEntity.EntityFields.Add(testField);


                    GraphConnector DAC_Graph = new GraphConnector(ConfigurationManager.AppSettings["AppId"].ToString(), ConfigurationManager.AppSettings["Secret"].ToString(), ConfigurationManager.AppSettings["Extension"].ToString(), ConfigurationManager.AppSettings["WebToken"].ToString());
                    GraphResult result = DAC_Graph.insertEntityRecord(testEntity);

                }
                catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }
            }



            return response.Content;
        }        
    }
}