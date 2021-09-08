
let listaEmpresas;


//Cargar información - Eventos: Cargar select empresas, Evento click para boton agregar empresa
$(document).ready(function () {


    /*empresasList = document.getElementById('MainContent_Empresas').value;
    $("#MainContent_Empresas").remove();
    empresasList = JSON.parse(empresasList);

    empresasList['RetVal'].forEach(element => {
        //console.log(element['Attributes'][1]['AttrValue'])
        $('#empresa ').append('<li>' + element['Attributes'][0]['AttrValue'] + '</li>');
    });*/

    //INFORMACIÓN DE LA SOLICITUD CON EL ID
    /*let data = {
        solicitud: {
            id: 33
        }
    }*/

    /*$.ajax({
        type: "POST",
        url: 'NuestrasEmpresas.aspx/getSolicitudById',              // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {                             // función que va a ejecutar si el pedido fue exitoso
            console.log(resultado);
        }
        }).done(function (resultado) {
            *//*response_ajax = JSON.parse(resultado.d);
            response_ajax['RetVal'].forEach(element => {

                $('#id_Solicitud').text(element.Attributes[0].AttrValue);
                $('#razon').text(element.Attributes[1].AttrValue);
                $('#rfc').text(element.Attributes[2].AttrValue);
                $('#correo').text(element.Attributes[3].AttrValue);

            });*//*

        }).fail(function () {
            console.log('Algo salió mal. Intenta más tarde.')
        });*/
    
});


