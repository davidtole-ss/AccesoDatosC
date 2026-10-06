using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej12
{
    internal class EmpleadoFijo:Empleado
    {
        private decimal sueldoBase;
        private decimal complemento;

        public EmpleadoFijo(string nombre,decimal sueldoBase,decimal complemento) : base(nombre)
        {
            this.sueldoBase = sueldoBase;
            this.complemento = complemento;
        }
        public override decimal calcularSueldo()
        {
            return sueldoBase + complemento;
        }
    }
}
