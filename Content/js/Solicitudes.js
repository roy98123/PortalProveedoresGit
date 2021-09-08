function getInfo(id) {

    let data = {
        solicitud: {
            id: id
        }
    }

    $.ajax({
        type: "POST",
        url: 'Solicitudes.aspx/getInfo',                         // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {                             // función que va a ejecutar si el pedido fue exitoso

            response_ajax = JSON.parse(resultado.d);

            response_ajax['RetVal'].forEach(element => {

                document.getElementById('MainContent_id_Solicitud').innerHTML = element.Attributes[0].AttrValue;
                document.getElementById('MainContent_razon').innerHTML = element.Attributes[1].AttrValue;
                document.getElementById('MainContent_rfc').innerHTML = element.Attributes[2].AttrValue;
                document.getElementById('MainContent_correo').innerHTML = element.Attributes[3].AttrValue;

            });

        }
    });

    getEmpresasDocs(id);
}


function getEmpresasDocs(id) {

    document.getElementById('MainContent_divListaEmpresas').innerHTML = "<div class=\"col-md-12\"><h5>Cargando contenido...</h5></div>";

    let data = {
        solicitud: {
            id: id
        }
    }

    $.ajax({
        type: "POST",
        url: 'Solicitudes.aspx/getEmpresasDocs',                         // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {
            //console.log(resultado);
            //response_ajax = JSON.parse(resultado);
            
            document.getElementById('MainContent_divListaEmpresas').innerHTML = "";
            document.getElementById('MainContent_divListaEmpresas').innerHTML = resultado.d;
        }
    });

}


function Aprobacion(op) {
    let id = document.getElementById('MainContent_id_Solicitud').innerText;
    let data = {
        solicitud: {
            id: id,
            op: op
        }
    }
 
    $.ajax({
        type: "POST",
        url: 'Solicitudes.aspx/ActualizarEstatus',                   // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                 // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",             // tipo de contenido
        dataType: "json",                                           // formato de transmición de datos
        async: true,                                                // si es asincrónico o no
        success: function (resultado) {                             // función que va a ejecutar si el pedido fue exitoso
            //console.log(resultado);
            //console.log(JSON.parse(resultado.d));
            window.location.reload();
        }
    })  
}


function cerrarmodal() {

    $(".btninfo").prop('disabled', false);


}
$('#modal-info-solicitud').on('show.bs.modal', function (e) {
    $('body').addClass('test');
});