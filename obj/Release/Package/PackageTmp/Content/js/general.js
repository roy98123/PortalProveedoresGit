console.log("general.js")
const numeros = document.querySelectorAll('.numeros');
numeros.forEach(numero => numero.onkeypress = soloNumeros);

//Solo permite introducir números.
function soloNumeros(e) {
    let key = e.charCode;
    return key >= 48 && key <= 57;
}