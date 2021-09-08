<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="NuestrasEmpresas.aspx.cs" Inherits="Grow.PortalProveedores.NuestrasEmpresas" Title="Proveedores" %>
<asp:Content ContentPlaceHolderID="PlaceHolderCSS" runat="server">
        
        <link rel="stylesheet" href="Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

   
</asp:Content>

<asp:Content ContentPlaceHolderID="NameContent" runat="server">
    <a href="#" class="d-block" id="NameContent" runat="server">
        
    </a>
</asp:Content>

<asp:Content ContentPlaceHolderID="RolePlaceHolder" runat="server">
    <div runat="server" id="RolContent">

    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="MenuPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


        <div style="width: 100%;" class="row">
        <div class="col-12">
            <div class="card card-primary">
        <div class="card-header"" >
          <h3 style=" color:white;" class="card-title">Proveedores registrados</h3>
        </div>
              <!-- /.card-header -->
              <div class="card-body">
                <table id="tablaEmpresa" class="table table-hover">
                  <thead>
                  <tr>
                    <th>Folio</th>
                    <th>Razón Social</th>
                    <th>Detalles</th>
                  </tr>
                  </thead>
                  <tbody runat="server" id="tbody_empresas">
                      </tbody>
                  <tfoot>
                  <tr>
                    <th>Folio</th>
                    <th>Razón Social</th>
                    <th>Detalles</th>
                  </tr>
                  </tfoot>
                </table>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
        </div>
    </div>

   

    <div class="modal fade" id="modal-info-solicitud" tabindex="-1" role="dialog" aria-labelledby="modal-sample-label" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header">
                <div class="position-relative text-center">
                    <div>
                        <h4><span class="glyph glyph-paste" aria-hidden="true"></span> Información del proveedor </h4>
                    </div>
                </div>
            </div>

            <div class="modal-body">
                <form autocomplete="off">
                    <div class="card-body">
                      <div class="form-group">
                        <label for="nombre">Nombre</label>
                        <input type="text" class="form-control" id="nombre" placeholder="Nombre" style="max-width: inherit">
                      </div>
                      <div class="form-group">
                        <label for="correo">Correo electrónico</label>
                        <input type="email" class="form-control" id="correo" placeholder="Correo electrónico" style="max-width: inherit">
                      </div>

                      <div class="form-group">
                        <label for="proposito">Propósito</label> <br />
                        <select style="width:100%; max-width: inherit" class="form-control" id="proposito" style="max-width: inherit">
                          <option selected disabled>Selecciona una opción</option>
                          <option>Ventas</option>
                          <option>Facturación</option>
                        </select>
                      </div>
                      
                     
                    </div>
                    <!-- /.card-body -->
    
                    <div class="card-footer">
                        <label for="correo">Documentos Requeridos:</label>
                        <ul id="documentos" class="list-items">
                    </div>
                  
             </div>

            <div class="modal-footer">
                <button  onclick="cerrarmodal()" type="button" class="btn btn-default" data-dismiss="modal">
                    Cerrar
                </button>

                <button type="button" class="btn btn-primary" data-dismiss="modal">
                    Guardar Cambios
                </button>
            </div>

            </form>

        </div>
    </div>
</div>
  


</asp:Content>


<asp:Content ID="ContentJS" ContentPlaceHolderID="ContentJS" runat="server">

    <!-- DataTables  & Plugins -->
    <script src="Content/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
    <script src="Content/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.html5.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.print.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.colVis.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

<script src="Content/js/NuestrasEmpresas.js"></script>

<script>
    $(document).ready(function () {

        $('.select-razon').select2();


        $('#tablaEmpresa').DataTable({
            //"order": [[0, 'des']],
            language: {
                "decimal": "",
                "emptyTable": "No hay información",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                "infoEmpty": "Mostrando 0 to 0 of 0 registros",
                "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "No se encontraron registros",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                }
            }
        }); // DataTable


    });



</script>

</asp:Content>