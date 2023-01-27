using System;
namespace Prueba1.Shared.Controllers
{
    public class PeliculaActor
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        //Propiedades de navegacion
        public BlazorPeliculas? Peliculas { get; set; }
        public Actores? Actores { get; set; }
        public string? Personaje { get; set; }
        public int Orden { get; set; }
    }
}

