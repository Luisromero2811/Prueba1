using System;
using Prueba1.Shared.Controllers;
namespace Prueba1.Client.Repositorios
{
	public interface IRepositorio
	{
		List<BlazorPeliculas> obtenerPeliculas();
	}
}

