using System;
namespace Prueba1.Shared.Controllers
{
    public class BlazorPeliculas
    { 
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Resumen { get; set; }
        public bool EnCartelera { get; set; }
        public string? Trailer { get; set; }
        public DateTime? Fecha_de_lanzamiento { get; set; }
        public int costo_pelicula { get; set; }
        public string? poster { get; set; } 
        public string? TituloCortado
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Titulo))
                {
                    return null;
                }
                if(Titulo.Length > 60)
                {
                    return Titulo.Substring(0, 60) + "...";
                }
                else
                {
                    return Titulo;
                }
            }
        }

    }
}

