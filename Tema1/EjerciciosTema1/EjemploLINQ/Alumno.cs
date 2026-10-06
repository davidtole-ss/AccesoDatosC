using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploLINQ
{
    public class Alumno
    {
        public string Nombre { get; }
        public string Ciclo { get; }
        public int Nota { get; }
        public int Edad { get; }

        public Alumno(string nombre, string ciclo, int nota, int edad)
        {
            Nombre = nombre;
            Ciclo = ciclo;
            Nota = nota;
            Edad = edad;
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Ciclo: {Ciclo}, Nota: {Nota}, Edad: {Edad}";
        }
        
    }
}
