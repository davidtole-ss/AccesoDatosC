using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Edad 15: {EsMayorDeEdad(15)}");
            Console.WriteLine($"Edad 18: {EsMayorDeEdad(18)}");
            Console.WriteLine($"Edad 30: {EsMayorDeEdad(30)}");
        }

        static bool EsMayorDeEdad(int edad)
        {
            return edad >= 18;
        }
    }
}
