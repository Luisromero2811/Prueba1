using System;
using System.ComponentModel.DataAnnotations;

namespace Prueba1.Shared.Controllers
{
	public class Actores
	{
		public int Id { get; set; }
		[Required]
		public string Nombre { get; set; } = null!;
		public string? Biografia { get; set; }
		public string? Foto { get; set; }
		public DateTime? FechaNacimiento { get; set; }
	}
}

