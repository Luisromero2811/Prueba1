using System;
namespace Prueba1.Shared.Controllers
{
	//Clase dedicada para modelar una relacion muchos a muchos entre genero y pelicula, dando a entender que una pelicula pertenece a varios generos y 1 genero pertenece a varias peliculas
	public class GeneroPelicula
	{
		//Modelamos que genero y pelicula se puede relacionar
		public int PeliculaId { get; set; }
		public int GeneroId { get; set; }
	}
}

