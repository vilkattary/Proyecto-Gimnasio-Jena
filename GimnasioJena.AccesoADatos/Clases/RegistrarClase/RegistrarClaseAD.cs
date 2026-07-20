using GimnasioJena.Abstracciones.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.RegistrarClase
{
    public class RegistrarClaseAD : IRegistrarClaseAD
    {
        private Contexto _elContexto;

        public RegistrarClaseAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarClase(ClaseCrearDto laClase)
        {
            if (laClase.Horarios != null && laClase.Horarios.Any())
                return RegistrarConHorarios(laClase);

            return RegistrarSimple(laClase);
        }

        private int RegistrarConHorarios(ClaseCrearDto laClase)
        {
            using (var transaccion = _elContexto.Database.BeginTransaction())
            {
                try
                {
                    var horarioClase = new HorarioClaseEntidad
                    {
                        idTipoClase   = laClase.idTipoClase,
                        idEstadoClase = laClase.idEstadoClase,
                        cupoMaximo    = laClase.cupoMaximo,
                        observaciones = laClase.observaciones,
                        fechaCreacion = laClase.fechaCreacion
                    };

                    _elContexto.HorariosClase.Add(horarioClase);
                    _elContexto.SaveChanges();

                    int total = 0;
                    foreach (var h in laClase.Horarios)
                    {
                        var clase = new ClaseEntidad
                        {
                            idHorarioClase      = horarioClase.idHorarioClase,
                            idTipoClase         = laClase.idTipoClase,
                            idUsuarioEntrenador = h.idUsuarioEntrenador,
                            idEstadoClase       = laClase.idEstadoClase,
                            fechaClase          = DateTime.Parse(h.fechaClase),
                            horaInicio          = TimeSpan.Parse(h.horaInicio),
                            horaFin             = TimeSpan.Parse(h.horaFin),
                            cupoMaximo          = laClase.cupoMaximo,
                            observaciones       = laClase.observaciones,
                            fechaCreacion       = laClase.fechaCreacion
                        };

                        _elContexto.Clases.Add(clase);
                        total += _elContexto.SaveChanges();
                    }

                    transaccion.Commit();
                    return total;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        private int RegistrarSimple(ClaseCrearDto laClase)
        {
            _elContexto.Clases.Add(new ClaseEntidad
            {
                idTipoClase         = laClase.idTipoClase,
                idUsuarioEntrenador = laClase.idUsuarioEntrenador,
                idEstadoClase       = laClase.idEstadoClase,
                fechaClase          = laClase.fechaClase,
                horaInicio          = laClase.horaInicio,
                horaFin             = laClase.horaFin,
                cupoMaximo          = laClase.cupoMaximo,
                ubicacion           = laClase.ubicacion,
                observaciones       = laClase.observaciones,
                fechaCreacion       = laClase.fechaCreacion
            });
            return _elContexto.SaveChanges();
        }
    }
}
