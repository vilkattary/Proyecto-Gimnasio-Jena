using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System.Collections.Generic;


namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios
{
    public interface IObtenerTodosLosUsuariosLN
    {
        List<UsuarioListadoDto> ObtenerTodosLosUsuarios();
    }
}