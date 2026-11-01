using GimnasioJena.Abstracciones.General.Fechas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.General.Fechas
{
    public class FechasLN : IFechasLN
    {
        public DateTime ObtenerFechaActual()
        {
            string configuracion =
                ConfigurationManager.AppSettings["zonaHoraria"];

            int zonaHoraria;

            if (!int.TryParse(configuracion, out zonaHoraria))
            {
                zonaHoraria = -6;
            }

            return DateTime.UtcNow.AddHours(zonaHoraria);
        }
    }
}
