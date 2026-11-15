using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int[] temperaturas = { 18, 21, 19, 23, 20 };

            Console.WriteLine("Temperaturas de los últimos 5 días:");
            int suma = 0;
            foreach (int temperatura in temperaturas)
            {
                Console.WriteLine($"{temperatura} ºC");
                suma += temperatura;
            }

            double media = (double)suma / temperaturas.Length;
            Console.WriteLine($"Temperatura media: {media:0.00} ºC");
        }
    }
}
