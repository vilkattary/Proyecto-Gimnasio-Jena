using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Usuarios.CambiarRolUsuario
{
    public interface ICambiarRolUsuarioAD
    {
        bool CambiarRolUsuario(UsuarioCambiarRolDto usuarioCambiarRol);
    }
}