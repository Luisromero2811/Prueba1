using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Prueba1.Shared.DTOs;
using System.Text.Json.Serialization;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Prueba1.Server.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        //A partir de esta clase podemos crear los JWT para que a su vez los usuarios puedan registrarse e iniciar sesion
        //Necesitamos del constructor traer dos clases, una para representa al usuario y la otra para authenticarlo, necesitamos acceder a la llave
        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        [HttpPost("crear")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserInfo model)
        {
            var usuario = new IdentityUser { UserName = model.Email, Email = model.Email };
            var resultado = await userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }
        }

        //Method para la construccion del webtoken
        private async Task<UserTokenDTO> BuildToken(UserInfo userInfo)
        {
            //Asignamos la informacion del usuario a través de los claims que se irán a la construccion del jwt
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim("miValor", "Dua Lipa")
            };

            //Traemos al usuario
            var usuario = await userManager.FindByEmailAsync(userInfo.Email);
            var roles = await userManager.GetRolesAsync(usuario!);

            foreach(var rol in roles)
            {
                //Se agrega un claim al listado de claims
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            //Utilizar la llave para construir el JWT 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]!));
            //Crearemos las credenciales a partir de la llave y modo de cifrado
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Asignaremos la expiracion del jwt
            var expiration = DateTime.Now.AddMinutes(10);

            //creacion del token
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);
            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        //Method para login desde un usuario ya creado con token
        [HttpPost("Login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfo model)
        {
            var resultado = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Intento de inicio de sesion fallido");
            }
        }
        //EndPoint para renovar JsonWebToken
        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserTokenDTO>> Renovar()
        {
            //Construimos un userInfo para poder utilizar el method BuildToken
            var userInfo = new UserInfo()
            {
                Email = HttpContext.User.Identity!.Name!
            };
            return await BuildToken(userInfo);
        }

    }
}

