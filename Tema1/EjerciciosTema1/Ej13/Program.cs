using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<IReproducible> lista = new List<IReproducible>()
            {
                new Cancion("Bohemian Rhapsody", "Queen"),
                new Cancion("Mediterráneo", "Joan Manuel Serrat"),
                new Podcast("Código Abierto", 12)
            };

            Console.WriteLine("=== LISTA DE REPRODUCCIÓN ===");
            foreach (IReproducible elemento in lista)
            {
                elemento.Reproducir();
            }

            Console.WriteLine();
            Console.WriteLine("=== DESCARGAS ===");
            Cancion cancion = new Cancion("Bohemian Rhapsody", "Queen");
            cancion.Descargar();

            Podcast podcast = new Podcast("Código Abierto", 12);
            // podcast.Descargar();
        }
    }
}
