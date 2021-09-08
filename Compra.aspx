<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compra.aspx.cs" Inherits="Grow.PortalProveedores.Compra" Title="Compra" %>

<asp:Content ID="CSSContent" ContentPlaceHolderID="PlaceHolderCSS" runat="server">
      <link rel="stylesheet" href="Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css">
      <link rel="stylesheet" href="Content/plugins/autocompletar.css">
</asp:Content>

<asp:Content ContentPlaceHolderID="NameContent" runat="server">
    <a href="#" class="d-block" id="NameContent" runat="server">
    </a>
</asp:Content>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  
<section class="content" style="width: 100%">

    <div class="card card-primary">
        <div class="card-header"  >
          <h3 class="card-title">Nueva Compra</h3>
        </div>
        <div class="card-body">

            <br />

          <form autocomplete="off">
              <div class="row"> 
            <div class="col-sm">
              <label for="proveedor">Proveedor</label>
              <input type="text" style="max-width: inherit" class="form-control" id="proveedor"  />
            </div>
            <div class="col-sm">
              <label for="fecha">Fecha de pedido</label>
              <input type="text" style="max-width: inherit" class="form-control" id="fecha" readonly />
            </div>
          </div>

          <div class="row"> 
            <div class="col-sm">
              <label for="moneda">Moneda</label> 
              <input type="text" style="max-width: inherit" class="form-control" id="moneda" placeholder="">
            </div>
              <div class="col-sm">
            </div>
          </div>
          </form>

          <br>

          <div class="row">
           <div class="col">
            <button class="float-right btn btn-primary" id="btnModalProductos"  type="button" data-toggle="modal" data-target="#modal-default">
              Agregar Artículo
            </button>
           </div>
          </div>

          <br>


             <table id="" style="width: 100%;" class="table table-estriped table-bordered">
                  <thead>
                    <tr>
                      <th scope="col">#</th>
                      <th scope="col">Artículo</th>
                      <th scope="col">Costo</th>
                      <th scope="col">Cantidad</th>
                        <th scope="col">Descuento</th>
                      <th scope="col">Subtotal</th>
                      <th scope="col"></th>
                    </tr>
                  </thead>
                  <tbody id="tablaArticulos">
                    
                  </tbody>
                </table>

                <br />

                <div class="row">
                    <div class="col"> 
                        <button id="btnFinalizar" class="float-right btn btn-primary">
                          Finalizar Compra
                        </button>
                    </div>
                    <div class="float-right col" style="width: 35%;">
                        <table class="table table-bordered float-right">
                        <tbody>
                          <tr>
                            <td><b>Subtotal</b></td>
                            <td id="subtotal" style="align-content: rigth;">$00.00</td>
                          </tr>
                          <tr>
                            <td><b>IVA</b></td>
                            <td id="iva" style="align-content: rigth;">$00.00</td>
                          </tr>
                          <tr>
                            <td><b>Total</b></td>
                            <td id="total" style="align-content: rigth;" >$00.00</td>
                          </tr>
                        </tbody>
                        </table>
                    </div>
                </div>
    </div>




</section>
 

<!-- modal -->
  <div class="modal fade" id="modal-default">
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h4 class="modal-title">Agregar Artículo</h4>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
            <br />
          
            <form  id="formAtributos" autocomplete="off">
                <div class="container">

            <div>
                <label for="articulo">Artículo</label>
                <input type="text" class="form-control" style="max-width: inherit" id="articulo" placeholder="">
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="cantidad">Cantidad</label>
                <input type="number" class="form-control" style="max-width: inherit" id="cantidad" placeholder="">
              </div>
              <div class="col">
                <label for="precio">Precio Unitario</label>
                <input type="text" class="form-control" style="max-width: inherit" id="precio" placeholder="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="importe">Importe</label>
                <input type="text" class="form-control" style="max-width: inherit" id="importe" placeholder="">
              </div>
              <div class="col">
                <label for="descuento">Descuento</label>
                <input type="text" class="form-control" style="max-width: inherit" id="descuento" placeholder="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="trasladados">Impuestos Trasladados</label>
                <input type="text" class="form-control" style="max-width: inherit" id="trasladados" placeholder="">
              </div>
              <div class="col">
                <label for="retenidos">Impuestos Retenidos</label>
                <input type="text" class="form-control" style="max-width: inherit" id="retenidos" placeholder="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col-sm">
                <label for="estatus">Estatus</label>
                <input type="text" class="form-control" style="max-width: inherit" id="estatus" placeholder="">
              </div>
                <div class="col-sm">
              </div>
            </div>
          </div>
            </form>

        </div>
        <div class="modal-footer justify-content-between float-right">
          <button type="button" id="btnGuardarArticulo"  class="btn btn-primary float-right">Agregar</button>
        </div>
      </div>
    </div>
  </div>
  <!-- /.modal -->

</asp:Content>

<asp:Content ID="ContentJS" ContentPlaceHolderID="ContentJS" runat="server">


<script src="Content/js/Compra.js"></script>

<script src="Content/js/Autocompletar.js"></script>

</asp:Content>

