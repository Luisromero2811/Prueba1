using System;
namespace Prueba1.Shared.Controllers
{
    public class PeliculaActor
    {
        public int ActorId { get; set; }
        public int PeliculasId { get; set; }
        //Propiedades de navegacion
        public BlazorPeliculas? Peliculas { get; set; }
        public Actores? Actor { get; set; }
        public string? Personaje { get; set; }
        public int Orden { get; set; }
    }
}

