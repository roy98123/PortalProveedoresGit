<%--<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Grow.PortalProveedores.Login" %>--%>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Grow.PortalProveedores.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:BundleReference runat="server" Path="~/Content/css" />

     <style>
        body{
            background-color: #000000;
            padding: 0px;
            margin: 0px;
            }

          #gradient
          {
          width: 100%;
          height: 100%;
          padding: 0px;
          margin: 0px;
          }
    </style>

    <link href="Content/js/vendor/jquery.min.js" rel="stylesheet" />
      <!-- Google Font: Source Sans Pro -->
      <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
      <!-- Font Awesome Icons -->
      <link rel="stylesheet" href="Content/plugins/fontawesome-free/css/all.min.css">
      <!-- Theme style -->
      <link rel="stylesheet" href="Content/dist/css/adminlte.min.css">


      <!-- Google Font: Source Sans Pro -->
      <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
      <!-- Font Awesome Icons -->
      <link rel="stylesheet" href="Content/plugins/fontawesome-free/css/all.min.css">
      <!-- Theme style -->
      <link rel="stylesheet" href="Content/dist/css/adminlte.min.css">

    <title>Login</title>
</head>
<body id="gradient">
    <form id="form1" runat="server">
        <div>
            <div class="container">
                <br/>
        
                <center>
                    <div class="card shadow mb-4 bg-body rounded container-card jumbotron" style="width: 400px; ">
                        <div class="position-relative text-center ">
                            <div>
                                <h4 class="txt-acceder">Acceder</h4>
                            </div>
                            <div>
                                <span>Bienvenido al Portal de Proveedores</span> <span runat="server" id="displayNameSpan"></span>    
                            </div>
                        </div>
                        <br />

                       <center>
                            <img style="width: 200px;" class="" src="Content/images/logo-removebg-preview.png" />
                       </center>
                            <%--<form id="formLogin">
                                <div class="form-group" clas>
                                    <label for="inputCorreo">Correo</label>
                                    <input type="email" class="form-control" w placeholder="Correo" runat="server" id="inputCorreo"/>
                                    <small class="text-danger" runat="server" id="error_correo"></small>
                                </div>
    
                                <div class="form-group">
                                    <label for="inputContrasena">Contraseña</label>
                                    <input type="password" class="form-control" id="inputContrasena" runat="server" placeholder="Contraseña"/>
                                    <small class="text-danger" runat="server" id="error_contrasena"></small>
                                </div>
    
                                <asp:Button type="button" ID="btnLogin" runat="server" class="form-control btn btn-primary btn-lg btn-block" Text="Iniciar Sesión"  AutoPostBack="True" OnClick="btnLogin_Click"/>
                                <div class="form-group">
                                    <button type="button" class="btn btn-default" id="btnIniciarSesion" onclick="validarFormLogin()">Iniciar Sesión</button>
                                </div>
                                <small class="text-danger" id="error_formulario"></small>
                            </form>--%>
                        <div class="form-group text-center">
                            <asp:button text="Haz click aquí para Iniciar Sesión" CssClass="btn btn-primary" runat="server" ID="btnIngresar" OnClick="btnIngresar_Click" />
                        </div>
                        <div class="text-center" style="padding-bottom:10px;">
                            <a runat="server" href="~/Register.aspx"><small>¿No tienes una cuenta? Haz click aquí para crear una</small></a>
                        </div>
                    </div>
                </center>
                
            </div>
        </div>
        <div class="container body-content">
            <br /><br />
        <footer style="margin-bottom: 15px; color: white;" class="text-center">
            <p>&copy; <%: DateTime.Now.Year %> - Portal Proveedores</p>
        </footer>
        </div>
    </form>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.0.2/jquery.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-gtEjrD/SeCtmISkJkNUaaKMoLD0//ElJ19smozuHV6z3Iehds+3Ulb9Bn9Plx0x4" crossorigin="anonymous"></script>
    <script src="Content/js/vendor/bootstrap.min.js"></script>
    <script src="Content/js/app.js"></script>
    <%--<script src="Content/js/login.js"></script>--%>
    <script>

var colors = new Array(
    [0,127,255],
    [75,146,219],
    [0,63,255],
    [0,51,153],
    [75,146,219],
    [0,63,125]);
  
  var step = 0;
  //color table indices for: 
  // current color left
  // next color left
  // current color right
  // next color right
  var colorIndices = [0,1,2,3];
  
  //transition speed
  var gradientSpeed = 0.002;
  
  function updateGradient()
  {
    
    if ( $===undefined ) return;
    
  var c0_0 = colors[colorIndices[0]];
  var c0_1 = colors[colorIndices[1]];
  var c1_0 = colors[colorIndices[2]];
  var c1_1 = colors[colorIndices[3]];
  
  var istep = 1 - step;
  var r1 = Math.round(istep * c0_0[0] + step * c0_1[0]);
  var g1 = Math.round(istep * c0_0[1] + step * c0_1[1]);
  var b1 = Math.round(istep * c0_0[2] + step * c0_1[2]);
  var color1 = "rgb("+r1+","+g1+","+b1+")";
  
  var r2 = Math.round(istep * c1_0[0] + step * c1_1[0]);
  var g2 = Math.round(istep * c1_0[1] + step * c1_1[1]);
  var b2 = Math.round(istep * c1_0[2] + step * c1_1[2]);
  var color2 = "rgb("+r2+","+g2+","+b2+")";
  
   $('#gradient').css({
     background: "-webkit-gradient(linear, left top, right top, from("+color1+"), to("+color2+"))"}).css({
      background: "-moz-linear-gradient(left, "+color1+" 0%, "+color2+" 100%)"});
    
    step += gradientSpeed;
    if ( step >= 1 )
    {
      step %= 1;
      colorIndices[0] = colorIndices[1];
      colorIndices[2] = colorIndices[3];
      
      //pick two new target color indices
      //do not pick the same as the current one
      colorIndices[1] = ( colorIndices[1] + Math.floor( 1 + Math.random() * (colors.length - 1))) % colors.length;
      colorIndices[3] = ( colorIndices[3] + Math.floor( 1 + Math.random() * (colors.length - 1))) % colors.length;
      
    }
  }
  
  setInterval(updateGradient,10);
    </script>
</body>
</html>



