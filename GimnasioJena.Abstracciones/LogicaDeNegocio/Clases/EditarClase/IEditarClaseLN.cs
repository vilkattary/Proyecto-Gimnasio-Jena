using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase
{
    public interface IEditarClaseLN
    {
        bool EditarClase(ClaseEditarDto claseAEditar);
    }
}