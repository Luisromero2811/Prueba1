using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba1.Shared.DTOs;
using Prueba1.Shared.Controllers;
namespace Prueba1.Server.Controllers
{
    [Route("api/votos")]
    [ApiController]
    //Proteger los controladores, si estas auth puedes usarlos, de lo contrario dará error
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VotoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;

        public VotoController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        //Method POST para asignarles votos a las peliculas
        [HttpPost]
        public async Task<ActionResult> Votar(VotoPeliculaDTO votoPeliculaDTO)
        {
            //Vamos a traer el registro del usuario mediante su ID asignado porque el será el encargado de dar voto al elemento pelicula
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity!.Name!);
            //Si el usuario es nulo mandamos un BadRequest
            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }
            var usuarioId = usuario.Id;
            //Corroboramos si el usuario actual ya voto por la pelicula
            var votoActual = await context.VotosPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == votoPeliculaDTO.PeliculaId
                && x.UsuarioId == usuarioId);
            //Si el usuario no ha votado por la pelicula
            if (votoActual is null)
            {
                //Se crea un nuevo registro en la tabla VotosPelicula
                var votoPelicula = mapper.Map<VotoPelicula>(votoPeliculaDTO);
                votoPelicula.UsuarioId = usuarioId;
                votoPelicula.FechaVoto = DateTime.Now;
                context.Add(votoPelicula);
                
            }//Sino actualizamos el registro actual
            else
            {
                votoActual.FechaVoto = DateTime.Now;
                votoActual.Voto = votoPeliculaDTO.Voto;
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

