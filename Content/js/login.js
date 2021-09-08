function validarFormLogin() {
    let input_correo = document.getElementById('inputCorreo');
    let error_correo = document.getElementById('error_correo');
    let input_contrasena = document.getElementById('inputContrasena'); 
    let error_contrasena = document.getElementById('error_contrasena');
    let error_formulario = document.getElementById('error_formulario');


    if(input_correo.value == "" || input_contrasena.value == "") {
        input_correo.value == "" ? error_correo.innerText = "Ingresa tu correo electrónico para poder continuar." : error_correo.innerText = "";
        input_contrasena.value == "" ? error_contrasena.innerText = "Ingresa tu contraseña poder continuar." : error_contrasena.innerText = "";
        error_formulario.innerText == "Algunos campos presentan error.";
    }else {
        error_contrasena.innerText = "";
        error_correo.innerText = "";
        error_formulario.innerText = "";
        console.log(document.getElementById("formLogin"));
    }
}