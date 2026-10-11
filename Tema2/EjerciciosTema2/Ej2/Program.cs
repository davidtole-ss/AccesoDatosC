using System;
using System.IO;
using System.Text;

namespace Ej2
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("--- Parte A ---");

            using (RecursoDemo bloque = new RecursoDemo("Bloque"))
            {
                bloque.Usar();
            }
            Console.WriteLine("Después del bloque using");
            Console.WriteLine();

            
            try
            {
                using (RecursoDemo excepcion = new RecursoDemo("Excepción"))
                {
                    excepcion.Usar();
                    throw new InvalidOperationException("Fallo a propósito");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Capturada: " + ex.Message);
            }
            Console.WriteLine();

         
            Console.WriteLine("--- Parte B ---");
            string ruta = "diario.txt";

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine("Lunes: empezamos el Tema 2");
                sw.WriteLine("Martes: practicamos con System.IO");
                sw.WriteLine("Miércoles: ya domino using");
            }

            using (StreamReader sr = new StreamReader(ruta))
            {
                Console.WriteLine("Contenido de diario.txt:");
                Console.Write(sr.ReadToEnd());
            }

            
        }
    }
}
