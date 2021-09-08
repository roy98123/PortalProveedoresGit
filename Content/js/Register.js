//Registro

let empresasList;
let documentosList;
let nuevasEmpresas = [];

//Cargar información - Eventos: Cargar select empresas, Evento click para boton agregar empresa
$(document).ready(function () {

    limpiarForm();

    $('#empresas0 option').remove();
    $('#empresas0 ').append('<option value="" selected>Selecciona una opción</option>');

    empresasList = document.getElementById('Empresas').value;
    documentosList = document.getElementById('Documentos').value;
    $("#Empresas").remove();
    $("#Documentos").remove();
    empresasList = JSON.parse(empresasList);
    documentosList = JSON.parse(documentosList);

    empresasList['RetVal'].forEach(element => {
        //console.log(element['Attributes'][1]['AttrValue'])
        $('#empresas0 ').append('<option value="' + element['Attributes'][1]['AttrValue'] + '">' + element['Attributes'][0]['AttrValue'] + '</option>');
    });
    T

    $('#btnAgregarEmpresa').click(function () {
        //$('#ContenidoDinamico').append('<span id="texto">Texto ingresado</span>');


        if (nuevasEmpresas.length == 0) {
            var ultimo = 1;
        }
        else {
            var ultimo = nuevasEmpresas[nuevasEmpresas.length - 1] + 1;
        }
        nuevasEmpresas.push(ultimo);

        //console.log(nuevasEmpresas);

        $('#ContenidoDinamico').append(
            '<div  id="div' + ultimo + '"> <hr> <br>' +
            '<div>' +
            '<div>' +
            '<select id="empresas' + ultimo + '" class="form-control select2bs4"  style="max-width: inherit; width:100;" onchange="Documentos(' + ultimo + ')" required>' +
            '<option value="" selected>Selecciona una empresa</option>' +
            '</select>' +
            '<div id="Documentos' + ultimo + '">' +
            '</div>' +
            '</div>' +
            '<div>' +
            '<br><input type="button btn-danger" id="btnEliminarEmpresa' + ultimo + '" onclick="EliminarEmpresa(' + ultimo + ')" class="btn btn-danger" value="Eliminar" />' +
            '</div>' +
            '</div>' +
            '<br />' +
            '</div>'
        );
        empresasList['RetVal'].forEach(element => {
            $('#empresas' + ultimo).append('<option value="' + element['Attributes'][1]['AttrValue'] + '">' + element['Attributes'][0]['AttrValue'] + '</option>');
        });
        document.getElementById("btnAgregarEmpresa").disabled = true;
    });

    //SUBMIT DEL FORMULARIO REGISTRO
    $('#btnPrueba').click(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var listaEmpresas = document.getElementById("empresas0");
        var selectedIdEmpresa = listaEmpresas.options[listaEmpresas.selectedIndex].value;

        var empresasArray = [];
        var documentosArray = [];
        documentosList['RetVal'].forEach(element => {
            if (selectedIdEmpresa == element['Attributes'][1]['AttrValue']) {
                var input = document.getElementById('documento0.' + element['Attributes'][0]['AttrValue']);
                var file = input.files[0];
                documentosArray.push({ idDocumento: element['Attributes'][0]['AttrValue'], nombre: file.name, contenido: document.getElementById('documento0.' + element['Attributes'][0]['AttrValue'] + '.base64').value, tipo: file.type });
            }
        });
        empresasArray.push({ idEmpresa: selectedIdEmpresa, documentos: documentosArray });

        // EMPRESAS, DINAMICOS
        if (nuevasEmpresas.length != 0) {
            for (var i = 0; i < nuevasEmpresas.length; i++) {
                listaEmpresas = document.getElementById("empresas" + nuevasEmpresas[i]);
                selectedIdEmpresa = listaEmpresas.options[listaEmpresas.selectedIndex].value;

                documentosArray = [];
                documentosList['RetVal'].forEach(element => {
                    if (selectedIdEmpresa == element['Attributes'][1]['AttrValue']) {
                        var input = document.getElementById('documento' + nuevasEmpresas[i] + '.' + element['Attributes'][0]['AttrValue']);
                        var file = input.files[0];
                        documentosArray.push({ idDocumento: element['Attributes'][0]['AttrValue'], nombre: file.name, contenido: document.getElementById('documento' + nuevasEmpresas[i] + '.' + element['Attributes'][0]['AttrValue'] + '.base64').value, tipo: file.type });
                    }
                });
                empresasArray.push({ idEmpresa: selectedIdEmpresa, documentos: documentosArray });
            }
        }

        //validateForm

        if (validateForm()) {

            if (validarContrasena($('#contrasena').val())) {

                var data = {
                    proveedor: {
                        razon: $('#razon').val(),
                        rfc: $('#rfc').val(),
                        correo: $('#correo').val(),
                        contrasena: $('#contrasena').val(),
                        empresas: empresasArray
                    }
                };


                $.ajax({
                    type: "POST",                                              // tipo de request que estamos generando
                    url: 'Register.aspx/Registrar',                            // URL al que vamos a hacer el pedido
                    data: JSON.stringify(data),                                // data es un arreglo JSON que contiene los parámetros que 
                    contentType: "application/json; charset=utf-8",            // tipo de contenido
                    dataType: "json",                                          // formato de transmición de datos
                    async: true,                                               // si es asincrónico o no
                    success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso
                        let response_ajax = "";
                        let respuesta = "";
                        response_ajax = JSON.parse(resultado.d);
                        response_ajax.forEach(element => {
                            respuesta = element['Success'];
                        });

                        if (respuesta) {

                            Swal.fire({
                                icon: 'success',
                                text: '¡Solicitud enviada!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            limpiarForm()

                            setInterval($(location).attr('href', 'Login.aspx'), 5000);


        
                        } else {
                            Swal.fire({
                                icon: 'error',
                                text: 'Hubo un error, por favor intenta más tarde.',
                                showConfirmButton: false,
                                timer: 3000
                            })
                        }
                    }
                });
            } else {

                Swal.fire({
                    icon: 'error',
                    text: 'La contraseña debe contener por lo menos ocho caracteres, una mayúscula, un número y un caracter especial.',
                    showConfirmButton: false,
                    timer: 7000
                })

            }
        } else {

            Swal.fire({
                icon: 'error',
                text: 'Todos los campos son requeridos',
                showConfirmButton: false,
                timer: 7000
            })
        }



    });
});

//Archivos
function CargarBase64(noEmpresa, idDoc) {
    var input = document.getElementById('documento' + noEmpresa + '.' + idDoc);
    var file = input.files[0];

    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        var base64 = reader.result;
        /*var arrayAux = base64.split(',');
        base64 = arrayAux[1];
        console.log(reader.result);*/
        document.getElementById('documento' + noEmpresa + '.' + idDoc + '.base64').value = base64;
    }
}

//Generar inputs para los documentos
function Documentos(noEmpresa) {
    if (nuevasEmpresas.length > 0) {
        if (noEmpresa == nuevasEmpresas[nuevasEmpresas.length - 1]) {
            document.getElementById("btnAgregarEmpresa").disabled = false;
        }
    }
    else {
        document.getElementById("btnAgregarEmpresa").disabled = false;
    }
    $("#empresas" + noEmpresa + " option[value='']").remove();
    var listaEmpresas = document.getElementById("empresas" + noEmpresa);
    $('#Documentos' + noEmpresa).empty();
    //console.log(listaEmpresas.options[listaEmpresas.selectedIndex].value);
    documentosList['RetVal'].forEach(element => {
        if (listaEmpresas.options[listaEmpresas.selectedIndex].value == element['Attributes'][1]['AttrValue']) {
            $('#Documentos' + noEmpresa).append(
                '<br />' +
                '<label for="documento' + noEmpresa + '.' + element['Attributes'][0]['AttrValue'] + '">' + element['Attributes'][2]['AttrValue'] + '</label><br />' +
                '<input type="file" class="input-group" id="documento' + noEmpresa + '.' + element['Attributes'][0]['AttrValue'] + '" onchange="CargarBase64(' + noEmpresa + ', ' + element['Attributes'][0]['AttrValue'] + ')" required />' +
                '<input type="text" id="documento' + noEmpresa + '.' + element['Attributes'][0]['AttrValue'] + '.base64" hidden/>'
            );
        }
    });
}

//Eliminar contenido dinamico de nuevas empresas
function EliminarEmpresa(noEmpresa) {
    for (var i = 0; i < nuevasEmpresas.length; i++) {
        if (noEmpresa == nuevasEmpresas[i]) {
            nuevasEmpresas.splice(i, 1);
        }
    }
    $("#div" + noEmpresa).remove();
    document.getElementById("btnAgregarEmpresa").disabled = false;
    //console.log(nuevasEmpresas);
}


function limpiarForm() {

    $("#razon").val('');

    $("#rfc").val('');

    $("#correo").val('');

    $("#contrasena").val('');

}



function validateForm() {

    let rfc = jQuery('#rfc').val();
    let razon = jQuery('#razon').val();
    let correo = jQuery('#correo').val();
    let contrasena = jQuery('#contrasena').val();

    var validate = true;

    if (rfc === "") {
        validate = false;
    }
    if (razon === "") {
        validate = false;
    }
    if (correo === "") {
        validate = false;
    }
    if (contrasena === "") {
        validate = false;
    }

    return validate;

}


//Validar contrasena


function validarContrasena(contrasena) {

    var validate = true;

    var invalido = ['/']; //no sé qué otros chars no están permitidos pero así los agregamos aquí :B

    var pattern = /^(?=.{5,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W])/

    if (contrasena.length < 64 && contrasena.length > 7) {

        var checkval = pattern.test(contrasena);

        if (!checkval) {
            validate = false;
        }

        invalido.forEach(element => {
            for (var i = 0; i < contrasena.length; i++) {

                if (contrasena[i] === element) {

                    validate = false;

                }
            }

        });

    } else {
        validate = false;
    }


    return validate;

}