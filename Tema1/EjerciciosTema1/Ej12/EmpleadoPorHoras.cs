using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej12
{
    internal class EmpleadoPorHoras :Empleado
    {
        private int horas {  get; set; }
        private  decimal precioHoras{ get; set; }

        public EmpleadoPorHoras(string nombre,int horas, decimal precioHoras) : base(nombre)
          
        {
            this.horas = horas;
            this.precioHoras = precioHoras;

        }
        public override decimal calcularSueldo()
        {
            return horas * precioHoras;
        }
    }
}
