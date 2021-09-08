

function getInfo(id) {

    let data = {
        solicitud: {
            id: id
        }
    }

    document.getElementById("documentos").innerHTML = "";

    response = "";

    $.ajax({
        type: "POST",
        url: 'NuestrasEmpresas.aspx/getInfo',                         // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {                             // función que va a ejecutar si el pedido fue exitoso

            response_ajax = JSON.parse(resultado.d);

            response_ajax['RetVal'].forEach(element => {

                response += "<li >" + element.Attributes[2].AttrValue + "</li>";


            });

            if (response == "") {
                response = "No hay documentos solicitados"
            }

            document.getElementById("documentos").innerHTML = response;
        }
    });

}



function cerrarmodal() {

    $(".btninfo").prop('disabled', false);


}
$('#modal-info-solicitud').on('show.bs.modal', function (e) {
    $('body').addClass('test');
});