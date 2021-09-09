<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CFDI.aspx.cs" Inherits="Grow.PortalProveedores.CFDI" Title="CFDI" %>


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


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<section class="content" style="width: 100%">
    <div class="card card-primary">
        <div class="card-header">
            <div class="row">
                <div class="col"><h3 class="card-title">Registrar CFDI</h3></div>
                <div class="col text-right"><small class="text-white">Los campos marcados con un asterisco son obligatorios. </small></div>
            </div>
        </div>
        <!-- /.card-header -->
        <div class="card-body">

                <div class="form-group">
                    <div class="row">
                        <div class="col">
                            <label for="XML_file">XML </label><small class="text-danger">*</small>
                            <div class="row">
                                <div class="col-8">
                                    <asp:FileUpload class="form-control" ID="XML_file" style="min-width:-webkit-fill-available;" runat="server" accept=".xml" />
                                    <small class="form-text text-danger" id="error_XML_file"></small>
                                    <small class="form-text text-danger" id="error_XML_file_b64"></small>

                                </div>
                                    <div class="col">
                                    <asp:Button type="button" CssClass="btn btn-primary" Text="Analizar XML" runat="server" OnClick="btnRegistarCFDI_Click" ID="btnAnalizarXML" />
                                </div>
                            </div>
                        </div>
                        <div class="col text-center">
                            <small class="form-text text-muted">Si desea analizar y cargar la información del archivo XML de click aquí.</small>
                            <small class="form-text text-muted">Para esto necesitarás cargar dos veces tu archivo, una para ser procesado y otra para ser guardado.</small>
                            <%--<button type="button" class="btn btn-primary mt-2" runat="server" onclick="btnRegistarCFDI_Click">Analizar XML</button>--%>
                        </div>
                    </div>
                </div>
                <input type="text" id="XML_file_b64" value="" runat="server" hidden/>

                <div class="form-group">
                    <label for="tipo_comprobante">Tipo de Comprobante</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="tipo_comprobante" maxlength="5" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_tipo_comprobante"></small>
                </div>
                

                <div class="form-group">
                    <label for="folio">Folio</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="folio" maxlength="40" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_folio"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Serie</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="serie" maxlength="40" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_serie"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Nombre Emisor</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="nombre_emisor" maxlength="200" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_nombre_emisor"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">RFC Emisor</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="rfc_emisor" maxlength="18" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_rfc_emisor"></small>
                </div>
                <div class="form-group">    
                    <label for="tipo_comprobante">RFC Receptor</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="rfc_receptor" maxlength="18" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_rfc_receptor"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Fecha Emisión</label><small class="text-danger">*</small>
                    <input runat="server" type="dateTime-local" class="form-control" id="fecha_emision" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_fecha_emision"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Fecha de Certificación</label>
                    <input runat="server" type="dateTime-local" class="form-control" id="fecha_certificacion" style="min-width: -webkit-fill-available;">
                    <small class="form-text text-danger" id="error_fecha_certificacion"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Forma de Pago</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="forma_pago" maxlength="10" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_forma_pago"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Método de Pago</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="metodo_pago" maxlength="10" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_metodo_pago"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Subtotal</label><small class="text-danger">*</small>
                    <input runat="server" type="number" step="0.00001" min="0" max="999999999999999999.99999999" class="form-control" id="subtotal" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_subtotal"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Total</label><small class="text-danger">*</small>
                    <input runat="server" type="number" class="form-control" min="0" max="999999999999999999.99999999" id="total" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_total"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Impuestos Retenidos</label><small class="text-danger">*</small>
                    <input runat="server" type="number" min="0" max="999999999999999999.99999" class="form-control" id="impuestos_retenidos" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_impuestos_retenidos"></small>
                </div>
                <div class="form-group">
                    <label for="tipo_comprobante">Impuestos Trasladados</label><small class="text-danger">*</small>
                    <input runat="server" type="number" min="0" max="999999999999999999.99999" class="form-control" id="impuestos_trasladados" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_impuestos_trasladados"></small>
                </div>

                <div class="form-group">
                    <label for="tipo_comprobante">UUID</label><small class="text-danger">*</small>
                    <input runat="server" type="text" class="form-control" id="uuid" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_uuid"></small>
                </div>

                <div class="form-group">
                    <label for="tipo_comprobante">Estatus Comprobante</label><small class="text-danger">*</small>
                    <select runat="server" type="text" class="form-control" id="estatus_comprobante" style="min-width: -webkit-fill-available;">
                        <option value="">-- Selecciona una opción -- </option>
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                    </select>
                    <small class="form-text text-danger" id="error_estatus_comprobante"></small>
                </div>

                <div class="form-group">
                    <label for="tipo_comprobante">Estatus SAT</label><small class="text-danger">*</small>
                    <select runat="server" type="text" class="form-control" id="estatus_sat" style="min-width: -webkit-fill-available;">
                        <option value="">-- Selecciona una opción -- </option>
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                    </select>
                    <small class="form-text text-danger" id="error_estatus_sat"></small>
                </div>

                <div class="form-group">
                    <label for="tipo_comprobante">Ultima actualización del SAT </label><small class="text-danger">*</small>
                    <input runat="server" type="dateTime-local" class="form-control" id="actualizacion_sat" style="min-width: -webkit-fill-available;" >
                    <small class="form-text text-danger" id="error_actualizacion_sat"></small>
                </div>

                <%--<asp:Button type="submit" CssClass="btn btn-primary" Text="Registrar CFDI" runat="server" id="btnRegistarCFDI" OnClick="enviarXML"/>--%>
                <button type="button" class="btn btn-primary" id="btnRegistrarCFDI">
                    Registrar CFDI
                </button>

                <input type="text" id="errores_xml" runat="server" value="" hidden/>
                <input type="text" id="success_xml_titulo" runat="server" value="" hidden/>
                <input type="text" id="success_xml_texto" runat="server" value="" hidden/>

        </div>
          <!-- /.card-body -->
    </div>
      <!-- /.card -->




</section>

    


</asp:Content>

<asp:Content ID="ContentJS" ContentPlaceHolderID="ContentJS" runat="server">

    <script src="Content/js/cfdi.js"></script>
</asp:Content>

