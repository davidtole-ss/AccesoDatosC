using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ej11
{
    internal class Coche:Vehiculo
    {
        private int puertas {  get; set; }

        public Coche(int puertas, string marca, string modelo, int anio) : base(marca,modelo,anio)
        {
            this.puertas = puertas;

        }
        public override void mostrarInfo()
        {
                base.mostrarInfo();
            Console.WriteLine("Numero de puertas: "+puertas);
        }
    }
}
