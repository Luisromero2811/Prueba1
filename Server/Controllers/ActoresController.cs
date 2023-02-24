using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba1.Server.Helpers;
using Prueba1.Shared.Controllers;
using Prueba1.Shared.DTOs;

namespace Prueba1.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    //Proteger los controladores, si estas auth puedes usarlos, de lo contrario dará error
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "personas";
        private readonly IMapper mapper;
        

        //Constructor para instanciar ApplicationDBContext para así poder agregar datos a actores
        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
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
        //Traemos una lista de actores los cuales ya estan almacenados en la base de datos
        //Con QueryString podremos traer tipos de datos complejos mediante func GET
        //
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actores>>> Get([FromQuery]PaginacionDTO paginacion)
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);
            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacion).ToListAsync();
        }
        //A traves de este method Get podemos filtrar el nombre del actor mediante el textarea, con variable textobusqueda cambiara la URL 
        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Actores>>> Get(string textoBusqueda)
        {
            if(string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Actores>();
            }
            textoBusqueda = textoBusqueda.ToLower();
            //Si el string no es nulo, retornamos un filtro usando where, En el contexto de Actores donde "filtro" el nombre del actor siempre sea en minusculas contenga lo llenado en variable textoBusqueda y retorne una lista
            return await context.Actores
                .Where(x => x.Nombre.ToLower().Contains(textoBusqueda))
                .Take(6)
                .ToListAsync();
        }
        [HttpGet("{id:int}")]
        //Estamos recibiendo un actor para anexarlo al textbox
        public async Task<ActionResult<Actores>> Get(int id)
        {
            //En caso de que no exista el actor retornamos notfound en caso de que exista lo retornamos
            var actores = await context.Actores.FirstOrDefaultAsync(actores => actores.Id == id);
            if (actores is null)
            {
                return NotFound();
            }
            return actores;
        }
        //Method put de forma diferente debido al campo de foto, por lo cual la manejaremos de forma especial, si la foto viene nula el usuario ignoro el cambio de la misma, sino, lo va a guardar con Automapper
        [HttpPut]
        public async Task<ActionResult> Put(Actores actor)
        {
            //Variable actorDB igualada al contexto de tabla actores donde encontrará al primero con su ID 
            var actorDB = await context.Actores.FirstOrDefaultAsync(a => a.Id == actor.Id);

            if(actorDB is null)
            {
                return NotFound();
            }
            //Con automapper indicamos que tome las propiedades del actor y las pase a actorDB
            actorDB = mapper.Map(actor, actorDB);

            if(!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actorDB.Foto = await almacenadorArchivos.EditarArchivo(fotoActor, ".jpg", contenedor, actorDB.Foto!);
            }

            await context.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //igualamos al actor con la tabla actores trayendo al primero por su ID
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            //Si el actor es nulo retornamos un NotFound
            if (actor is null)
            {
                return NotFound();
            }
            //Eliminamos al actor
            context.Remove(actor);
            //Guardamos cambios
            await context.SaveChangesAsync();
            //Eliminamos la foto del actor del entorno
            await almacenadorArchivos.EliminarArchivo(actor.Foto!, contenedor);
            return NoContent();
        }
    }
}

