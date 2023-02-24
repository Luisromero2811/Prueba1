using System;
namespace Prueba1.Shared.DTOs
{
	public class PaginacionDTO
	{
		//Con este DTO vamos a controlar la cantidad de items que se va a mostrar por elemento de paginacion
		public int Pagina { get; set; } = 1;
		public int CantidadRegistros { get; set; } = 10;
	}
}

