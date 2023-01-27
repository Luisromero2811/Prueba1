using System;
using Microsoft.AspNetCore.Mvc;
using Prueba1.Server.Helpers;
using Prueba1.Shared.Controllers;

namespace Prueba1.Server.Controllers
{
	[Route("api/peliculas")]
	[ApiController]
	public class PeliculasController : ControllerBase
	{
		private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
		{
			this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }
		[HttpPost]
		public async Task<ActionResult<int>> Post(BlazorPeliculas peliculas)
		{
            //Si del cliente no manda ninguna foto no se hace, por el contrario se ejecuta la accion
            if (!string.IsNullOrWhiteSpace(peliculas.poster))
            {
                //convertir la foto en un arreglo de bytes
                var fotoActor = Convert.FromBase64String(peliculas.poster);
                peliculas.poster = await almacenadorArchivos.GuardarArchivo(fotoActor, ".jpeg", "personas");
            }
            context.Add(peliculas);
			await context.SaveChangesAsync();
			return peliculas.Id;
		}
	}
}

