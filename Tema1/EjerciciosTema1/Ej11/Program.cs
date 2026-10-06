using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Coche c1 = new Coche(4,"Audi","q7",2020);
            Moto m1 = new Moto(125,"Vespa","3",2011);
            c1.mostrarInfo();
            m1.mostrarInfo();

        }
    }
}
