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
using RestSharp;
using Grow.PortalProveedores.Authentication;

namespace Grow.PortalProveedores
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/GetEntity";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            // EMPRESAS
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"        {
                        " + "\n" +
                        @"  ""connStr"":null,""EntityName"":""Empresa"",""EntityAlias"":""Empresa"",""PKId"":1,""Action"":0,""GroupWheres"":[],""Attributes"":
                            [{""AttrName"":""RazonSocial"",""AttrValue"":""1"",""AttrType"":"""",""AttrAlias"":""RazonSocial""},
                            {""AttrName"":""Id"",""AttrValue"":""1"",""AttrType"":"""",""AttrAlias"":""Id""}],
                            ""ChildEntities"":[],""getLastIdentity"":false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);             
            Empresas.Value = response.Content;
            // DOCUMENTOS
            request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            body = @"
                        " + "\n" +
                        @"        {
                        " + "\n" +
                        @"  ""connStr"":null,""EntityName"":""Documento"",""EntityAlias"":""Documento"",""PKId"":1,""Action"":0,""GroupWheres"":[],""Attributes"":
                            [{""AttrName"":""Id"",""AttrValue"":""1"",""AttrType"":"""",""AttrAlias"":""Id""},
                            {""AttrName"":""EmpresaId"",""AttrValue"":""1"",""AttrType"":"""",""AttrAlias"":""EmpresaId""},
                            {""AttrName"":""Nombre"",""AttrValue"":""1"",""AttrType"":"""",""AttrAlias"":""Nombre""}],
                            ""ChildEntities"":[],""getLastIdentity"":false
                        " + "\n" +
                        @"        }
                        " + "\n" +
                        @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            response = client.Execute(request);
            Documentos.Value = response.Content;
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            
            if (razon.Value != "" && rfc.Value != "" && contrasena.Value != "" && correo.Value != "")
            {
                try
                {
                    GraphEntity testEntity = new GraphEntity(GraphEntity.GraphEntityType.User);
                    
                    GraphField testField = new GraphField("DisplayName", false, razon.Value, GraphField.GraphFieldType.String);                   
                    testEntity.EntityFields.Add(testField);
                    testField = new GraphField("Password", false, contrasena.Value, GraphField.GraphFieldType.Password);
                    testEntity.EntityFields.Add(testField);
                    //testField = new GraphField("GRWB2C.onmicrosoft.com", false, ConfigurationManager.AppSettings["Conection"].ToString(), GraphField.GraphFieldType.Username);
                    //testEntity.EntityFields.Add(testField);
                    testField = new GraphField("RazonSocial", true, razon.Value, GraphField.GraphFieldType.String);
                    testEntity.EntityFields.Add(testField);
                    testField = new GraphField("RFC", true, rfc.Value, GraphField.GraphFieldType.String);
                    testEntity.EntityFields.Add(testField);
                    testField = new GraphField(ConfigurationManager.AppSettings["Issuer"].ToString(), false, correo.Value, GraphField.GraphFieldType.Username);
                    testEntity.EntityFields.Add(testField);


                    GraphConnector DAC_Graph = new GraphConnector(ConfigurationManager.AppSettings["AppId"].ToString(), ConfigurationManager.AppSettings["Secret"].ToString(), ConfigurationManager.AppSettings["Extension"].ToString(), ConfigurationManager.AppSettings["WebToken"].ToString());
                    GraphResult result = DAC_Graph.insertEntityRecord(testEntity);
                } catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }
            }
        }


        //Codigo respuesta a AJAX

        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public static string Registrar(Registro proveedor)
        {
            string server = ConfigurationManager.AppSettings["Host"].ToString();
            string _urlApi = "api/IncomingMessage/MultipleInsert";
            var client = new RestClient(server + _urlApi);
            client.Timeout = -1;
            
            //Solicitud
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c");
            var body = @"
                        " + "\n" +
                        @"        [{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""SolicitudAlta"",
                                        ""EntityAlias"":""SolicitudAlta"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""RazonSocial"",
                                                                ""AttrValue"":""" + proveedor.razon + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""RazonSocial""
                                                            },
                                                            {
                                                                ""AttrName"":""RFC"",
                                                                ""AttrValue"":""" + proveedor.rfc + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""RFC""
                                                            },
                                                            {
                                                                ""AttrName"":""CorreoElectronico"",
                                                                ""AttrValue"":""" + proveedor.correo + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""CorreoElectronico""
                                                            },
                                                            {
                                                                ""AttrName"":""Contraseña"",
                                                                ""AttrValue"":""" + proveedor.contrasena + @""",
                                                                ""AttrType"":""string"",
                                                                ""AttrAlias"":""Contraseña""
                                                            },
                                                            {
                                                                ""AttrName"":""Estatus"",
                                                                ""AttrValue"":0,
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""Estatus""
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

            //string resultado = response.Content;

            dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
            int IdSolicitud = 0;
            foreach (dynamic i in responseContent)
            {
                IdSolicitud = i.returnedId;
            }

            //Empresas
            string attributes = "";
            for (int i = 0; i < proveedor.empresas.Length; i++)
            {
                attributes += @"{
                        " + "\n" +
                        @"              ""connStr"":null,
                                        ""EntityName"":""SolicitudEmpresa"",
                                        ""EntityAlias"":""SolicitudEmpresa"",
                                        ""PKId"":0,
                                        ""Action"":0,
                                        ""GroupWheres"":[],
                                        ""Attributes"":[
                                                            {
                                                                ""AttrName"":""IdSolicitud"",
                                                                ""AttrValue"":" + IdSolicitud + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""IdSolicitud""
                                                            },
                                                            {
                                                                ""AttrName"":""IdEmpresa"",
                                                                ""AttrValue"":" + proveedor.empresas[i].idEmpresa + @",
                                                                ""AttrType"":""int"",
                                                                ""AttrAlias"":""IdEmpresa""
                                                            }
                                                        ],
                                        ""ChildEntities"":[],
                                        ""getLastIdentity"":false
                        " + "\n" +
                        @"        }";
                if ((proveedor.empresas.Length - 1) > i)
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

            //Documentos
            
            for (int i = 0; i < proveedor.empresas.Length; i++)
            {
                attributes = "";
                for (int j = 0; j < proveedor.empresas[i].documentos.Length; j++)
                {
                    attributes += @"{
                    " + "\n" +
                            @"          ""connStr"":null,
                                    ""EntityName"":""SolicitudDocumento"",
                                    ""EntityAlias"":""SolicitudDocumento"",
                                    ""PKId"":0,
                                    ""Action"":0,
                                    ""GroupWheres"":[],
                                    ""Attributes"":[
                                                        {
                                                            ""AttrName"":""IdSolicitud"",
                                                            ""AttrValue"":" + IdSolicitud + @",
                                                            ""AttrType"":""int"",
                                                            ""AttrAlias"":""IdSolicitud""
                                                        },
                                                        {
                                                            ""AttrName"":""IdDocumento"",
                                                            ""AttrValue"":" + proveedor.empresas[i].documentos[j].idDocumento + @",
                                                            ""AttrType"":""int"",
                                                            ""AttrAlias"":""IdDocumento""
                                                        },
                                                        {
                                                            ""AttrName"":""NombreDocumento"",
                                                            ""AttrValue"":""" + proveedor.empresas[i].documentos[j].nombre + @""",
                                                            ""AttrType"":""string"",
                                                            ""AttrAlias"":""NombreDocumento""
                                                        },
                                                        {
                                                            ""AttrName"":""ContenidoDocumento"",
                                                            ""AttrValue"":""" + proveedor.empresas[i].documentos[j].contenido + @""",
                                                            ""AttrType"":""string"",
                                                            ""AttrAlias"":""ContenidoDocumento""
                                                        }
                                                    ],
                                    ""ChildEntities"":[],
                                    ""getLastIdentity"":false
                    " + "\n" +
                            @"        }";
                    if ((proveedor.empresas[i].documentos.Length - 1) > j)
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
            }                        

            return response.Content;

        }


        [WebMethod]
        public static string Empresa(Solicitud solicitud)
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
                                        ""EntityName"": ""Empresa"",
                                        ""EntityAlias"": ""Empresa"",
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
                                        ""Attributes"": [],
                                        ""ChildEntities"": [],
                                        ""getLastIdentity"": false
                        " + "\n" +
                       @"        }
                        " + "\n" +
                       @"";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }


    }
}