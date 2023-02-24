using System;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
namespace Prueba1.Client.Auth
{
    public class ProovedorAutenticacionPrueba : AuthenticationStateProvider
    {
        //Con esta clase informamos a Blazor el estado de auth de un persona
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            //Method que devuelve el estado de auth del usuario
            var anonimo = new ClaimsIdentity();
            var usuarioLuis = new ClaimsIdentity(
                new List<Claim>
                {
                   new Claim(ClaimTypes.Name, "Luis Romero"),
                   new Claim(ClaimTypes.Country, "Germany"),
                   new Claim(ClaimTypes.Email, "luis@gmail.com"),
                   new Claim(ClaimTypes.Role, "admin")
                },
                authenticationType: "Prueba");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(usuarioLuis)));
        }
    }
 //Claims son datos relacionados acerca del usuario como nombre, edad, etc.
 //Con AuthorizeView mostramos u ocultamos elementos de la vista segun el estado de
 //Con Identity nos permite crear y configurar un sistema de usuarios para permitir que ellos se registen, inicien sesión, asignarles roles y permisos
 //El mismo Identity funciona para ser configurable, no es un paquete rigido el cual no pueda moverse
 //Con IdentityDBContext nos ayuda para obtener una serie de entidades propias para el usuario, tales como tabla de usuario, roles, etc.
 //Con JsonWebToken nos permite auth al usuario, con estos mismos ciframos los claims del usuario mediante el uso de una llave secreta
}

