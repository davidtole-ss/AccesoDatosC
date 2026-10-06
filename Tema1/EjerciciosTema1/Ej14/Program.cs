using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numeros = new List<int> { 12, 7, 25, 3, 18, 40, 9, 16 };
            var pares=numeros.Where(x => x%2==0);
            var mayor10 = numeros.Where(x => x>10);
            var entre10Y20 = numeros.Where(x => x>=10&&x<=20);
            Console.WriteLine("Pares: "+string.Join(",",pares));
            Console.WriteLine("Mayores que 10:" + string.Join(",", mayor10));
            Console.WriteLine("Entre 10 y 20: " + string.Join(",", entre10Y20));
            var impares = numeros.Where(x => x % 2 != 0).Count();
            Console.WriteLine("Numero impares: "+impares);


        }
    }
}
