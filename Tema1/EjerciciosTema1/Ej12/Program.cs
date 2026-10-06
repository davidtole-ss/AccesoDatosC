using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EmpleadoFijo ef1= new EmpleadoFijo("Jorge",1800,200);
            EmpleadoPorHoras ef2= new EmpleadoPorHoras("Manuel",40,20);
            List <Empleado> list = new List<Empleado>();
            list.Add(ef1);
            list.Add(ef2);
            foreach(Empleado empleados in list)
            {
                empleados.mostrarNomina();
            }
        }
    }
}
