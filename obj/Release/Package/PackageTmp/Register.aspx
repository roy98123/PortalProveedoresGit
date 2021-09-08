<%--<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Grow.PortalProveedores.Login" %>--%>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Grow.PortalProveedores.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:BundleReference runat="server" Path="~/Content/css" />

    
  <!--[if lt IE 9]>
      <script src="js/vendor/html5shiv.min.js"></script>
      <script src="js/vendor/respond.min.js"></script>
  <![endif]-->

  <!-- Google Font: Source Sans Pro -->
  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
  <!-- Font Awesome Icons -->
  <link rel="stylesheet" href="Content/plugins/fontawesome-free/css/all.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="Content/dist/css/adminlte.min.css">



    <title>Login</title>
</head>
<body>

    <form id="form1" runat="server" autocomplete="off" style="padding: 0% 5% 0 5%;">


        <section class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h3>Registro de Proveedores</h3>
          </div>
          <div class="col-sm-6">
            <ol style="background-color: white;" class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Login.aspx">Inicio</a></li>
              <li class="breadcrumb-item active">Registro de Solicitud</li>
            </ol>
          </div>
        </div>
      </div><!-- /.container-fluid -->
    </section>



      <div class="container-fluid ">
        <div class="row ">
          <!-- left column -->
          <div class="col-md-12">
            <!-- general form elements -->
            <div class="card card-primary">
              <div class="card-header bg-primary text-white  ">
                <h3 style="font-size: 15px;" class="card-title">Registro de Solicitud</h3>
              </div>
                    
                <div class="row">

                     <div class="col">

                         <div style="margin:20px;">
                             <form autocomplete="off" id="formRegistro">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="razon">Razón Social</label>
                                    <input type="text" class="form-control " id="razon" runat="server"  style="max-width: inherit" placeholder="Razón Social" >
                                    <div class="text-danger" hidden  runat="server" id="divErrorRazon">
                                        <small class="text-formulario"><b>Ingresa razón social para poder continuar</b></small>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="rfc">RFC</label>
                                    <input type="text" class="form-control" id="rfc" style="max-width: inherit"  runat="server" placeholder="RFC" >
                                    <div class="text-danger" hidden runat="server" id="divErrorRFC">
                                        <small class="text-formulario"><b>Ingresa tu rfc para poder continuar</b></small>
                                    </div>
                                </div>
                            </div>
                        </div>
        
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="correo">Correo electrónico</label>
                                    <input type="email" class="form-control " id="correo"  style="max-width: inherit" runat="server" aria-describedby="emailHelp" placeholder="Correo electrónico" >
                                    <div class="text-danger" hidden runat="server" id="divErrorCorreo">
                                        <small class="text-formulario"><b>Ingresa tu correo electrónico para poder continuar</b></small>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="contrasena">Contraseña</label>
                                    <input type="password" class="form-control" id="contrasena"  style="max-width: inherit" runat="server" placeholder="Contraseña" >
                                    <div class="text-danger" hidden runat="server" id="divErrorContra">
                                        <small class="text-formulario"><b>Ingresa tu contraseña para poder continuar</b></small>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <br />

                        <div class="row">

                            <div class="col">
                                 
                                <button type="button"  style="max-width: inherit; width:100;" class="btn btn-secondary  btn-lg btn-block" id="btnAgregarEmpresa" disabled>Agregar empresa</button>

                            </div>

                            <div class="col">
                                <button type="submit" class="btn btn-primary  btn-lg btn-block"  style="max-width: inherit; width:100;" id="btnPrueba">Completar Registro</button>   
                            </div>

                        </div>
                        

                        <input type="text" hidden runat="server" id="Text1"/>
                        <input type="text" hidden runat="server" id="Text2"/>
        
                   
                   
                        <%--<asp:Button ID="btnRegistrar" runat="server" class="form-control btn btn-primary btn-lg btn-block" OnClick="btnRegistrar_Click" Text="Registrarse" AutoPostBack="True"/>--%>

            
                    
                    </form>
                         </div>
                                                     

                     </div>

                     <div class="col" >

                         <div style="margin:20px;">

                         <div style= "height: 450px; overflow-y: scroll;" class="container">
                        <div class="alert alert-info" role="alert">
                          Selecciona una empresa (o más)
                        </div>
                    <div>
                    <select style="max-width: inherit; width:100;" id="empresas0" onchange="Documentos(0)" class="form-control select2bs4" name="state">
                            
                    </select>
                </div>
                   
                <div id="Documentos0">
                </div>

                <div id="ContenidoDinamico">

                </div>
            </div>

                     </div>
                </div>

                </div>
              
            </div>
            <!-- /.card -->

             <input type="text"  runat="server" id="Documentos"/>
                <input type="text"  runat="server" id="Empresas"/>

      </div><!-- /.container-fluid -->


        
       </form>

    <div class=" body-content">
        <br />
        <footer class="text-center">
            <p>&copy; <%: DateTime.Now.Year %> - Portal Proveedores</p>
        </footer>
    </div>

    <script src="Content/js/vendor/bootstrap.min.js"></script>
    <script src="Content/js/app.js"></script>
    
    <script src="Content/js/vendor/bootstrap.min.js"></script>
     <script src="Content/js/alertas.js"></script>
    <script src="Content/js/Register.js"></script>
    <%--<script src="Content/js/login.js"></script>--%>
        
       
</body>
</html>