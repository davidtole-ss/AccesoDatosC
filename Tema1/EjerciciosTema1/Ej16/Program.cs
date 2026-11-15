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
            Console.OutputEncoding = Encoding.UTF8;

            List<Producto> productos = new List<Producto>()
            {
                new Producto("Teclado", "Periféricos", 25.99m),
                new Producto("Monitor", "Pantallas", 149.00m),
                new Producto("Ratón", "Periféricos", 12.50m),
                new Producto("Portátil", "Ordenadores", 799.00m),
                new Producto("Webcam", "Periféricos", 39.90m),
                new Producto("Tablet", "Ordenadores", 229.00m)
            };

            Console.WriteLine("--- Por precio ---");
            List<Producto> porPrecio = productos.OrderBy(p => p.Precio).ToList();
            foreach (Producto p in porPrecio)
            {
                Console.WriteLine(p);
            }

            Producto productoMasCaro = productos.OrderByDescending(p => p.Precio).FirstOrDefault();
            Console.WriteLine();
            Console.WriteLine($"El más caro: {productoMasCaro.Nombre} ({productoMasCaro.Precio:0.00} €)");

            Console.WriteLine();
            Console.WriteLine("--- Por categoría y nombre ---");
            List<Producto> porCategoria = productos
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .ToList();
            foreach (Producto p in porCategoria)
            {
                Console.WriteLine($"{p.Categoria} - {p.Nombre}");
            }

            Producto impresora = productos.FirstOrDefault(p => p.Categoria == "Impresoras");
            Console.WriteLine();
            if (impresora == null)
            {
                Console.WriteLine("No hay impresoras en el catálogo.");
            }
            else
            {
                Console.WriteLine($"Primera impresora: {impresora.Nombre}");
            }
        }
    }
}
