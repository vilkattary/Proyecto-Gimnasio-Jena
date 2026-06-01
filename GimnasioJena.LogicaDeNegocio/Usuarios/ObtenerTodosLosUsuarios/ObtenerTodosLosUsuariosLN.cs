using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios
{
    public class ObtenerTodosLosUsuariosLN : IObtenerTodosLosUsuariosLN
    {
        private readonly IObtenerTodosLosUsuariosAD _obtenerTodosLosUsuariosAD;

        public ObtenerTodosLosUsuariosLN()
        {
            _obtenerTodosLosUsuariosAD = new ObtenerTodosLosUsuariosAD();
        }

        public List<UsuarioListadoDto> ObtenerTodosLosUsuarios()
        {
            return _obtenerTodosLosUsuariosAD.ObtenerTodosLosUsuarios();
        }
    }
}