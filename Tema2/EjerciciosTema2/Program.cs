using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjerciciosTema2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ruta = Path.Combine("datos", "2DAM", "alumnos.csv");
            Console.WriteLine("Nombre archivo:,"+Path.GetFileName(ruta));
            Console.WriteLine("Nombre archivo in exetension: "+Path.GetFileNameWithoutExtension(ruta));
            Console.WriteLine("Extension: "+Path.GetExtension(ruta));
            Console.WriteLine("Nombre directorio: "+Path.GetDirectoryName(ruta));
            Console.WriteLine("Cambio de extension a .json: "+Path.ChangeExtension(ruta,".json"));
            Console.WriteLine("Ruta absoluta: "+Path.GetFullPath(ruta));
            //Console.WriteLine(Directory.GetCurrentDirectory(Path.GetFullPath(ruta)));

            string[] rutas = { "foto.PNG", "notas.txt", "logo.gif", "informe.pdf", "playa.Jpg", "LEEME" };
            foreach(string s in rutas)
            {
                if (Path.GetExtension(s).Equals(".png"))
                {
                    Console.WriteLine("Imagen: "+s);
                } else if (Path.GetExtension(s).Equals(".jpg"))
                {
                    Console.WriteLine("Imagen: " + s);
                }
                else if (Path.GetExtension(s).Equals(".gif"))
                {

                    Console.WriteLine("Imagen: " + s);
                }
                else
                {
                    Console.WriteLine("No imagen: " + s);
                }
            }




        }
    }
}
