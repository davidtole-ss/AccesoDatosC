using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<string> tareas = new List<string>();
            tareas.Add("Estudiar Acceso a Datos");
            tareas.Add("Hacer la práctica de Scrum");
            tareas.Add("Subir los ejercicios a GitHub");

            Console.WriteLine("Tareas pendientes:");
            int numero = 1;
            foreach (string tarea in tareas)
            {
                Console.WriteLine($"{numero}. {tarea}");
                numero++;
            }

            tareas.Remove("Hacer la práctica de Scrum");
            Console.WriteLine();
            Console.WriteLine("Hecha: Hacer la práctica de Scrum");
            Console.WriteLine($"Quedan {tareas.Count} tareas pendientes.");
        }
    }
}
