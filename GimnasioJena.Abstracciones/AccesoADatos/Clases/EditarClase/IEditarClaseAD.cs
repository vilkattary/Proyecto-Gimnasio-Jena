using GimnasioJena.Abstracciones.Modelos.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Clases.EditarClase
{
    public interface IEditarClaseAD
    {
        int EditarClases(ClaseEditarDto ClaseAEditar);
    }
}