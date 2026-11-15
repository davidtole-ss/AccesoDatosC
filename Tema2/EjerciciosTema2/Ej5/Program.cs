using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string raiz = "instituto";
            if (Directory.Exists(raiz))
            {
                Directory.Delete(raiz, recursive: true);
            }

            Dictionary<string, int> ficheros = new Dictionary<string, int>
            {
                { Path.Combine("1DAM", "programacion.txt"),        120 },
                { Path.Combine("1DAM", "bases_datos.txt"),          80 },
                { Path.Combine("1DAM", "horario.pdf"),             300 },
                { Path.Combine("2DAM", "acceso_datos.txt"),        150 },
                { Path.Combine("2DAM", "proyectos", "tfc.pdf"),    900 },
                { Path.Combine("2DAM", "proyectos", "notas.csv"),   60 },
                { "normas.pdf",                                    200 }
            };

            foreach (KeyValuePair<string, int> par in ficheros)
            {
                string ruta = Path.Combine(raiz, par.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(ruta));
                File.WriteAllText(ruta, new string('x', par.Value));
            }

            Console.WriteLine("Subcarpetas:");
            List<string> subcarpetas = Directory
                .EnumerateDirectories(raiz, "*", SearchOption.AllDirectories)
                .OrderBy(c => c, StringComparer.Ordinal)
                .ToList();
            foreach (string carpeta in subcarpetas)
            {
                Console.WriteLine("  " + RutaRelativa(raiz, carpeta));
            }

            Console.WriteLine();
            Console.WriteLine("Ficheros:");
            List<string> rutas = Directory
                .EnumerateFiles(raiz, "*", SearchOption.AllDirectories)
                .OrderBy(r => r, StringComparer.Ordinal)
                .ToList();

            long tamanoTotal = 0;
            FileInfo masGrande = null;
            foreach (string ruta in rutas)
            {
                FileInfo info = new FileInfo(ruta);
                Console.WriteLine($"  {RutaRelativa(raiz, ruta)} ({info.Length} bytes)");

                tamanoTotal += info.Length;
                if (masGrande == null || info.Length > masGrande.Length)
                {
                    masGrande = info;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total: {rutas.Count} ficheros, {tamanoTotal} bytes");

            Console.WriteLine($"Más grande: {masGrande.Name} ({masGrande.Length} bytes)");

            int pdfs = Directory.EnumerateFiles(raiz, "*.pdf", SearchOption.AllDirectories).Count();
            Console.WriteLine($"Ficheros PDF: {pdfs}");

            Console.WriteLine();
            foreach (string carpeta in Directory.GetDirectories(raiz).OrderBy(c => c, StringComparer.Ordinal))
            {
                long tamano = Directory
                    .EnumerateFiles(carpeta, "*", SearchOption.AllDirectories)
                    .Sum(f => new FileInfo(f).Length);
                Console.WriteLine($"{Path.GetFileName(carpeta)}: {tamano} bytes");
            }
        }

        static string RutaRelativa(string raiz, string ruta)
        {
            return ruta.Substring(raiz.Length + 1);
        }
    }
}
