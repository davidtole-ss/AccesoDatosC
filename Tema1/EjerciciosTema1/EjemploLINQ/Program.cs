using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Alumno> alumnos = new List<Alumno>{
                new Alumno("Ana",   "DAM",  9, 20),
                new Alumno("Luis",  "DAW",  4, 22),
                new Alumno("Marta", "DAM",  7, 19),
                new Alumno("Pablo", "ASIR", 3, 21),
                new Alumno("Lucía", "DAW",  8, 23),
                new Alumno("Jorge", "DAM",  5, 20)
            };

            //List<Alumno> aprobados = alumnos.Where(a => a.Nota >= 5).ToList();
            //foreach (Alumno a in aprobados) {
            //    Console.WriteLine(a);
            //}

            //var edad = alumnos.Where(a => a.Edad >= 22);
            //foreach (Alumno a in edad) {
            //    Console.WriteLine(a);
            //}


            //List<int> aprobados = alumnos.Select(a =>a.Nota).ToList();
            //foreach (int a in aprobados)
            //{
            //    Console.WriteLine(a);
            //}

            var notas = alumnos.Select(a => a.Nota).OrderBy(n => n).ToList();
            foreach (var n in notas)
            {
                Console.WriteLine(n);
            }

        }
    }
}
