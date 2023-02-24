function pruebaPuntoNetStatic() {
    DotNet.invokeMethodAsync("Prueba1.Client", "ObtenerCurrentCount").then(resultado => {
        console.log('conteo desde JS' + resultado);
    })
}

function timerInactivo(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 3*10000);
    }
    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}