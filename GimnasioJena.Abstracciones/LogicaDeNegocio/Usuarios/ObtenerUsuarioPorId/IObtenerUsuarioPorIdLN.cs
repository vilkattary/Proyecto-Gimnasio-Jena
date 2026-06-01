using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId
{
    public interface IObtenerUsuarioPorIdLN
    {
        Task<UsuarioPerfilDto> ObtenerUsuarioPorId(string identityUserId);
    }
}
