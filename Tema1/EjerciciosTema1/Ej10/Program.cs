using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Saludar("Ana");
            Saludar("John", "en");
            Saludar(idioma: "es", nombre: "Luis");
        }

        static void Saludar(string nombre, string idioma = "es")
        {
            if (idioma == "en")
            {
                Console.WriteLine($"Hello, {nombre}!");
            }
            else
            {
                Console.WriteLine($"Hola, {nombre}!");
            }
        }
    }
}
