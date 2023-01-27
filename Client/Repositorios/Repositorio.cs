using System;
using System.Text;
using System.Text.Json;
using Prueba1.Shared.Controllers;
namespace Prueba1.Client.Repositorios

{
    public class Repositorio : IRepositorio
    {
        public readonly HttpClient httpClient;
        //Constructor para instanciar el httpclient
        public Repositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        private JsonSerializerOptions OpcionesPorDefectoJSON => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true 
        };
        //Estamos encapsulando al objeto para despues poderlo enviar, recibiendo un string url y darle la func con T 
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            //Vamos a serializar el objeto que vamos a enviar
            var enviarJSON = JsonSerializer.Serialize(enviar);
            //Con esta variable estamos enviando la informacion 
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            //Enviamos la peticion por method post 
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            //Encapsulamos la respuesta de la peticion 
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        //Method para no solo mandar datos por Post sino tambien deserializar la respuesta de un dato que se reciba del servidor
        //Thodo con tal de recibir el ID de la URL y poderla usar mediante POST
        //Estamos encapsulando al objeto para despues poderlo enviar, recibiendo un string url y darle la func con T 
        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            //Vamos a serializar el objeto que vamos a enviar
            var enviarJSON = JsonSerializer.Serialize(enviar);
            //Con esta variable estamos enviando la informacion 
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            //Enviamos la peticion por method post 
            var responseHttp = await httpClient.PostAsync(url, enviarContent);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<TResponse>(responseHttp, OpcionesPorDefectoJSON);
                return new HttpResponseWrapper<TResponse>(response, Error: false, responseHttp);
            }

            //Encapsulamos la respuesta de la peticion 
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> DeserializarRespuesta<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }

        public List<BlazorPeliculas> obtenerPeliculas()
        {
            return new List<BlazorPeliculas>()
{
            new BlazorPeliculas{ Titulo = "Spider No Way Home", costo_pelicula = 200,
                poster= "https://upload.wikimedia.org/wikipedia/en/0/00/Spider-Man_No_Way_Home_poster.jpg",Fecha_de_lanzamiento = new DateTime(2020,11,28)},
            new BlazorPeliculas{ Titulo = "American Me", costo_pelicula = 20,
            poster="https://upload.wikimedia.org/wikipedia/en/5/5c/American_me_poster.jpg", Fecha_de_lanzamiento = new DateTime(2020,11,29)},
            new BlazorPeliculas{ Titulo = "Sangre por Sangre", costo_pelicula = 2600,
            poster="https://upload.wikimedia.org/wikipedia/en/a/aa/Bloodinbloodout_poster.jpg",Fecha_de_lanzamiento = new DateTime(2020,11,27)},
        };
        }
    }
}

