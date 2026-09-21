using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace PrimeraApp_Tema1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int ? x = null;
            //int n = x ?? 12;
            //Console.WriteLine(x);
            //Console.WriteLine(n);


            //Console.Write("Introduce tu edad: ");
            //string? entrada = Console.ReadLine();

            //if (int.TryParse(entrada, out int edad))
            //{
            //    Console.WriteLine($"El año que viene tendrás {edad + 1} años.");
            //}
            //else
            //{
            //    Console.WriteLine("Eso no es un número válido.");
            //}


            int nota = 4;
            string resultado = (nota >= 5) ? "Aprobado" : "Suspenso";
            Console.WriteLine(resultado);

            
        }
    }
}
