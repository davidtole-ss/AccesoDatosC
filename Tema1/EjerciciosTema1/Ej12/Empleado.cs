using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej12
{
    internal abstract class Empleado
    {
        private string nombre {  get; set; }

        public Empleado(string nombre) {  this.nombre = nombre; }

        public abstract decimal calcularSueldo();
        public void mostrarNomina()
        {
            Console.WriteLine("Nombre empleado: "+nombre);
            Console.WriteLine(calcularSueldo());
        }
    }
}
