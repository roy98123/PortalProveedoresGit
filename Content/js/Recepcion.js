$(document).ready(function () {
    $('#tablaCompras').DataTable({
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
        columnDefs: [{
            className: 'control',
            orderable: false,
            // targets:   0 // oculta la primer linea
        }],
        order: [0, 'desc']
    }); // DataTable


});


let lineasEnviar = [];

document.getElementById('btnFinalizarRecibido').onclick = enviarRecepcion;
function enviarRecepcion() {
    console.log(JSON.parse(getCookie('OCEncabezado')));
    let encabezado = JSON.parse(getCookie('OCEncabezado'));
    let today = new Date();
    let date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    console.log(date);
    //return;
    let data = {
        encabezado: {
            OCRecepcionId:  encabezado[0],
            ProveedorId:    encabezado[1],
            MonedaId:       encabezado[2],
            Fecha:          date,
            Estatus:        1               //En proceso
        },
        //documentos: {
        //    documentos: archivos
        //}
    }

    $.ajax({
        type: "POST",
        url: 'Recepcion.aspx/insertRecepcion',                      // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (response) {                              // función que va a ejecutar si el pedido fue exitoso
            console.log(JSON.parse(response.d));
            Swal.fire({
                icon: 'success',
                text: '¡Empresa registrada!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    });
}

document.getElementById('btnGuardarArticuloRecibir').onclick = agregarArticulosRecibir;
let idLineaRecibir = 0;
let total_recibido = 0.00000;
function agregarArticulosRecibir() {
    console.log("registrando archivo");
    let articulo = document.getElementById('selectArticulosRecibir').value;
    let cantidad = document.getElementById('cantidad_recibir').value;
    let precio = document.getElementById('precio_recibir').value;
    let importe = document.getElementById('importe_recibir').value; 
    let descuento = document.getElementById('descuento_recibir').value;
    let traslados = document.getElementById('trasladados_recibir').value;
    let retenidos = document.getElementById('retenidos_recibir').value;
    let estatus = document.getElementById('estatus_recibir').value;
    let subtotal = 0.0000;
    //            100   - descuento  / 100  * (cantidad * precio)
    try {
        subtotal = (100 - descuento) / 100 * (cantidad * precio);
    } catch (exception) {
        subtotal = 0.0000;
    }
    document.getElementById('tablaArticulosRecibidos').innerHTML +=`<tr>
                            <td>${idLineaRecibir    /* Número de Linea  */}</td>
                            <td>${articulo          /* Articúlo         */}</td>
                            <td>${precio            /* Precio unitario  */}</td>
                            <td>${cantidad          /* Cantidad         */}</td>
                            <td>${descuento         /* Descuento        */}</td>
                            <td>${subtotal          /* Subtotal         */}</td>
                            <td></td>
                            </tr>`;
    idLineaRecibir++;

    total_recibido += subtotal;
    document.getElementById('subtotal_recibido').innerText = `$${total_recibido}`;
    let iva = total_recibido * 0.16;
    document.getElementById('iva_recibido').innerText = "$" + iva;
    document.getElementById('total_recibido').innerText = "$" + (total_recibido + iva);
    let selectArituclos = document.getElementById("selectArticulosRecibir");
    for (var i = 0; i < selectArituclos.length; i++) {
        if (selectArituclos.options[i].value == articulo)
            selectArituclos.remove(i);
    }

    document.getElementById('dissmisModalArticulos').click();
    //$("#modalArticulos").modal("hide");
}


function resetTablaArticulosRecibir() {
    document.getElementById('tablaArticulosRecibidos').innerHTML = ""
    document.getElementById('subtotal_recibido').innerText = `$0.00`;
    document.getElementById('iva_recibido').innerText = "$0.00"
    document.getElementById('total_recibido').innerText = "$0.00";
    idLineaRecibir = 0;
    total_recibido = 0.00000;
}

document.getElementById('cantidad_recibir').onfocusout = calcularImporte;
function calcularImporte() {
    let cantidad = this.value;
    let precio = document.getElementById('precio_recibir').value;
    let cantidad_maxima = document.getElementById('input_cantidad_maxima').value;
    let importe = 0;

    try {
        if (cantidad <= cantidad_maxima) {
            try {
                importe = cantidad * precio;
                document.getElementById('importe_recibir').value = importe;
                Swal.fire({
                    icon: "success",
                    text: `Importe actualizado: ${importe}`,
                    showConfirmButton: false,
                    timer: 2000
                });
            } catch (ex) {
                document.getElementById('importe_recibir').value = importe;
                Swal.fire({
                    icon: "error",
                    text: "No es posible calcular el importe",
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        } else {
            document.getElementById('cantidad_recibir').value = cantidad_maxima;
            Swal.fire({
                icon: "error",
                text: "La cantidad pot recibir supera la cantidad de compra",
                showConfirmButton: false,
                timer: 2000
            });
        }
    } catch (ex) {
        document.getElementById('cantidad_recibir').value = cantidad_maxima;
        Swal.fire({
            icon: "error",
            text: "Ingrese un valor valido. No fue posible calcular el nuevo importe",
            showConfirmButton: false,
            timer: 2000
        });
    }


}

document.getElementById('selectArticulosRecibir').onchange = cargarArticulo;
function cargarArticulo() {
    let articulo = this.value
    let cookieArticulos = JSON.parse(getCookie("lineasRecepcion"));

    document.getElementById('cantidad_recibir').value       = cookieArticulos[articulo][3]["AttrValue"];
    document.getElementById('input_cantidad_maxima').value  = cookieArticulos[articulo][3]["AttrValue"];
    document.getElementById('cantidad_maxima').innerText    = `Cantidad máxima: ${cookieArticulos[articulo][3]["AttrValue"]}`;
    document.getElementById('precio_recibir').value         = cookieArticulos[articulo][4]["AttrValue"];
    document.getElementById('importe_recibir').value        = cookieArticulos[articulo][5]["AttrValue"];
    document.getElementById('descuento_recibir').value      = cookieArticulos[articulo][6]["AttrValue"];
    document.getElementById('trasladados_recibir').value    = cookieArticulos[articulo][7]["AttrValue"];
    document.getElementById('retenidos_recibir').value      = cookieArticulos[articulo][8]["AttrValue"];
    document.getElementById('estatus_recibir').value        = cookieArticulos[articulo][9]["AttrValue"];

//    console.log("cookie:  ", cookieArticulos);
//    console.log(cookieArticulos)
}

//Obtenemos la info de la compra seleccionada a recibir
function getInfo(id_compra, proveedor, moneda, fecha) {

    console.log(id_compra, proveedor, moneda, fecha);
    document.getElementById('proveedor').value  = proveedor;
    document.getElementById('moneda').value     = moneda;
    document.getElementById('fecha').value      = fecha;
    document.getElementById('OCId').value       = id_compra;

    //Creamos una cookie para pasar los valores de OC a OCRecepcion
    let OCEncabezado = [id_compra,proveedor,moneda,fecha]; 
    createCookie("OCEncabezado", JSON.stringify(OCEncabezado), 2);
    let data = {
        recepcion: {
            id: id_compra,
        }
    }
    let total = 0;
    $.ajax({
        type: "POST",
        url: 'Recepcion.aspx/getInfo',                              // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {
            resetTablaArticulosRecibir()
            // función que va a ejecutar si el pedido fue exitoso
            response_ajax = JSON.parse(resultado.d);
            document.getElementById('tablaArticulos').innerHTML = "";
            document.getElementById("selectArticulosRecibir").innerHTML = "<option value=''>-- Selecciona un artículo a recibir -- </option>";
            let lineasRecepcion = new Object();

            response_ajax['RetVal'].forEach(element => {
                lineasRecepcion[element.Attributes[2].AttrValue] = element.Attributes;
                let subtotal = 0.0000;
                //            100 -     descuento                   / 100  * (      cantidad                    *               precio          )
                try {
                    subtotal = (100 - element.Attributes[6].AttrValue) / 100 * (element.Attributes[3].AttrValue * element.Attributes[4].AttrValue);
                } catch (exception) {
                    subtotal = 0.0000;
                }
                
                document.getElementById('tablaArticulos').innerHTML += `<tr>
                                                                            <td>${element.Attributes[1].AttrValue /* Número de Linea  */}</td>
                                                                            <td>${element.Attributes[2].AttrValue /* Articúlo         */}</td>
                                                                            <td>${element.Attributes[4].AttrValue /* Precio unitario  */}</td>
                                                                            <td>${element.Attributes[3].AttrValue /* Cantidad         */}</td>
                                                                            <td>${element.Attributes[6].AttrValue /* Descuento        */}</td>
                                                                            <td>${subtotal                        /* Subtotal         */}</td>
                                                                            <td></td>
                                                                        </tr>`;

                //Agregarndo opciones al select de articulos que se pueden recibir
                document.getElementById("selectArticulosRecibir").innerHTML += `<option value='${element.Attributes[2].AttrValue}'>${element.Attributes[2].AttrValue}</option>`
                
                total += subtotal;
                document.getElementById('subtotal').innerText = `$${total}`;
                let iva = total * 0.16;
                document.getElementById('iva').innerText = "$" + iva;
                document.getElementById('total').innerText = "$" + (total + iva);
            });
            createCookie("lineasRecepcion", JSON.stringify(lineasRecepcion), 2);
            document.getElementById('btnModalProductos').removeAttribute('disabled');
        }
    });
}


$(document).ready(function () {



    if (getCookie("recepcion") === "") {

        createCookie("recepcion", "", 2);

    } else {

        productos = JSON.parse(getCookie("recepcion"));

    }

    document.getElementById('tablaArticulos').innerHTML = "";

    date();

    //creartabla();


});

var productos = [];

$('#btnGuardarArticulo').on('click', function (e) {

    e.preventDefault();

    if (validateArticulo()) {

        productos.push(getAtributos());

        createCookie("recepcion", JSON.stringify(productos), 2);

        creartabla();

    } else {
        alert('Todos los campos son necesarios');
    }

});

$('#btnFinalizar').on('click', function (e) {
    return;
    e.preventDefault();

    if (validateForm()) {

        if (productos !== []) {

            console.log("Aquí se debería de llamar a la API ")

        } else {
            alert('Agrega algún artículo a tu compra');
        }

    } else {
        alert('Todos los campos son necesarios');
    }

});

function editar(id) {
       
    var articulo = JSON.parse(getCookie("recepcion"))[id - 1].articulo;

    var array = JSON.parse(getCookie("recepcion"));

    var cantidad = jQuery('#' + id + '').val();

    for (let index = 0; index < array.length; index++) {

        const element = array[index];

        if (element.articulo == articulo) {

            element.cantidad = cantidad;
        }

    }

    createCookie("recepcion", JSON.stringify(array), 2);

   
    creartabla()


}

function date() {

    var today = new Date();

    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

    document.getElementById('fecha').value = date + " " + time;
}


function creartabla() {


    var items = JSON.parse(getCookie("recepcion"));

    var tabla = "";

    var sum = 0;

    var sumaTotal = 0;

    for (let index = 0; index < items.length; index++) {

        const element = items[index];

        sum = (element.cantidad * element.precio);

        sub = (100 - element.descuento) * .01;

        sub = sub * sum;

        sumaTotal += sub;

        tabla += `
      								    <tr>
                                            <td>` + (index + 1) + `</td>
                                            <td>` + element.articulo + `</td>
                                            <td>$` + element.precio + `</td>
                                            <td>
                                                <input style="background-color: white;" onfocusout="editar(this.id)" contenteditable="true" id="` + (index + 1) + `" value="` + element.cantidad + `">
                                                                               </td>
                                                <td>` + element.descuento + `%</td>
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


    var art = getCookie("recepcion");

    var items = JSON.parse(art);

    items.splice(item, 1);

    createCookie("recepcion", JSON.stringify(items), 2);

    creartabla();

}

function clear() {
    document.getElementById("formAtributos").reset();
}


function getAtributos() {

    data = {
        'articulo': jQuery('#articulo').val(),
        'cantidad': jQuery('#cantidad').val(),
        'precio': jQuery('#precio').val(),
        'importe': jQuery('#importe').val(),
        'descuento': jQuery('#descuento').val(),
        'trasladados': jQuery('#trasladados').val(),
        'retenidos': jQuery('#retenidos').val(),
        'estatus': jQuery('#estatus').val()
    }

    clear();

    return data;

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

function getOCID() {

    let today = new Date();
    let date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    let time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

    return "OCRecepcion" + date + time;
}