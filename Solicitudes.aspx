<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Solicitudes.aspx.cs" Inherits="Grow.PortalProveedores.Solicitudes" Title="Solicitudes" %>

<asp:Content ID="CSSContent" ContentPlaceHolderID="PlaceHolderCSS" runat="server">
      <link rel="stylesheet" href="Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css">
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

<section class="content" style="width: 100%">
    <div class="card card-primary">
        <div class="card-header"  >
          <h3 class="card-title">Solicitudes</h3>
        </div>
        <!-- /.card-header -->
        <div class="card-body">
            <table id="tablaSolicitud" class="table table-striped">
                    <thead>
                        <tr>
                            <th scope="col">Folio</th>
                            <th scope="col">Solicitante</th>
                            <th scope="col">Correo</th>
                            <th scope="col">Detalles</th>
                        </tr>
                    </thead>
                    <tbody runat="server" id="tbody_solicitudes">
                        
                    </tbody>    
            </table>
        </div>
          <!-- /.card-body -->
    </div>
      <!-- /.card -->




</section>



<div class="modal fade" id="modal-info-solicitud" tabindex="-1" role="dialog" aria-labelledby="modal-sample-label" >
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                        <h4>Información de la solicitud <span class="glyph glyph-paste" aria-hidden="true"></span> </h4>
            </div>
                <div class="modal-body">

                <div class="row">
                    <div class="col">
                        <h6>ID Solicitud: <b><span id="id_Solicitud" runat="server"></span></b></h6>            
                        <%--<ul ID="empresa" >  
                        </ul>--%>         
                    </div>    
                    <div class="col">
                        <h6>Razón Social: <b><span id="razon" runat="server"></span></b></h6>            
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <h6>RFC: <b><span id="rfc" runat="server"></span></b></h6>            
                            <%--<ul ID="empresa" >  
                            </ul>--%>     
                    </div>
                    <div class="col">
                        <h6>Correo electrónico: <b><span id="correo" runat="server"></span></b></h6>            
                    </div>
                </div>
                <hr />
                <h4>Empresas:</h4>
                <div class="row" id="divListaEmpresas" runat="server">
                    <div class="col-md-12">
                        <h5>Empresa 1</h5>
                        <ul>
                            <li>INE: <a>Prueba</a></li>
                        </ul>
                    </div>
                    <div class="col-md-12">
                        <h5>Empresa 2</h5>
                        <ul>
                            <li>RFC: <a>Prueba</a></li>
                        </ul>
                    </div>
                    <div class="col-md-12">
                        <h5>Empresa 3</h5>
                        <ul>
                            <li>CURP: <a>Prueba</a></li>
                        </ul>
                    </div>
                </div>
                <hr />
                <br />
                <div class="text-center">
                    <%--<button type="button" class="btn btn-danger m-l-sm m-r-sm"
                            data-dismiss="modal">
                        Rechazar Solicitud
                    </button>--%>
                    <%--<button type="button" class="btn btn-primary m-l-sm m-r-sm">
                        Aprobar solicitud
                    </button>--%>
                    <%--<asp:Button class="btn btn-danger m-l-sm m-r-sm" Text="Rechazar solicitud" runat="server" ID="btnRechazar"/>
                    <asp:Button class="btn btn-primary m-l-sm m-r-sm" Text="Aprobar solicitud" runat="server" ID="btnAprobar"/>--%>
                </div>

            </div>
            <div class="modal-footer">
                <button  onclick="cerrarmodal()" type="button" class="btn btn-default" data-dismiss="modal">
                    Cerrar
                </button>
                <button type='button' class='btn btn-danger m-l-sm m-r-sm' onclick='Aprobacion(2)'>Rechazar solicitud</button>
                <button type='button' class='btn btn-primary m-l-sm m-r-sm' onclick='Aprobacion(1)'>Aprobar solicitud</button>
            </div>

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

<%--En este script lleno mi tabla--%>
<script src="Content/js/Solicitudes.js"></script>

<%--en este otro script le aplico la funcion a mi tabla--%>
<script>
    $(document).ready(function () {
        $('#tablaSolicitud').DataTable({
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
            },
            responsive: {
                  details: {
                      type: 'column',
                      target: 'tr'
                  }
              },
              columnDefs: [ {
                  className: 'control',
                  orderable: false,
                  // targets:   0 // oculta la primer linea
              } ],
              order: [ 0, 'desc' ]
        }); // DataTable


    });

    

</script>

</asp:Content>

