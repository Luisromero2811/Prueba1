using System;
using Microsoft.AspNetCore.Mvc;
using Prueba1.Server.Helpers;
using Prueba1.Shared.Controllers;

namespace Prueba1.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        //Constructor para instanciar ApplicationDBContext para así poder agregar datos a actores
        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Post(Actores actores)
        {
            //Si del cliente no manda ninguna foto no se hace, por el contrario se ejecuta la accion
            if (!string.IsNullOrWhiteSpace(actores.Foto))
            {
                //convertir la foto en un arreglo de bytes
                var fotoActor = Convert.FromBase64String(actores.Foto);
                actores.Foto = await almacenadorArchivos.GuardarArchivo(fotoActor, ".jpeg", "personas");
            }
            //Recordar que en esta linea no aplicamos aun datos a la BD sino que solamente pasamos conexto para marcar que mandamos un actor a la base
            context.Add(actores);
            await context.SaveChangesAsync();
            return actores.Id;
        }
    }
}

