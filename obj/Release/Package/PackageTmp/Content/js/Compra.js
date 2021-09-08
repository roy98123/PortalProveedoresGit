
$(document).ready(function () {

    autocomplete(document.getElementById("proveedor"), getProveedores());


    if (getCookie("articulos") === "") {

        createCookie("articulos", "", 2);

    } else {

        productos = JSON.parse(getCookie("articulos"));

    }
    document.getElementById('tablaArticulos').innerHTML = "";

    date();

    creartabla();

});

function getProveedores() {

    var res;
    var hola = [];


    $.ajax({
        type: "POST",                                              // tipo de request que estamos generando
        url: 'Compra.aspx/getProveedores',                         // URL al que vamos a hacer el pedido
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: false,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso

            res = JSON.parse(resultado.d);
            res['RetVal'].forEach(element => {
                hola.push(element['Attributes'][1]['AttrValue'])
            });

           
            
        }
    });

    return hola;
}

var productos = [];

$('#btnGuardarArticulo').on('click', function (e) {

    e.preventDefault();

    if (validateArticulo()) {

        productos.push(getAtributos());


        createCookie("articulos", JSON.stringify(productos), 2);

        creartabla();

    } else {
        alert('Todos los campos son necesarios');
    }

});

$('#btnFinalizar').on('click', function (e) {

    e.preventDefault();

    if (validateForm()) {

        if (productos !== []) {

            if (getProveedores().includes(jQuery('#proveedor').val())) {

                sendCompra(getOCID());

            } else {

                alerta("error", "Elige un proveedor válido.");

            }

        } else {
            alerta("error",'Agrega algún artículo a tu compra.');
        }

    } else {
        alerta("error",'Todos los campos son necesarios.');
    }

});

function alerta(tipo, mensaje) {
    Swal.fire({
        icon: tipo,
        text: mensaje,
        showConfirmButton: false,
        timer: 2000
    })
}

function sendCompra(OCID) {

    PutOCID();

   
    var data = {
        encabezado: {
            OCID        : OCID,
            proveedor   : $('#proveedor').val(),
            moneda      : $('#moneda').val(),
            fecha       : $('#fecha').val(),
            estatus     : 1,
            items       : JSON.parse(getCookie('articulos'))
        }
    };

    $.ajax({
        type: "POST",                                              // tipo de request que estamos generando
        data: JSON.stringify(data), 
        url: 'Compra.aspx/registrarCompra',                         // URL al que vamos a hacer el pedido
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: false,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso

            let response_ajax = "";
            let respuesta = "";
            response_ajax = JSON.parse(resultado.d);
            response_ajax.forEach(element => {
                respuesta = element['Success'];
            });

            if (respuesta) {

                alerta('success', '¡Compra realizada!')

                createCookie("articulos", "", 2);

                creartabla();

                clear();

                clear2();

            }
        }
    });


}

function getAtributos() {

    data = {

        'OCId': "",
        'NumLinea': 1,
        'ArticuloId': jQuery('#articulo').val(),
        'Cantidad': jQuery('#cantidad').val(),
        'PrecioUnitario': jQuery('#precio').val(),
        'Importe': jQuery('#importe').val(),
        'Descuento': jQuery('#descuento').val(),
        'ImpuestosTransladados': jQuery('#trasladados').val(),
        'ImpuestosRetenidos': jQuery('#retenidos').val(),
        'Estatus': jQuery('#estatus').val(),
 
    }

    clear();

    return data;

}

function PutOCID(id) {

    var array = JSON.parse(getCookie("articulos"));

    for (let index = 0; index < array.length; index++) {

        const element = array[index];

        element.OCID = id;
        element.NumLinea = index;
    }

    createCookie("articulos", JSON.stringify(array), 2);

}

function editar(id) {

    var articulo = JSON.parse(getCookie("articulos"))[id - 1].articulo;

    var array = JSON.parse(getCookie("articulos"));

    var cantidad = jQuery('#' + id + '').val();

    for (let index = 0; index < array.length; index++) {

        const element = array[index];

        if (element.articulo == articulo) {

            element.Cantidad = cantidad;
        }

    }

    createCookie("articulos", JSON.stringify(array), 2);

    creartabla()

}

function date() {

    var today = new Date();

    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

    document.getElementById('fecha').value = date;

}


function getOCID() {

    var today = new Date();

    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

    return "OC" + date + time;
}


function creartabla() {

    var tabla = "";

    var sum = 0;

    var sumaTotal = 0;

    for (let index = 0; index < JSON.parse(getCookie("articulos")).length; index++) {

        const element = JSON.parse(getCookie("articulos"))[index];

        sum = (element.Cantidad * element.PrecioUnitario);

        sub = (100 - element.Descuento) * .01;

        sub = sub * sum;

        sumaTotal += sub;

        tabla += `
      								    <tr>
                                            <td>` + (index + 1) + `</td>
                                            <td>` + element.ArticuloId + `</td>
                                            <td>$` + element.PrecioUnitario + `</td>
                                            <td>
                                                <input style="background-color: white;" onfocusout="editar(this.id)" contenteditable="true" id="` + (index + 1) + `" value="` + element.Cantidad + `">
                                                                               </td>
                                                <td>` + element.Descuento + `%</td>
                                                <td>$` + (sub).toFixed(3) + `</td>
                                                <td>
                                                    <center><button onclick="eliminarArticulo(` + index + `)" class="btn btn-danger">Eliminar</button></center>
                                                </td>
								    	</tr>
			     `;

    }



    document.getElementById('subtotal').innerHTML = "$" + (sumaTotal).toFixed(3);

    document.getElementById('iva').innerHTML = "$" + (sumaTotal * .16).toFixed(3);

    document.getElementById('total').innerHTML = "$" + (sumaTotal * 1.16).toFixed(3);


    document.getElementById('tablaArticulos').innerHTML = tabla;

}


function createCookie(nombre, valor, days) {
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = nombre + "=" + valor + expires + "; path=/";
}

function eliminarArticulo(item) {


    var art = getCookie("articulos");

    var items = JSON.parse(art);

    items.splice(item, 1);

    createCookie("articulos", JSON.stringify(items), 2);

    creartabla();

}

function clear() {
    document.getElementById("formAtributos").reset();
}

function clear2() {
    document.getElementById("proveedor").value = "";
    document.getElementById("moneda").value = "";  
}

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}



function validateForm() {

    let proveedor = jQuery('#proveedor').val();
    let fecha = jQuery('#fecha').val();
    let moneda = jQuery('#moneda').val();
    let validate = true;

    if (proveedor === "") {
        validate = false;
    }
    if (fecha === "") {
        validate = false;
    }
    if (moneda === "") {
        validate = false;
    }

    return validate;
}

function validateArticulo() {

    let articulo = jQuery('#articulo').val();
    let cantidad = jQuery('#cantidad').val();
    let precio = jQuery('#precio').val();
    let importe = jQuery('#importe').val();
    let descuento = jQuery('#descuento').val();
    let trasladados = jQuery('#trasladados').val();
    let retenidos = jQuery('#retenidos').val();
    let estatus = jQuery('#estatus').val();
    let validate = true;

    if (articulo === "") {
        validate = false;
    }
    if (cantidad === "") {
        validate = false;
    }
    if (precio === "") {
        validate = false;
    }
    if (importe === "") {
        validate = false;
    }
    if (descuento === "") {
        validate = false;
    }
    if (trasladados === "") {
        validate = false;
    }
    if (retenidos === "") {
        validate = false;
    }
    if (estatus === "") {
        validate = false;
    }

    return validate;

}