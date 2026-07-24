using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using System;

namespace GimnasioJena.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales
{
    public class RegistrarHorariosSemanalesAD
        : IRegistrarHorariosSemanalesAD
    {
        private readonly Contexto _elContexto;

        public RegistrarHorariosSemanalesAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarHorariosSemanales(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            using (var transaccion =
                _elContexto.Database.BeginTransaction())
            {
                try
                {
                    int cantidadRegistrada = 0;
                    DateTime fechaActual = DateTime.Now;

                    foreach (var detalle in modelo.horarios)
                    {
                        HorarioSemanalEntidad entidad =
                            new HorarioSemanalEntidad
                            {
                                idTipoClase =
                                    modelo.idTipoClase,

                                idUsuarioEntrenador =
                                    modelo.idUsuarioEntrenador,

                                diaSemana =
                                    modelo.diaSemana,

                                horaInicio =
                                    detalle.horaInicio,

                                horaFin =
                                    detalle.horaFin,

                                cupoMaximo =
                                    modelo.cupoMaximo,

                                ubicacion =
                                    modelo.ubicacion.Trim(),

                                estado = true,

                                fechaCreacion =
                                    fechaActual,

                                fechaModificacion =
                                    null
                            };

                        _elContexto.HorariosSemanales.Add(entidad);

                        cantidadRegistrada++;
                    }

                    _elContexto.SaveChanges();

                    transaccion.Commit();

                    return cantidadRegistrada;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    throw new Exception(
                        "No fue posible registrar los horarios semanales. " +
                        ex.Message,
                        ex
                    );
                }
            }
        }
    }
}