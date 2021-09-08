
let archivos = [];

function agregarArchivo(){
    console.log("Hola desde agregar archivo");
    event.preventDefault();
    let nombre_archivo = document.getElementById('nombre-archivo');
    if(nombre_archivo.value.trim() == ""){
        document.getElementById('error_agregar_archivo').innerHTML = "Por favor ingresa un valor valido";
        return;
    }else{
        document.getElementById('error_agregar_archivo').innerHTML = "";
    }

    let lista_archivos = document.getElementById('lista-archivos');

    archivos.push(nombre_archivo.value);
    console.log(archivos);
    lista_archivos.innerHTML = "";
    archivos.forEach(element => {
        lista_archivos.innerHTML += `<tr>
                                        <td>${element}</td>
                                        <td class="text-right"><button type="button" class="btn btn-link" onclick="borrarArchivo('${element}')" >Eliminar</button></td>
                                     </tr>`;
    });
                    
    nombre_archivo.value = "";
    console.log(archivos);
}
            
function borrarArchivo(archivo){
    let index = archivos.indexOf(archivo);
    archivos.splice(index,1);
    let lista_archivos = document.getElementById('lista-archivos');
    lista_archivos.innerHTML = "";

    archivos.forEach(element => {
        lista_archivos.innerHTML += `<tr>
                                        <td>${element}</td>
                                        <td class="text-right"><button type="button" class="btn btn-link" onclick="borrarArchivo('${element}')" >Eliminar</button></td>
                                     </tr>`;
    });

}


// Este ajax no sirve de manera directa
function registrarEmpresa() {
    console.log('Ejecutando funcion empresa');
    //return;
    $.ajax({
        type: "POST",                                              // tipo de request que estamos generando
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json"),
            request.setRequestHeader("Cookie", "ARRAffinity=3476a45ffaf68d2bcdb985995034447d94fa4df1ae54a194b813620782b22d6c")
        },
        url: 'http://portalproveedoresapi.azurewebsites.net/api/IncomingMessage/GetList',                // URL al que vamos a hacer el pedido
        data: {
            "connStr": null,
            "EntityName": "Empresa",
            "EntityAlias": "Empresa",
            "PKId": 0,
            "Action": 0,
            "GroupWheres": [],
            "Attributes": [
                {
                    "AttrName": "RazonSocial",
                    "AttrValue": "",
                    "AttrType": "",
                    "AttrAlias": "RazonSocial"
                }
            ],
            "ChildEntities": [],
            "getLastIdentity": false
        },                                                // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: true,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso
            console.log(resultado);
        }
    });
}

// este ajax realiza a una petición al Back donde se realiza otra petición hacia la API y nos retorna un objeto pero de tipo String
function getResponse() {
    $.ajax({
        type: "POST",              
        url: 'RegisterCompany.aspx/getCompanies',                   // URL al que vamos a hacer el pedido
        data: null,                                                // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: true,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso

            let response_ajax = "";
            response_ajax = JSON.parse(resultado.d);
            response_ajax['RetVal'].forEach(element => {
                console.log(element['Attributes'][0]['AttrValue'])
            });

        }
    });
}


function insertCompany() {
    let razon_social = document.getElementById('razon-social');
    let error_razon_social = document.getElementById('error_razon_social');
    let archivo = document.getElementById('nombre-archivo');

    if (razon_social.value == "") {
        error_razon_social.innerHTML = "Por favor ingresa la Razón Social";
        return;
    } else {
        error_razon_social.innerHTML = "";
    };

    let data = {
        empresa: {
            razon: razon_social.value,
        },
        documentos: {
            documentos: archivos
        }
    }

    $.ajax({
        type: "POST",
        url: 'RegisterCompany.aspx/insertCompany',                   // URL al que vamos a hacer el pedido
        data: JSON.stringify(data),                                                // data es un arreglo JSON que contiene los parámetros que 
        contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: true,                                               // si es asincrónico o no
        success: function (resultado) {                            // función que va a ejecutar si el pedido fue exitoso

            Swal.fire({
                icon: 'success',
                text: '¡Empresa registrada!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    });

    archivo.value = "";
    razon_social.value = "";
    console.log("terminando de registrar compañia");
    archivos.splice(0, archivos.length);


    let lista_archivos = document.getElementById('lista-archivos');
    lista_archivos.innerHTML = "";
    archivos.forEach(element => {
        lista_archivos.innerHTML += `<li class="list-group-item">
                                        <div class="row">
                                            <div class="col-sm">
                                                <label class="mt-2" runat="server" id="archivo"> ${element}</label>
                                            </div>
                                            <div class="col-sm-3">
                                                <button type="button" class="btn btn-danger" onclick="borrarArchivo('${element}')" >Eliminar</button>
                                            </div>
                                        </div>
                                    </li>`;
    });


}




