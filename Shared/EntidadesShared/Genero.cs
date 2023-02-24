using System;
using System.ComponentModel.DataAnnotations;
namespace Prueba1.Shared.Controllers
{
	public class Genero
	{
		public int Id { get; set; }
		[Required (ErrorMessage ="El campo {0} es requerido")]
		public string Nombre { get; set; } = null!;
        //Podemos acceder al listado de Peliculas mediante esta propiedad
        public List<GeneroPelicula> GenerosPelicula { get; set; } = new List<GeneroPelicula>();
    }
}

