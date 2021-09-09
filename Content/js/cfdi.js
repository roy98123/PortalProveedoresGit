
if (document.getElementById("MainContent_errores_xml").value.length ) {
    let texto = document.getElementById("MainContent_errores_xml").value;
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: texto,
    });
}

if (document.getElementById("MainContent_success_xml_titulo").value.length) {
    let titulo = document.getElementById("MainContent_success_xml_titulo").value;
    let texto = document.getElementById("MainContent_success_xml_texto").value;

    Swal.fire({
        icon: 'success',
        title: titulo,
        text: texto,
    });
}

// Asignamos la función al botón submit del  formuluario
document.getElementById('MainContent_XML_file').onchange = loadXML;
function loadXML() {
    // Pasamos el archivo xml a Base64
    let input = document.getElementById('MainContent_XML_file');
    let file = input.files[0];
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        let base64 = reader.result;
        document.getElementById('MainContent_XML_file_b64').value = base64; // Lo asignamos a un input ya que no podemos retornar un valor de una Función anónima
    }
}

// Asignamos la función al botón submit del  formuluario
document.getElementById("btnRegistrarCFDI").onclick = insertCFDI;
function insertCFDI() {

    // Validamos que los campos requeridos del formulario no esten vacios
    if (!validarFormulario()) {
        Swal.fire({
            icon: 'error',
            title: 'Campos obligatorios',
            text: 'Faltan campos por llenar',
        });
        return;
    } 

    let fecha_certificacion = document.getElementById('MainContent_fecha_certificacion').value;
    fecha_certificacion == "" ? fecha_certificacion = null : "";
    // Le damos formato a los datos del fomurlario para enviarlos al Back
    setTimeout(function () { }, 3000);
    let data = {
        factura: {
            xml:                    document.getElementById('MainContent_XML_file_b64').value, // Mandamos el valor del input del archivo en B64
            tipoComprobante:        document.getElementById('MainContent_tipo_comprobante').value,
            folio:                  document.getElementById('MainContent_folio').value,
            serie:                  document.getElementById('MainContent_serie').value,
            rfcEmisor:              document.getElementById('MainContent_rfc_emisor').value,
            rfcReceptor:            document.getElementById('MainContent_rfc_receptor').value,
            nombreEmisor:           document.getElementById('MainContent_nombre_emisor').value,
            fechaEmision:           document.getElementById('MainContent_fecha_emision').value,
            fechaCertificacion:     fecha_certificacion,
            formaPago:              document.getElementById('MainContent_forma_pago').value,
            metodoPago:             document.getElementById('MainContent_metodo_pago').value,
            subtotal:               document.getElementById('MainContent_subtotal').value,
            total:                  document.getElementById('MainContent_total').value,
            impuestosRetenidos:     document.getElementById('MainContent_impuestos_retenidos').value,
            impuetoTrasladados:     document.getElementById('MainContent_impuestos_trasladados').value,
            uuid:                   document.getElementById('MainContent_uuid').value,
            estatusComprobante:     document.getElementById('MainContent_estatus_comprobante').value,
            estatusSAT:             document.getElementById('MainContent_estatus_sat').value,
            ultimaActualizacionSAT: document.getElementById('MainContent_actualizacion_sat').value
        }
    }

    $.ajax({
        type: "POST",
        url: 'CFDI.aspx/insertCFDI',                               // URL al que vamos a hacer la petición
        data: JSON.stringify(data),                                // data es un arreglo JSON que contiene los datos que enviamos
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: true,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso
            // Mostramos un sweetAlert si la peticón Ajax salio correctamente
            Swal.fire({
                icon: 'success',
                text: 'CFDI Registrado',
                showConfirmButton: false,
                timer: 3000
            });
            limpiarFormulario();
        }
    });
}

function validarFormulario() {
    let ids = [
        "XML_file",
        "folio",
        "tipo_comprobante",
        "serie",
        "nombre_emisor",
        "rfc_emisor",
        "rfc_receptor",
        "fecha_emision",
        "forma_pago",
        "metodo_pago",
        "subtotal",
        "total",
        "impuestos_retenidos",
        "impuestos_trasladados",
        "uuid",
        "estatus_comprobante",
        "estatus_sat",
        "actualizacion_sat"
    ];
    let resp = true;
    ids.forEach(id => {
        if (document.getElementById("MainContent_" + id).value == null || document.getElementById("MainContent_" + id).value == "") {
            document.getElementById('error_' + id).innerText = "Campo obligatorio";
            resp = false;
        } else {
            document.getElementById('error_' + id).innerText = "";
        }
    });

    return resp ;
}

 function limpiarFormulario(){
     let ids = [
         "XML_file",
         "XML_file_b64",
         "folio",
         "tipo_comprobante",
         "serie",
         "nombre_emisor",
         "rfc_emisor",
         "rfc_receptor",
         "fecha_emision",
         "forma_pago",
         "metodo_pago",
         "subtotal",
         "total",
         "impuestos_retenidos",
         "impuestos_trasladados",
         "uuid",
         "estatus_comprobante",
         "estatus_sat",
         "actualizacion_sat"
     ];
     let resp = true;
     ids.forEach(id => {
         document.getElementById('error_' + id).innerText = "";
         document.getElementById("MainContent_" + id).value = "";
     });
}