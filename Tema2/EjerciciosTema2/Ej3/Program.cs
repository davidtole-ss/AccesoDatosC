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
                string rutaProyecto = Path.Combine("ProyectoDAM", carpetas[i]);

                if (Directory.Exists(rutaProyecto) == false)
                {
                    Directory.CreateDirectory(rutaProyecto);
                    Console.WriteLine("Creada: " + rutaProyecto);
                }
                else
                {
                    Console.WriteLine("Ya existía: " + rutaProyecto);
                }
                string ruta = Path.Combine(rutaProyecto, "LEEME.txt");
                if (File.Exists(ruta))
                {
                    Console.WriteLine("El archivo existe: "+ruta);
                }
                else
                {
                    File.WriteAllText(ruta, $"Carpeta {carpetas[i]} del ProyectoDAM");
                    Console.WriteLine("No existe, creando: " +ruta);
                }

                FileInfo archivo= new FileInfo(ruta);
                Console.WriteLine($"Nombre: {archivo.Name}");
                Console.WriteLine($"Tamaño: {archivo.Length} bytes");
                DateTime fecha = archivo.CreationTime;
                Console.WriteLine($"Fecha de creacion: {fecha: dd/MM/yyyy HH:mm}");
                Console.WriteLine();
            }

            Boolean existeCarpeta = File.Exists("ProyectoDAM\\docs");
            Boolean existeCarpeta2 = Directory.Exists("ProyectoDAM\\docs");
            Boolean existeArchivo = File.Exists("ProyectoDAM\\docs\\LEEME.txt");
            Boolean existeArchivo2 = Directory.Exists("ProyectoDAM\\docs\\LEEME.txt");
            Console.WriteLine($"ProyectoDAM\\docs -> File.Exists: {existeCarpeta}  Directory.Exists: {existeCarpeta2}");
            Console.WriteLine($"ProyectoDAM\\docs\\LEEME.txt -> File.Exists: {existeArchivo}  Directory.Exists: {existeArchivo2}");



        }
    }
}
