using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba1.Server.Helpers;
using Prueba1.Shared.Controllers;
using Prueba1.Shared.DTOs;

namespace Prueba1.Server.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    //Proteger los controladores, si estas auth puedes usarlos, de lo contrario dará error
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "personas";

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.userManager = userManager;
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
            //Condicion para ver si existen peliculasactor se le sume 1 valor a su orden 
            EscribirOrdenActores(peliculas);

            context.Add(peliculas);
            await context.SaveChangesAsync();
            return peliculas.Id;
        }

        private static void EscribirOrdenActores(BlazorPeliculas peliculas)
        {
            if (peliculas.PeliculasActor is not null)
            {
                for (int i = 0; i < peliculas.PeliculasActor.Count; i++)
                {
                    peliculas.PeliculasActor[i].Orden = i + 1;
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            //Aplicamos una variable para indicar un limite de pelicula que queremos mostrar
            var Limite = 6;
            //Aplicamos un filtrado de peliculas en cartelera
            var peliculasEnCartelera = await context.Peliculas
                //Filtramos donde la pelicula este en cartelera teniendo un limite ya especifico
                .Where(pelicula => pelicula.EnCartelera).Take(Limite)
                //Los cuales los resultados los ordenamos de forma descendente a traves de la fecha de lanzamiento
                .OrderByDescending(pelicula => pelicula.Fecha_de_lanzamiento)
                //Mostrando los resultados a traves de una lista
                .ToListAsync();
            // variable para checar la fecha actual
            var fechaActual = DateTime.Today;
            //Aplicamos una variable para las peliculas proximas a estrenar
            var peliculasEstreno = await context.Peliculas
                .Where(pelicula => pelicula.Fecha_de_lanzamiento > fechaActual)
                .OrderBy(pelicula => pelicula.Fecha_de_lanzamiento).Take(Limite)
                .ToListAsync();

            var resultado = new HomePageDTO
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = peliculasEstreno
            };
            return resultado;
        }

        //Method para filtrar las peliculas
        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<BlazorPeliculas>>> Get([FromQuery] ParametrosBusquedaPelicuaDTO modelo)
        {
            //Con ejecucion diferida armamos los query paso por paso, sentencia por sentencia se realiza el filtrado que nosotros queramos hacer
            //ASQueryable es un tipo de dato que se va a ir armando
            var peliculasQueryable = context.Peliculas.AsQueryable();
            //Si el titulo no es nulo, si es diferente a nulo o espacio en blanco
            if (!string.IsNullOrWhiteSpace(modelo.Titulo))
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.Titulo.Contains(modelo.Titulo));
            }
            //Si modelo en cartelera es verdadero ejecutamos la accion de filtro
            if (modelo.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.EnCartelera);
            }
            if (modelo.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.Fecha_de_lanzamiento >= hoy);
            }
            //Si el genero es distinto de cero
            if (modelo.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.GenerosPelicula
                    .Select(y => y.GeneroId)
                    .Contains(modelo.GeneroId));
            }
            //Filtrar por la mas votadas
            if(modelo.MasVotadas)
            {
                peliculasQueryable = peliculasQueryable.OrderByDescending(p => p.VotosPeliculas.Average
                (vp => vp.Voto));
            }

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(peliculasQueryable, modelo.CantidadRegistros);
            var peliculas = await peliculasQueryable.Paginar(modelo.PaginacionDTO).ToListAsync();
            return peliculas;
        }

        //Endpoint para obtener una pelicula mediante su ID
        //Poder utilizar una variable de ruta
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            //variable pelicula siendo igualada al contexto de la Tabla BD Peliculas, donde filtraremos la pelicula sera igual al ID de la pelicula
            var peliculas = await context.Peliculas.Where(peliculas => peliculas.Id == id)
                           //Cargaremos la data relacionada a una pelicula, en este caso a los
                           //Incluiremos la propiedad de navegacion de GenerosPelicula
                           .Include(peliculas => peliculas.GenerosPelicula)
                                //Entonces incluye Genero para obtener la data del genero 
                                .ThenInclude(gp => gp.Genero)
                           //Realizaremos lo mismo para los actores
                           .Include(peliculas => peliculas.PeliculasActor.OrderBy(pa => pa.Orden))
                                .ThenInclude(pa => pa.Actor)
                           //Retorno de solamente un registro
                           .FirstOrDefaultAsync();
            if (peliculas is null)
            {
                return NotFound();
            }
            var promedioVoto = 0.0;
            var votoUsuario = 0;
            //Si la pelicula ha sido votada por alguien, 
            if (await context.VotosPeliculas.AnyAsync(x => x.PeliculaId == id))
            {
                promedioVoto = await context.VotosPeliculas.Where(x => x.PeliculaId == id)
                .AverageAsync(x => x.Voto);
            }
            //Si el usuario esta auth podra votar, de lo contrario no podra hacer nada
            if(HttpContext.User.Identity!.IsAuthenticated)
            {
                //Vamos a traer el registro del usuario mediante su ID asignado porque el será el encargado de dar voto al elemento pelicula
                var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity!.Name!);
                //Si el usuario es nulo mandamos un BadRequest
                if (usuario is null)
                {
                    return BadRequest("Usuario no encontrado");
                }
                var usuarioId = usuario.Id;
                //Si el usuario ya voto en el pasado
                var votoUsuarioDB = await context.VotosPeliculas
                    .FirstOrDefaultAsync(x => x.PeliculaId == id && x.UsuarioId == usuarioId);
                if(votoUsuarioDB is not null)
                {
                    votoUsuario = votoUsuarioDB.Voto;
                }
            }
            var modelo = new PeliculaVisualizarDTO();
            modelo.Pelicula = peliculas;
            modelo.Generos = peliculas.GenerosPelicula.Select(gp => gp.Genero!).ToList();
            modelo.Actores = peliculas.PeliculasActor.Select(pa => new Actores
            {
                Nombre = pa.Actor!.Nombre,
                Foto = pa.Actor.Foto,
                Personaje = pa.Personaje,
                Id = pa.ActorId
            }).ToList();

            modelo.PromedioVotos = promedioVoto;
            modelo.VotoUsuario = votoUsuario;
            //Antes de retornar la peticion, tenemos que indicar en Program que no permita ningun error de serializacion de Json ante eventos ciclicos
            return modelo;
        }
        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            //Variable donde invoca la funcion Get de obtencion de la pelicula mediante su ID
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            //Proyectamos para traer los elementos seleccionados
            //Igualamos al valor de visualizarpelicula DTO
            var peliculaVisualizarDTO = peliculaActionResult.Value;
            //Buscamos los ID de los generos seleccionados
            var generosSeleccionadosIds = peliculaVisualizarDTO!.Generos.Select(x => x.Id).ToList();
            //Traemos thodos los generos que no sean seleccionados de la BD 
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            var modelo = new PeliculaActualizacionDTO();
            modelo.Pelicula = peliculaVisualizarDTO.Pelicula;
            modelo.GeneroNoSeleccionado = generosNoSeleccionados;
            modelo.GenerosSeleccionados = peliculaVisualizarDTO.Generos;
            modelo.Actor = peliculaVisualizarDTO.Actores;

            return modelo;
        }
        [HttpPut]
        public async Task<ActionResult> Put(BlazorPeliculas pelicula)
        {
            //Variable actorDB igualada al contexto de tabla actores donde encontrará al primero con su ID 
            var peliculaDB = await context.Peliculas
                .Include(x => x.GenerosPelicula)
                .Include(x => x.PeliculasActor)
                .FirstOrDefaultAsync(p => p.Id == pelicula.Id);

            if (peliculaDB is null)
            {
                return NotFound();
            }
            peliculaDB = mapper.Map(pelicula, peliculaDB);
            if (!string.IsNullOrWhiteSpace(pelicula.poster))
            {
                var posterImagen = Convert.FromBase64String(pelicula.poster);
                peliculaDB.poster = await almacenadorArchivos.EditarArchivo(posterImagen, ".jpg", contenedor, peliculaDB.poster!);
            }
            EscribirOrdenActores(peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            // igualamos al actor con la tabla actores trayendo al primero por su ID
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            //Si el actor es nulo retornamos un NotFound
            if (pelicula is null)
            {
                return NotFound();
            }
            //Eliminamos al actor
            context.Remove(pelicula);
            //Guardamos cambios
            await context.SaveChangesAsync();
            //Eliminamos la foto del actor del entorno
            await almacenadorArchivos.EliminarArchivo(pelicula.poster!, contenedor);
            return NoContent();
        }

    }
}
//Con los DTOs podemos tener una clase la cual va a contener datos