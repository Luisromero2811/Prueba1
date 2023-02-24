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
        //Creamos una lista de relacion de Generos y Peliculas
        //Podemos acceder al listado de Generos mediante esta propiedad
        public List<GeneroPelicula> GenerosPelicula { get; set; } = new List<GeneroPelicula>();
        //Podemos acceder al listado de Actores mediante esta propiedad
        public List<PeliculaActor> PeliculasActor { get; set; } = new List<PeliculaActor>();
        //Accedemos a los votos de los usuarios mediante esta propiedad
        public List<VotoPelicula> VotosPeliculas { get; set; } = new List<VotoPelicula>();
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

