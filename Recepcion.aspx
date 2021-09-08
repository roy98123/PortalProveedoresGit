<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recepcion.aspx.cs" Inherits="Grow.PortalProveedores.Recepcion" Title="Recepción de Compra" %>

<asp:Content ID="CSSContent" ContentPlaceHolderID="PlaceHolderCSS" runat="server">
    <link rel="stylesheet" href="Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
    <link rel="stylesheet" href="Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
    <link rel="stylesheet" href="Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css">
    <style>
        .autocomplete {
            /*the container must be positioned relative:*/
            position: relative;
            display: inline-block;
        }

        input {
            border: 1px solid transparent;
            background-color: #f1f1f1;
            padding: 10px;
            font-size: 16px;
        }

            input[type=text] {
                background-color: #f1f1f1;
                width: 100%;
            }

            input[type=submit] {
                background-color: DodgerBlue;
                color: #fff;
            }

        .progress-bar {
            background-color: #2596be !important;
        }

        .autocomplete-items {
            position: absolute;
            border: 1px solid #d4d4d4;
            border-bottom: none;
            border-top: none;
            z-index: 99;
            /*position the autocomplete items to be the same width as the container:*/
            top: 100%;
            left: 0;
            right: 0;
        }

            .autocomplete-items div {
                padding: 10px;
                cursor: pointer;
                background-color: #fff;
                border-bottom: 1px solid #d4d4d4;
            }

                .autocomplete-items div:hover {
                    /*when hovering an item:*/
                    background-color: #e9e9e9;
                }

        .autocomplete-active {
            /*when navigating through the items using the arrow keys:*/
            background-color: DodgerBlue !important;
            color: #ffffff;
        }
    </style>

</asp:Content>

<asp:Content ContentPlaceHolderID="NameContent" runat="server">
    <a href="#" class="d-block" id="NameContent" runat="server"></a>
</asp:Content>

<asp:Content ContentPlaceHolderID="RolePlaceHolder" runat="server">
    <div runat="server" id="RolContent">
    </div>
</asp:Content>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <section class="content" style="width: 100%">

        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Recepción de Compra</h3>
            </div>
            <div class="card-body">

                <br />

                <form autocomplete="off">
                    <div class="row">

                        <div class="col-sm">
                            <div class="row">
                                <div class="col-sm" style="display:flex; align-items:flex-end;">
                                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalCompras">
                                        Seleccionar compra a recibir    
                                    </button>
                                </div>
                                <div class="col-sm">
                                    <label for="fecha"># Compra</label>
                                    <input type="text" style="max-width: inherit" class="form-control" id="OCId" readonly />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm">
                            <label for="fecha">Fecha de pedido</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="fecha" readonly />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm">
                            <label for="proveedor">Proveedor</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="proveedor" placeholder="" disabled>
                        </div>
                        <div class="col-sm">
                            <label for="moneda">Moneda</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="moneda" placeholder="" disabled>
                        </div>
                    </div>
                </form>

                <br>
                <hr />
                <%-- Apartado de Compra --%>
                <div> 
                    <h4>Tabla de Compra</h4>
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

                        </div>
                        <div class="float-right col">
                            <table class="table table-bordered float-right">
                                <tbody>
                                    <tr>
                                        <td><b>Subtotal</b></td>
                                        <td id="subtotal">$00.00</td>
                                    </tr>
                                    <tr>
                                        <td><b>IVA</b></td>
                                        <td id="iva" style="align-content: rigth;">$00.00</td>
                                    </tr>
                                    <tr>
                                        <td><b>Total</b></td>
                                        <td id="total" style="align-content: rigth;">$00.00</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <%-- Apartado de Recibido --%>
                <hr />
                <div> 
                    <div class="row">
                        <div class="col">
                            <h4>Tabla de Recibido</h4>
                        </div>
                        <div class="col">
                            <button class="float-right btn btn-primary" id="btnModalProductos"  type="button" data-toggle="modal" data-target="#modalArticulos" disabled>
                              Agregar Artículo
                            </button>
                        </div>
                    </div>
                    <br />
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
                        <tbody id="tablaArticulosRecibidos">
                        
                        </tbody>
                    </table>
                    <br />
                    <div class="row">
                        <div class="col">
                            <button type="button" id="btnFinalizarRecibido" class="float-right btn btn-primary">
                                Finalizar Compra
                            </button>
                        </div>
                        <div class="float-right col" style="width: 35%;">
                            <table class="table table-bordered float-right">
                                <tbody>
                                    <tr>
                                        <td><b>Subtotal</b></td>
                                        <td id="subtotal_recibido">$00.00</td>
                                    </tr>
                                    <tr>
                                        <td><b>IVA</b></td>
                                        <td id="iva_recibido" style="align-content: rigth;">$00.00</td>
                                    </tr>
                                    <tr>
                                        <td><b>Total</b></td>
                                        <td id="total_recibido" style="align-content: rigth;">$00.00</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </section>


    <!-- modal de Compras-->
    <div class="modal fade" id="modalCompras">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Seleccionar compra</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="container">
                        <table id="tablaCompras" class="table table-striped" style="width: 100%;">
                            <thead>
                                <tr>
                                    <th scope="col"># Compra</th>
                                    <th scope="col">Proveedor</th>
                                    <th scope="col">Moneda</th>
                                    <th scope="col">Fecha</th>
                                    <th scope="col">Estatus</th>
                                    <th scope="col">Opción</th>
                                </tr>
                            </thead>
                            <tbody runat="server" id="tbody_compras">
                                
                            </tbody>    
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->

    <!-- modal articulos por recibir -->
  <div class="modal fade" id="modalArticulos">
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h4 class="modal-title">Agregar Artículo para recibir</h4>
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
                <select class="form-control select2-blue" style="max-width: inherit;" id="selectArticulosRecibir">
                    <option value="">-- Selecciona un artículo a recibir -- </option>
                </select>
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="cantidad">Cantidad</label>
                <input type="number" class="form-control" min="0" style="max-width: inherit" id="cantidad_recibir" placeholder="">
                <small class="form-text text-muted" id="cantidad_maxima"></small>
                <input type="text" name="cantidad_maxima" id="input_cantidad_maxima" value="" hidden disabled/>
              </div>
              <div class="col">
                <label for="precio">Precio Unitario</label>
                <input type="text" class="form-control disabled" style="max-width: inherit" id="precio_recibir" disabled="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="importe">Importe</label>
                <input type="text" class="form-control disabled" style="max-width: inherit" id="importe_recibir" disabled="">
              </div>
              <div class="col">
                <label for="descuento">Descuento</label>
                <input type="text" class="form-control disabled" style="max-width: inherit" id="descuento_recibir" disabled="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col">
                <label for="trasladados">Impuestos Trasladados</label>
                <input type="text" class="form-control disabled" style="max-width: inherit" id="trasladados_recibir" disabled="">
              </div>
              <div class="col">
                <label for="retenidos">Impuestos Retenidos</label>
                <input type="text" class="form-control disabled" style="max-width: inherit" id="retenidos_recibir" disabled="">
              </div>
            </div>

            <br>

            <div class="row">
              <div class="col-sm">
                <label for="estatus">Estatus</label>
                <input type="text" class="form-control" style="max-width: inherit" id="estatus_recibir">
              </div>
                <div class="col-sm">
              </div>
            </div>
          </div>
         </form>

        </div>
        <div class="modal-footer justify-content-between float-right">
          <button type="button" id="btnGuardarArticuloRecibir"  class="btn btn-primary float-right">Agregar</button>
          <button type="button" class="btn btn-secondary" data-dismiss="modal" hidden id="dissmisModalArticulos">Cancelar</button>
        </div>
      </div>
    </div>
  </div>
  <!-- /.modal -->

</asp:Content>

<asp:Content ID="ContentJS" ContentPlaceHolderID="ContentJS" runat="server">
    <script src="Content/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
    <script src="Content/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.html5.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.print.min.js"></script>
    <script src="Content/plugins/datatables-buttons/js/buttons.colVis.min.js"></script>
    <script src="Content/js/Recepcion.js"></script>

</asp:Content>

