using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.CambiarRolUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.CambiarRolUsuario;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.AccesoADatos.Usuarios.CambiarRolUsuario;

namespace GimnasioJena.LogicaDeNegocio.Usuarios.CambiarRolUsuario
{
    public class CambiarRolUsuarioLN : ICambiarRolUsuarioLN
    {
        private readonly ICambiarRolUsuarioAD _cambiarRolUsuarioAD;

        public CambiarRolUsuarioLN()
        {
            _cambiarRolUsuarioAD = new CambiarRolUsuarioAD();
        }

        public bool CambiarRolUsuario(UsuarioCambiarRolDto usuarioCambiarRol)
        {
            if (usuarioCambiarRol == null)
                return false;

            if (string.IsNullOrWhiteSpace(usuarioCambiarRol.identityUserId))
                return false;

            if (string.IsNullOrWhiteSpace(usuarioCambiarRol.rolNuevo))
                return false;

            return _cambiarRolUsuarioAD.CambiarRolUsuario(usuarioCambiarRol);
        }
    }
}