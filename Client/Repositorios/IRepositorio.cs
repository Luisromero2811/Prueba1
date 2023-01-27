using System;
using Prueba1.Shared.Controllers;
namespace Prueba1.Client.Repositorios
{
	public interface IRepositorio
	{
		List<BlazorPeliculas> obtenerPeliculas();
		Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar);
		Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar);
	}
}

