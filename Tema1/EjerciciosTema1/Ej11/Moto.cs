using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej11
{
    internal class Moto:Vehiculo
    {
        private int cc;

        public Moto(int cc,string marca,string modelo,int anio) : base(marca,modelo,anio)
        {
            this.cc = cc;
        }

        public override void mostrarInfo()
        {
            base.mostrarInfo();
            Console.WriteLine("Cilindrada: "+cc);
        }
    }
}
