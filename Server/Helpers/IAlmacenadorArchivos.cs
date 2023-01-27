using System;
namespace Prueba1.Server.Helpers
{
	//La interface sera usada como un Servicio para almacenar archivos
	public interface IAlmacenadorArchivos
	{
		//Devolverá un string el cual contendra la URL del archivo
		//Tendra como properties un array de bytes, la extension del archivo .jpeg y el nombre es una referencia a un folder en Azure
		Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);
		Task EliminarArchivo(string ruta, string nombreContenedor);
		async Task<string> EditarArchivo(byte[] contenido, string extension,
			string nombreContendor, string ruta)
		//A traves de las interface actualizadas de .NET podemos ahorrar code de servicios
		//En este caso aqui mismo haremos las condiciones pertinentes para actualizar un archivo nuevo
		//Haremos una implementacion por defecto
		{
            if (ruta is not null)
            {
			await EliminarArchivo(ruta, nombreContendor);
            }
			return await GuardarArchivo(contenido, extension, nombreContendor); 
        }
	}
}

