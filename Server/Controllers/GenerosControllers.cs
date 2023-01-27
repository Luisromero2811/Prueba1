using System;
using Microsoft.AspNetCore.Mvc;
using Prueba1.Shared.Controllers;

namespace Prueba1.Server.Controllers
{
    //Ruta de acceso para consumo de webAPI
	[Route("api/generos")]
	//Parametro para indicar que el controlador debe funcionar como una API
	[ApiController]
	//A traves de estas clases de controllers, podemos a traves del client mandar peticiones HTTP que a su vez esta misma clase las va a recibir peticiones HTTP para poder interactuar con la Base de datos
	public class GenerosControllers : ControllerBase
	{
        private readonly ApplicationDbContext context;

        //Constructor para instaciar ApplicationDBContext para así poder ingresar generos a nuestra base
        public GenerosControllers(ApplicationDbContext context)
		{
			this.context = context;
		}
		//Peticion a traves de method post para ingresar un nuevo genero
		[HttpPost]		//Mensaje gen //Si queremos retornar info //Clase //Tabla
		public async Task<ActionResult<int>> Post(Genero genero)
		{
			//Le pasamos el contexto solo para marcar que le mandamos un genero a la base
			//Marcamos el objeto para ser guardado en la DB
			context.Add(genero);
			//Aqui realmente es donde se inserta el genero directo a la base de datos
			await context.SaveChangesAsync();
			return genero.Id;
        }
	}
}
//Con HTTPClient podemos consultar el status del method en el controller
//Centralizar la logica de hacer una peticion en una clase de repositorio
//Por lo cual se van a centralizar las respuestas de la webAPI para obtener status de funciones

