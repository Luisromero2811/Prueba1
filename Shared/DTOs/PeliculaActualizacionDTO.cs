using System;
using Prueba1.Shared.Controllers;
namespace Prueba1.Shared.DTOs
{
	public class PeliculaActualizacionDTO
	{
		public BlazorPeliculas Pelicula { get; set; } = null!;
		public List<Actores> Actor { get; set; } = new List<Actores>();
		public List<Genero> GenerosSeleccionados { get; set; } = new List<Genero>();
		public List<Genero> GeneroNoSeleccionado { get; set; } = new List<Genero>();
	}
}

