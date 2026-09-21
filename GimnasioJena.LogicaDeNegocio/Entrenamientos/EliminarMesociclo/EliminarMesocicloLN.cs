using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.EliminarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.EliminarMesociclo;
using GimnasioJena.AccesoADatos.Entrenamientos.EliminarMesociclo;
using System;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.EliminarMesociclo
{
    public class EliminarMesocicloLN : IEliminarMesocicloLN
    {
        private readonly IEliminarMesocicloAD _eliminarMesocicloAD;

        public EliminarMesocicloLN()
        {
            _eliminarMesocicloAD = new EliminarMesocicloAD();
        }

        public bool EliminarMesociclo(int idMesociclo)
        {
            if (idMesociclo <= 0)
            {
                throw new ArgumentException("Debe indicar el mesociclo a eliminar.");
            }

            return _eliminarMesocicloAD.EliminarMesociclo(idMesociclo);
        }
    }
}
