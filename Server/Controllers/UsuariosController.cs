using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba1.Server.Helpers;
using Prueba1.Shared.DTOs;
namespace Prueba1.Server.Controllers
{
	[ApiController]
	[Route("api/usuarios")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
	public class UsuariosController : ControllerBase
	{
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        //Constructotr
        public UsuariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
            this.context = context;
            this.userManager = userManager;
        }
		[HttpGet]
		public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginacion)
		{
			//Traemos y paginamos a los usuarios
			//Users hace referencia a la tabla de usuarios
			var queryable = context.Users.AsQueryable();
			await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable,
				paginacion.CantidadRegistros);
			return await queryable.Paginar(paginacion)
				.Select(x => new UsuarioDTO { Id = x.Id, Email = x.Email! })
				.ToListAsync();
		}
		[HttpGet("roles")]
		public async Task<ActionResult<List<RolDTO>>> Get()
		{
			return await context.Roles.Select(x => new RolDTO { Nombre = x.Name!}).ToListAsync();
			//Ocupamos de dos propiedades para asignar(editar) y eliminar el rol.
		}
		[HttpPost("asignarRol")]
		public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
		{
			//Con esta variable igualada, estamos obteniendo al usuario buscandolo por su ID
			var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

			if(usuario is null)
			{
				return BadRequest("Usuario no existente");
			}
			//Asignacion de rol al usuario, traemos al usuario y se lo asignamos
			await userManager.AddToRoleAsync(usuario, editarRolDTO.Rol);
			return NoContent();
		}
		[HttpDelete("removerRol")]
		public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
		{
			var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

			if(usuario is null)
			{
				return BadRequest("Usuario no existente");
			}
			await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.Rol);
			return NoContent();
		}
	}
}

