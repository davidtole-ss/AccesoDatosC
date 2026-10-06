using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Alumno> alumnos = new List<Alumno>
{
                new Alumno("Ana", 9),
                new Alumno("Luis", 7),
                new Alumno("Marta", 8),
                new Alumno("Pablo", 4),
                new Alumno("Elena", 10),
                new Alumno("Jorge", 5),
                new Alumno("Sara", 6),
                new Alumno("Diego", 3)
            };

            // El serializador se crea indicando el TIPO de lo que vamos a guardar:
            // en este caso, una lista de alumnos.
            XmlSerializer serializador = new XmlSerializer(typeof(List<Alumno>));

            // Serializar
            using (StreamWriter escritor = new StreamWriter("alumnos.xml"))
            {
                serializador.Serialize(escritor, alumnos);
            }

            // Deserializar: Deserialize devuelve un object, así que hay que hacer un cast
            List<Alumno> cargados;
            using (StreamReader lector = new StreamReader("alumnos.xml"))
            {
                cargados = (List<Alumno>)serializador.Deserialize(lector);
            }

            Console.WriteLine($"Leídos {cargados.Count} alumnos del XML");   // Leídos 8 alumnos del XML







            //// Serializar (objeto -> JSON)
            // JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
            // string json = JsonSerializer.Serialize(alumnos, opciones);
            // File.WriteAllText("alumnos.json", json);

            // // Deserializar (JSON -> objeto)
            // string textoJson = File.ReadAllText("alumnos.json");
            // List<Alumno> cargados = JsonSerializer.Deserialize<List<Alumno>>(textoJson);

            // if (cargados != null)
            // {
            //     foreach (Alumno a in cargados)
            //     {
            //         Console.WriteLine($"{a.Nombre}: {a.Nota}");
            //     }
            // }
            //    // Escribir
            //    using (StreamWriter escritor = new StreamWriter("alumnos.csv"))
            //    {
            //        escritor.WriteLine("Nombre,Nota");   // cabecera
            //        foreach (Alumno a in alumnos)
            //        {
            //            escritor.WriteLine($"{a.Nombre},{a.Nota}");
            //        }
            //    }

            //    // Leer
            //    List<Alumno> leidos = new List<Alumno>();
            //    using (StreamReader lector = new StreamReader("alumnos.csv"))
            //    {
            //        string linea = lector.ReadLine();   // descarta la cabecera
            //        while ((linea = lector.ReadLine()) != null)
            //        {
            //            string[] campos = linea.Split(',');
            //            leidos.Add(new Alumno(campos[0], int.Parse(campos[1])));
            //        }
            //    }
            //    foreach (Alumno a in alumnos)
            //        Console.WriteLine(a.Nombre+";"+a.Nota);
            //    Console.WriteLine($"Leídos {leidos.Count} alumnos del CSV");   // Leídos 8 alumnos del CSV

        }
    }
}
