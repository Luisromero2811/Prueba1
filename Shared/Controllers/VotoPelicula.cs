using System;
namespace Prueba1.Shared.Controllers
{
	public class VotoPelicula
	{
		public int Id { get; set; }
		public int Voto { get; set; }
		public DateTime FechaVoto { get; set; }
		public int PeliculaId { get; set; }
		public BlazorPeliculas? Pelicula { get; set; }
	}
}

