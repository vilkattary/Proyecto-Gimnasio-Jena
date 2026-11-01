using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales
{
    public interface IRegistrarHorariosSemanalesAD
    {
        int RegistrarHorariosSemanales(
            HorarioSemanalMultipleCrearDto horarios, DateTime fechaCreacion
        );
    }
}