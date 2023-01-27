using System;
using System.Net;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prueba1.Client.Repositorios
{
    //Va a hacer generico, ya que podemos retornar distintos tipos de mensaje
    //Por lo cual este mismo, así se quedará para reutilizarse con otro tipo de peticiones
    public class HttpResponseWrapper<T>
    {
        //Construimos-inicializamos las propiedades
        public HttpResponseWrapper(T response, bool Error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            this.Error = Error;
            HttpResponseMessage = httpResponseMessage;
        }
        //Propiedad marcada para ver si hay errores
        public bool Error { get; set; }
        //Propiedad marcada para ver respuestas
        public T Response { get; set; }
        //Propiedad marcada para acceder a cualquier tipo de respuestas de la webApi
        public HttpResponseMessage HttpResponseMessage { get; set; }
        //Podemos crear una clase o method(en este caso es method para reutilizar el HttpResponseWrapper) para obtener informacion de errores de la webApi de forma mas especifica
        public async Task<string?> ObtenerMensajeError()
        {
            //Sino existe ningun error, no hagas nada
            if (!Error)
            {
                return null;
            }
            //Variable para guardar code de status
            var codigoEstatus = HttpResponseMessage.StatusCode;

            if (codigoEstatus == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }
            else if (codigoEstatus == HttpStatusCode.BadRequest)
            {
                //Mandamos el cuerpo del mensaje de status 
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoEstatus == HttpStatusCode.Unauthorized)
            {
                return "Tienes que tener una sesion activa para hacer esto";
            }
            else if (codigoEstatus == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta accion";
            }
            else
            {
                return "Ha ocurrido un error inesperado";
            }

        }
    }
}

//Con signo de ! indicamos si no hay, ejemplo !error= sino hay error