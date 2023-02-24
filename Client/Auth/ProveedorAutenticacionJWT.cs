using Prueba1.Client.Helpers;
using Prueba1.Client.Repositorios;
using Prueba1.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Prueba1.Client.Auth
{
    //Con esta clase informamos a Blazor el estado de auth de un persona mediante el uso de JWT
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private readonly IRepositorio repositorio;

        //Instancia del servicio de IJSRuntime para interactuar con localstorage
        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient, IRepositorio repositorio)
        {
            this.js = js;
            this.httpClient = httpClient;
            this.repositorio = repositorio;
        }
        //Llave de acceso
        public static readonly string TOKENKEY = "TOKENKEY";
        //Llave para guardar en el LocalStorage el tiempo de expiracion del token
        public static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";

        //Nuevo estado de auth anonimo (sin JWT)
        private AuthenticationState Anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //Se tomará la JWT para parsear y extraer los claims
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Interaccion con LocalStorage
            var token = await js.ObtenerDeLocalStorage(TOKENKEY);
            //Si el token es nulo, el usuario es anonimo y no tiene jwt, sino construimos el estado de auth
            if (token is null)
            {
                return Anonimo;
            }
            //Usar el tiempo de expiracion del Token para determinar si este mismo es valido, no basta con tener el token si ha finalizado su tiempo de vida
            var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
            DateTime tiempoExpiracion;

            //Si el usuario no tiene el tiempo de expiracion del token
            if (tiempoExpiracionObject is null)
            {
                await Limpiar();
                return Anonimo;
            }
            //Parseamos(leer o analizar) el string del LocalStorage para determinar si ya esta expirado, para esto necesitamos de otros methos auxiliares para corroborar ese dato
            if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                //si el token ya expiro, se limpia y se retorna anonimo
                if(TokenExpirado(tiempoExpiracion))
                {
                    await Limpiar();
                    return Anonimo;
                }
                if(DebeRenovarToken(tiempoExpiracion))
                {
                    token = await RenovarToken(token.ToString()!);
                }
            }
           
            return ConstruirAuthenticationState(token.ToString()!);
        }
        //Method que nos indique si un token ya debe de ser renovado
        private bool DebeRenovarToken(DateTime tiempoExpiracion)
        {
            //Si la diferencia del tiempo de expiracion y la hora actual es menor a cinco minutos, deberemos renovar  el token
            return tiempoExpiracion.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(9);
        }

        //Func que se va a ejecutar cada 4 minutos desde RenovadorToken
        public async Task ManejarRenovacionToken()
        {
            var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
            DateTime tiempoExpiracion;

            if(DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                if(TokenExpirado(tiempoExpiracion))
                {
                    await Logout();
                }
                if(DebeRenovarToken(tiempoExpiracion))
                {
                    var token = await js.ObtenerDeLocalStorage(TOKENKEY);
                    var nuevoToken = await RenovarToken(token.ToString()!);
                    var authState = ConstruirAuthenticationState(nuevoToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }
            }
        }

        //Method para renovar el token
        private async Task<string> RenovarToken(string token)
        {
            Console.WriteLine("Renovando el token...");
            //Aseguramos que el HttpClient tenga el token
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            //Peticion a controlador de cuentas trayendo el UserTokenDTO
            var nuevoTokenResponse = await repositorio.Get<UserTokenDTO>("api/cuentas/RenovarToken");
            //Extraer nuevo token
            var nuevoToken = nuevoTokenResponse.Response!;
            //Guardamos el nuevo token en el localstorage
            await js.GuardarEnLocalStorage(TOKENKEY, nuevoToken.Token);
            await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, nuevoToken.Expiration.ToString());
            return nuevoToken.Token;
        }

        //Method para determinar si el token ya esta expirado
        private bool TokenExpirado(DateTime tiempoExpiracion)
        {
            return tiempoExpiracion <= DateTime.UtcNow;
        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            //Cada que usemos httpclient a través de este se estará mandando el JWT
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);
            var claims = ParsearClaimsDelJWT(token);
            //Retornamos el authenticationsate
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }
        //Method para obtener los claims
        //Se toma el token, se lee y se le extraen los claims
        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }

        public async Task Login(UserTokenDTO tokenDTO)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, tokenDTO.Token);
            await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, tokenDTO.Expiration.ToString());
            var authState = ConstruirAuthenticationState(tokenDTO.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Limpiar();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
        //Method auxiliar para logica de limpiar la data relacionada al token
        private async Task Limpiar()
        {
            await js.RemoverDeLocalStorage(TOKENKEY);
            await js.RemoverDeLocalStorage(EXPIRATIONTOKENKEY);
            //Así removemos el token del httpclient
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
//Con este nuevo authstate provider vamos a obtener los claims del usuario apartir del JWT asignado de la webAPI.
//Se usan method independientes, donde traemos el estado de auth, por lo cual, construimos el estado usando un method para traer el token, leerlo y obtener los claims inyectados