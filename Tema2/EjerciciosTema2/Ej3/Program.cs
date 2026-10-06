using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ej3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string [] carpetas = { "docs", "src", "tests" };
            for (int i = 0; i < carpetas.Length; i++)
            {
                if (Directory.Exists(carpetas[i]) == false)
                {
                    Directory.CreateDirectory(carpetas[i]);
                    Console.WriteLine("Creada: " + carpetas[i]);
                }
                else
                {
                    Console.WriteLine("Ya existe: " + carpetas[i]);
                }
                string ruta = Path.Combine(carpetas[i], "datos.txt");
                if (File.Exists(ruta))
                {
                    Console.WriteLine("El archivo existe.");
                }
                else
                {
                    File.WriteAllText(ruta,$"Carpeta {carpetas[i]} del ProyectoDAM.");
                    Console.WriteLine("No existe");
                }

                FileInfo archivo= new FileInfo(ruta);
                Console.WriteLine($"Nombre: {archivo.Name}");
                Console.WriteLine($"Tamaño: {archivo.Length} bytes");
                DateTime fecha = archivo.CreationTime;
                Console.WriteLine($"Fecha de creacion: {fecha.Day}/{fecha.Month}/{fecha.Year} {fecha.Hour}:{fecha.Minute}");
            }

           

        }
    }
}
