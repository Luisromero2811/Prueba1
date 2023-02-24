using System;
using Prueba1.Shared.Controllers;
namespace Prueba1.Shared.DTOs

{
	public class PeliculaVisualizarDTO
	{
		public BlazorPeliculas Pelicula { get; set; } = null!;
		public List<Genero> Generos { get; set; } = new List<Genero>();
		public List<Actores> Actores { get; set; } = new List<Actores>();
		public int VotoUsuario { get; set; }
		public double PromedioVotos { get; set; }
	}
}

