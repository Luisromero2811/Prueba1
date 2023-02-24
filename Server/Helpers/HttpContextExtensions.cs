using System;
using Microsoft.EntityFrameworkCore;
namespace Prueba1.Server.Helpers
{
    public static class HttpContextExtensions
    {
        //Indicamos al cliente la cantidad de páginas que vamos a anexar, lo inyectaremos directo de la cabecera del cliente http
        public async static Task InsertarParametrosPaginacionEnRespuesta<T>(this HttpContext context, IQueryable<T> queryable, int cantidadRegistrosAMostrar)

        {
            //IQueryable es el tipo de dato que en entity se usa para ir armando la
            //
            if (context is null) { throw new ArgumentNullException(nameof(context)); }
            //Contaremos la cantidad de registros mostrados
            double conteo = await queryable.CountAsync();
            //Contaremos la cantidad de paginas
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            //Manipularemos la respuesta HTTP
            context.Response.Headers.Add("conteo", conteo.ToString());
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}

