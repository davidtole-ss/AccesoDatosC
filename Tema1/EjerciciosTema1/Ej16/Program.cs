using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Producto> productos = new List<Producto>()
            {
                new Producto("Teclado","Perifericos",25.99m),
                new Producto("Monitor", "Pantallas", 149.00m),
                new Producto("Raton", "Perifericos", 12.50m),
                new Producto("Portatil", "Ordenadores", 799.00m),
                new Producto("WebCam", "Perifericos", 39.90m),
                new Producto("Tablet", "Ordenadores", 229.00m)
            };

            Producto productoMasCaro = productos.OrderBy(p=>p.precio).FirstOrDefault();
            Console.WriteLine(productoMasCaro);

                

        }
    }
}
