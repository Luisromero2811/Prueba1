using System;
using Prueba1.Shared.Controllers;
namespace Prueba1.Client.Repositorios

{
    public class Repositorio : IRepositorio
    {
        public List<BlazorPeliculas> obtenerPeliculas()
        {
            return new List<BlazorPeliculas>()
{
            new BlazorPeliculas{ Titulo = "Spider No Way Home", costo_pelicula = 200, Fecha_de_lanzamiento = new DateTime(2020,11,28)},
            new BlazorPeliculas{ Titulo = "American Me", costo_pelicula = 20, Fecha_de_lanzamiento = new DateTime(2020,11,29)},
            new BlazorPeliculas{ Titulo = "Sangre por Sangre", costo_pelicula = 2600, Fecha_de_lanzamiento = new DateTime(2020,11,27)},
        };
        }
    }
}

