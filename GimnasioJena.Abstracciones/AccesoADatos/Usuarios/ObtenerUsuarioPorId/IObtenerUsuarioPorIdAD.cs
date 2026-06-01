using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId
{
    public interface IObtenerUsuarioPorIdAD
    {
        Task<UsuarioPerfilDto> ObtenerUsuarioPorId(string identityUserId);
    }
}
