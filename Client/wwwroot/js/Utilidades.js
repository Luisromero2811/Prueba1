function pruebaPuntoNetStatic() {
    DotNet.invokeMethodAsync("Prueba1.Client", "ObtenerCurrentCount").then(resultado => {
        console.log('conteo desde JS' + resultado);
    })
}
