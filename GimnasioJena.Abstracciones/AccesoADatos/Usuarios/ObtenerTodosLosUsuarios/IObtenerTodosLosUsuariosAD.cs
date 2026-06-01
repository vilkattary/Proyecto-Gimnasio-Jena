using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System.Collections.Generic;


namespace GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios
{
    public interface IObtenerTodosLosUsuariosAD
    {
        List<UsuarioListadoDto> ObtenerTodosLosUsuarios();
    }
}