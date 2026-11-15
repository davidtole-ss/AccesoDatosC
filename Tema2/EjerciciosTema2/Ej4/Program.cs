using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string descargas = "descargas";

            if (Directory.Exists(descargas))
            {
                Directory.Delete(descargas, recursive: true);   // borra la carpeta y todo su contenido
            }
            Directory.CreateDirectory(descargas);

            string[] ficherosPrueba = { "apuntes.pdf", "foto1.jpg", "practica.zip",
                            "foto2.jpg", "temp1.tmp", "examen.pdf", "temp2.tmp" };
            foreach (string nombre in ficherosPrueba)
            {
                File.WriteAllText(Path.Combine(descargas, nombre), $"Contenido de {nombre}");
            }
            string[] ficheros = Directory.GetFiles(descargas);
            Array.Sort(ficheros);
            Console.WriteLine($"Ficheros en descargas: {ficheros.Length}");

            foreach (string ruta in ficheros)
            {
                if (Path.GetExtension(ruta) == ".tmp")
                {
                    File.Delete(ruta);
                    Console.WriteLine($"Eliminado: {Path.GetFileName(ruta)}");
                }
            }

            ficheros = Directory.GetFiles(descargas);
            Array.Sort(ficheros);
            foreach (string ruta in ficheros)
            {
                string nombre = Path.GetFileName(ruta);
                string extension = Path.GetExtension(ruta);

                string subcarpeta = Path.Combine(descargas, extension.TrimStart('.'));
                Directory.CreateDirectory(subcarpeta);
                File.Move(ruta, Path.Combine(subcarpeta, nombre));      
                Console.WriteLine($"Movido: {nombre} -> {extension.TrimStart('.')}");
            }

            //4. Copia de seguridad (después del bucle)
            string copiaPdf = "copia_pdf";
            Directory.CreateDirectory(copiaPdf);
            string[] pdfs = Directory.GetFiles(Path.Combine(descargas, "pdf"));
            foreach (string pdf in pdfs)
            {
                File.Copy(pdf, Path.Combine(copiaPdf, Path.GetFileName(pdf)), true);
            }
            Console.WriteLine($"Copia de seguridad: {pdfs.Length} fichero(s) en {copiaPdf}");

            //5. Recuento
            Console.WriteLine($"Ficheros sueltos en descargas: {Directory.GetFiles(descargas).Length}");
            foreach (string subCarpeta in Directory.GetDirectories(descargas))
            {
                Console.WriteLine($"{subCarpeta}: {Directory.GetFiles(subCarpeta).Length} fichero(s)");
            }

            //6. Borrar sin recursive
            try
            {
                Directory.Delete(descargas);
            }
            catch (Exception)
            {
                Console.WriteLine("No se puede borrar 'descargas' sin 'recursive: true' (no está vacía).");
            }

        }
    }
}
