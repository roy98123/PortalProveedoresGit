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
                            <button type="button" class="btn btn-primary mt-3" data-toggle="modal" data-target="#modalCompras">
                                Seleccionar compra a recibir    
                            </button>
                        </div>
                        <div class="col-sm">
                            <label for="fecha">Fecha de pedido</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="fecha" readonly />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm">
                            <label for="proveedor">Proveedor</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="proveedor" placeholder="">
                        </div>
                        <div class="col-sm">
                            <label for="moneda">Moneda</label>
                            <input type="text" style="max-width: inherit" class="form-control" id="moneda" placeholder="">
                        </div>
                    </div>
                </form>

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

