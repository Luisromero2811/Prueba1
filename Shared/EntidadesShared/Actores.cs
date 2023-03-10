using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba1.Shared.Controllers
{
	public class Actores
	{
		public int Id { get; set; }
		[Required]
		public string Nombre { get; set; } = null!;
		public string? Biografia { get; set; }
		public string? Foto { get; set; }
		//El elemento NotMapped hace referencia a una propiedad la cual no será cargada como campo de la tabla Actores, no será mappeado, el uso va depender de la funcionalidad. 
		[NotMapped]
		public string? Personaje { get; set; }
		public DateTime? FechaNacimiento { get; set; }

		//Méthod para comparar a los actores 
		//A partir de este method partimos de comparar a los actores, si la comparación no es la misma se retorna un false, en caso de que sea la misma esta retornará el ID del actor
		//
		public override bool Equals(object? obj)
        {
			//Si el objeto que recibimos es igual a un actor, entonces indicamos que dos actores son iguales si solamente coinciden con su mismo ID.
			if(obj is Actores a2)
			{
				return Id == a2.Id;
			}
			//Sino devuelve un falso
			return false;
        }
		//Función obligatoria para usar el override de Equals, se pide hacer lo mismo con GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //Podemos acceder al listado de Actores mediante esta propiedad
        public List<PeliculaActor> PeliculasActor { get; set; } = new List<PeliculaActor>();
    }
}

