using System;
using Microsoft.EntityFrameworkCore;
using Prueba1.Shared.Controllers;

namespace Prueba1.Server
{
	public class ApplicationDbContext : DbContext 
	{
        //Para esta clase general se usará explicitamente para guardar el ambiente o entorno, algo parecido al .env de Laravel
        //Con DbContextOptions se pasarán las configuraciones iniciales, ejemplo la conexión a nuestra BD
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        //Api Fluente, tener acceso a todas las configuraciones de EntityFrameworkCore
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configurar la entidad con modelbuilder.Entity
            //A través de esta entidad podemos aplicar la llave compartida para generar el campo primario de la relacion muchos a muchos
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PeliculaId, x.ActorId });
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actores> Actores => Set<Actores>();
        public DbSet<BlazorPeliculas> Peliculas => Set<BlazorPeliculas>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();

    }
}

//Que clases de la app van a representar tablas
//A partir del set se crea un DbSet, por lo cual va a referenciar a la clase y se podrá crear la tabla con sus propiedades