using GimnasioJena.Abstracciones.AccesoADatos.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Home.ModificarSeccionHome;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace GimnasioJena.LogicaDeNegocio.Home.ModificarSeccionHome
{
    public class ModificarContenidoWebLN : IModificarContenidoWebLN
    {
        private readonly IModificarContenidoWebAD _repositorio;

        public ModificarContenidoWebLN()
        {
            _repositorio = new ModificarContenidoWebAD();
        }

        public async Task<bool> EjecutarAsync(ModificarContenidoWebDto dto)
        {
            if (dto.ArchivoImagen != null && dto.ArchivoImagen.ContentLength > 0)
            {
                string carpeta = HostingEnvironment.MapPath("~/images/uploads/");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string extension     = Path.GetExtension(dto.ArchivoImagen.FileName);
                string nombreArchivo = Guid.NewGuid().ToString() + extension;
                string rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

                using (var fileStream = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write))
                {
                    await dto.ArchivoImagen.InputStream.CopyToAsync(fileStream);
                }

                // Ruta relativa virtual — se resuelve con Url.Content en la vista
                dto.UrlImagen = "/images/uploads/" + nombreArchivo;
            }

            return await _repositorio.ModificarContenidoWeb(dto);
        }
    }
}
