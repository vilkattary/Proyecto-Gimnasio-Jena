using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos
{
    public class Contexto : DbContext
    {

        public Contexto() : base("name=Contexto")
        {

        }
        
        
    }
}
