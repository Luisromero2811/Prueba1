using System;
using Microsoft.EntityFrameworkCore;
using Prueba1.Shared.DTOs;
namespace Prueba1.Server.Helpers
{
	public static class QueryableExtensions
	{
		//Helper para centralizar nuestra paginacion y poderla usar en cualquier parte del proyecto
		public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacion)
		{
			//Retornamos el segmentado de las distintas paginas 
			return queryable
				//Con esta funcion indicamos la cantidad de registros que vamos a mostrar por cada salto de paginacion
				.Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
				.Take(paginacion.CantidadRegistros);
		}
	}
}

