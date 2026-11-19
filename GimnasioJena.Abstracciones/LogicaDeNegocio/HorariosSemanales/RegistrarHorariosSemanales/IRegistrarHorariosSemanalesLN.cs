using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales
{
    public interface IRegistrarHorariosSemanalesLN
    {
        ResultadoRegistroHorariosDto RegistrarHorariosSemanales(
            HorarioSemanalMultipleCrearDto horarios 
        );
    }
}