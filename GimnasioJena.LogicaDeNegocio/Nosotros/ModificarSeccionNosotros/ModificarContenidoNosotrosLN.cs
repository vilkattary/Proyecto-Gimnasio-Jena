using GimnasioJena.Abstracciones.AccesoADatos.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Home.ModificarSeccionHome;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace GimnasioJena.LogicaDeNegocio.Nosotros.ModificarSeccionNosotros
{
    public class ModificarContenidoNosotrosLN : IModificarContenidoWebLN
    {
        private readonly IModificarContenidoWebAD _repositorio;
        private const int TamanoMaximoImagen = 5 * 1024 * 1024;

        public ModificarContenidoNosotrosLN()
        {
            _repositorio = new ModificarContenidoWebAD();
        }

        public async Task<bool> EjecutarAsync(ModificarContenidoWebDto dto)
        {
            if (dto.ArchivoImagen != null && dto.ArchivoImagen.ContentLength > 0)
            {
                string extension = Path.GetExtension(dto.ArchivoImagen.FileName).ToLowerInvariant();

                string[] extensionesPermitidas =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

                if (!extensionesPermitidas.Contains(extension))
                {
                    throw new ArgumentException(
                        "Formato no permitido. Solo se aceptan imágenes JPG, PNG y WEBP.");
                }

                if (dto.ArchivoImagen.ContentLength > TamanoMaximoImagen)
                {
                    throw new ArgumentException(
                        "La imagen supera el tamaño máximo permitido de 5 MB.");
                }
                string carpeta = HostingEnvironment.MapPath("~/images/uploads/");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = Guid.NewGuid().ToString() + extension;
                string rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

                using (var fileStream = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write))
                {
                    await dto.ArchivoImagen.InputStream.CopyToAsync(fileStream);
                }

                dto.UrlImagen = "/images/uploads/" + nombreArchivo;
            }

            return await _repositorio.ModificarContenidoWeb(dto);
        }
    }
}
