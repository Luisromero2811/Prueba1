using System;

namespace Prueba1.Server.Helpers
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;
        //Constructor para gestion de archivos locales 
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            //Nombre del archivo del cual lo obtendremos de la ruta
            var nombreArchivo = Path.GetFileName(ruta);
            //Generar directorio
            var directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor, nombreArchivo);
            //Verificamos si existe un archivo en ese lugar
            if(File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }
            return Task.CompletedTask;
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            //Nombre unico de archivo
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            //Nombre de carpeta donde se almacenará el archivo
            var folder = Path.Combine(env.WebRootPath, nombreContenedor);
            //Sino existe el directorio lo va a crear
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //Ruta del archivo
            string rutaGuardado = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(rutaGuardado, contenido);

            //Generación de URL para alojamiento de archivos
            var urlActual = $"{httpContextAccessor!.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            //Ruta donde se almacenará el archivo en la base de datos
            var rutaParaBD = Path.Combine(urlActual, nombreContenedor, nombreArchivo).Replace("\\", "/");
            return rutaParaBD;
        }
    }
}

