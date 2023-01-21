using System;
namespace Prueba1.Shared.Controllers
{
    public class BlazorPeliculas
    {
        public string Titulo { get; set; } = null!;
        public DateTime Fecha_de_lanzamiento { get; set; }
        public int costo_pelicula { get; set; }
        public string poster { get; set; } = null!;
        public string TituloCortado
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

