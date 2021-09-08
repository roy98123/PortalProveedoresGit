<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RegisterCompany.aspx.cs" Inherits="Grow.PortalProveedores.RegisterCompany" Title="Registrar Empresa" %>


<asp:Content ID="CSSContent" ContentPlaceHolderID="PlaceHolderCSS" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="NameContent" runat="server">
    <a href="#" class="d-block" id="NameContent" runat="server">
        
    </a>
</asp:Content>

<asp:Content ContentPlaceHolderID="RolePlaceHolder" runat="server">
    <div runat="server" id="RolContent">

    </div>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<section class="content" style="width: 100%">
    <div class="card card-primary">
        <div class="card-header">
          <h3 class="card-title">Registre una nueva empresa.</h3>
        </div>
        <!-- /.card-header -->
        <div class="card-body">
            <div class="container">
                <br />
                <div class="card shadow p-3 mb-5 bg-body rounded container-card jumbotron">
            <form id="formAgregarEmpresa">
                <div class="form-group text-center" style="align-self: center">
                    <label for="razonSocial">Razón Social</label>
                    <input type="text" class="form-control" placeholder="Razón Social" id="razon-social" style="width: 100%">
                    <small style="color: red;" id="error_razon_social"></small>
                </div>
    

                <%--Aqui ira el bton para el modal--%>
                <center>
                    <button type = 'button' class='btn btn-primary btn-lg' data-toggle='modal' data-target='#nuevoArchivo'> 
                       Agregar archivo a Solicitar
                    </button>
                </center>

                  
                <br />
                <div class="mt-2">
                    <ul class="list-group" >

                    </ul>
                 </div>
                <h7>Lista de archivos para solicitar al proveedor</h7>
                <hr />
                <div class="table-responsive">
                    <table class="table">
                        <thead>
                          <tr>
                            <th>Archivo</th>
                            <th class="text-right">Eliminar</th>
                          </tr>
                        </thead>
                        <tbody id="lista-archivos">

                        </tbody>
                    </table>
                </div>

                <input type="text" name="inputObject" ID="inputResponse" hidden runat="server" value="" />

                <button type="button" class="form-control btn btn-primary btn-lg btn-block" onclick="insertCompany()">Registrar</button>
            </form>
                        
                </div>
            </div>
            
        </div>
          <!-- /.card-body -->
    </div>
      <!-- /.card -->
</section>


<%--MODAL AGREGAR CAMPO--%>

<div class="modal fade" id="nuevoArchivo" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Nuevo Archivo a Solicitar</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body" style="align-self: center;">
        <input type="text" runat="server" id="idSolicitud" hidden disabled="disabled"/>
            <div class="row text-center">
                <div class="form-group text-center">
                    <label for="nombre-archivo">Nombre de archivo</label>
                    <input type="text" class="form-control" placeholder="Nombre de archivo" id="nombre-archivo" autocomplete="off">
                    <small style="color: red;" id="error_agregar_archivo"></small>
                </div>
            </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button id="btn-agregar-archivo" class="btn btn-primary"  onclick="agregarArchivo()" data-dismiss="modal"> Agregar</button>

      </div>
    </div>
  </div>
</div>


<div class="modal" id="modal-agregar-archivo"
        tabindex="-1" role="dialog"
        aria-labelledby="modal-sample-label" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-center">
                <h4 class="modal-title" >
                    <b>Seleccione una opción</b>
                </h4>
            </div>

            
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="ContentJS" ContentPlaceHolderID="ContentJS" runat="server">
    
    <script src="Content/js/RegisterCompany.js"></script>

</asp:Content>