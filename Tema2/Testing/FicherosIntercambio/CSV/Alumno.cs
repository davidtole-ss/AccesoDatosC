using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV
{
    internal class Alumno
    {
  
            // Propiedades con get y set: los serializadores de JSON y XML necesitan
            // poder LEER y ESCRIBIR cada propiedad.
            public string Nombre { get; set; } = "";
            public int Nota { get; set; }

            // Constructor sin parámetros: al leer el fichero, el serializador crea
            // primero un objeto vacío y después rellena sus propiedades una a una.
            public Alumno() { }

            // Constructor con parámetros: para crear alumnos cómodamente en nuestro código.
            public Alumno(string nombre, int nota)
            {
                Nombre = nombre;
                Nota = nota;
            }
        }
    }

