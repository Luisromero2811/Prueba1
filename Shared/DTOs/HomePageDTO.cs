using System;
using Prueba1.Shared.Controllers;

namespace Prueba1.Shared.DTOs
{
	public class HomePageDTO
	{
		//Objeto que tiene como funcion transportar datos entre procesos
		public List<BlazorPeliculas>? PeliculasEnCartelera { get; set; }
		public List<BlazorPeliculas>? ProximosEstrenos { get; set; }
	}
}

