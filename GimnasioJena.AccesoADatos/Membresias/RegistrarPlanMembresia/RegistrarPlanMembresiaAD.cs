using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarPlanMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Entidades.Membresias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.RegistrarPlanMembresia
{
    public class RegistrarPlanMembresiaAD : IRegistrarPlanMembresiaAD
    {
        private readonly Contexto _contexto;

        public RegistrarPlanMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public int ContarPlanes()
        {
            return _contexto.PlanesMembresia.Count();
        }

        public bool RegistrarPlanMembresia(RegistrarPlanMembresiaDto modelo)
        {
            var plan = new PlanMembresiaEntidad
            {
                nombrePlan = modelo.nombrePlan,
                precio = modelo.precio,
                duracionDias = modelo.duracionDias,
                cantidadClases = modelo.cantidadClases,
                incluyeClasePrueba = false,
                estado = true,
                fechaCreacion = DateTime.UtcNow
            };

            _contexto.PlanesMembresia.Add(plan);

            _contexto.SaveChanges();

            return true;
        }
    }
}
