using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba1.Shared.Controllers;

namespace Prueba1.Server.Controllers
{
    //Ruta de acceso para consumo de webAPI
    [Route("api/generos")]
    //Parametro para indicar que el controlador debe funcionar como una API
    [ApiController]
    //A traves de estas clases de controllers, podemos a traves del client mandar peticiones HTTP que a su vez esta misma clase las va a recibir peticiones HTTP para poder interactuar con la Base de datos
    //Proteger los controladores, si estas auth puedes usarlos, de lo contrario dará error
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class GenerosControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;

        //Constructor para instaciar ApplicationDBContext para así poder ingresar generos a nuestra base
        public GenerosControllers(ApplicationDbContext context)
        {
            this.context = context;
        }
        //Peticion a traves de method post para ingresar un nuevo genero
        [HttpPost]      //Mensaje gen //Si queremos retornar info //Clase //Tabla
        public async Task<ActionResult<int>> Post(Genero genero)
        {
            //Le pasamos el contexto solo para marcar que le mandamos un genero a la base
            //Marcamos el objeto para ser guardado en la DB
            context.Add(genero);
            //Aqui realmente es donde se inserta el genero directo a la base de datos
            await context.SaveChangesAsync();
            return genero.Id;
        }
        //Peticion a traves de method GET para obtencion de datos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }
        //Endpoint para editar un genero
        //Tenemos que indicar que estamos recibiendo una variable en la ruta
        [HttpGet("{id:int}")]
        //Estamos recibiendo un genero 
        public async Task<ActionResult<Genero>> Get(int id)
        {
            //En caso de que no exista el genero returnamos notfound en caso de que exista lo retornamos
            var genero = await context.Generos.FirstOrDefaultAsync(genero => genero.Id == id);
            if(genero is null)
            {
                return NotFound();
            }
            return genero;
        }
        [HttpPut]
        public async Task<ActionResult>Put(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult>Delete(int id)
        {
            //Entramos a la tabla de generos, donde buscamos por ID del genero el cual lo asignamos para borrarlos
            var filasAfectadas = await context.Generos
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            if(filasAfectadas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
//Con HTTPClient podemos consultar el status del method en el controller
//Centralizar la logica de hacer una peticion en una clase de repositorio
//Por lo cual se van a centralizar las respuestas de la webAPI para obtener status de funciones

