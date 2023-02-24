using System;
namespace Prueba1.Shared.DTOs
{
	public class ParametrosBusquedaPelicuaDTO
	{
		//Traemos los elementos de paginacion para seguir haciendo uso del componente durante el filtrado
		public int Pagina { get; set; } = 1;
		public int CantidadRegistros { get; set; } = 10;
		public PaginacionDTO PaginacionDTO
		{
			get
			{
				return new PaginacionDTO { Pagina = Pagina, CantidadRegistros = CantidadRegistros };
			}
		}
		public string? Titulo { get; set; }
		public int GeneroId { get; set; }
		public bool EnCartelera { get; set; }
		public bool Estrenos { get; set; }
		public bool MasVotadas { get; set; }
	}
}

