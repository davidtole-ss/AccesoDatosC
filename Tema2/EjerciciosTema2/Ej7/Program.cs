using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Ej7
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //// Serializar (objeto -> JSON)
            //JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
            //string json = JsonSerializer.Serialize(alumnos, opciones);
            //File.WriteAllText("alumnos.json", json);


            if (File.Exists("coches.json"))
            {
                using (FileStream fs = File.OpenRead("coches.json"))
                {
                    List<Coche> cargados = JsonSerializer.Deserialize<List<Coche>>(fs);
                    Console.WriteLine($"{cargados.Count} leidos");
                    var cocheMasCaro = cargados.OrderByDescending(c => c.Precio).FirstOrDefault();
                    Console.WriteLine("Coches mas caro: "+cocheMasCaro);
                    var coches2020MasMediaKm = cargados.Where(f => f.Anio >= 2020).GroupBy(a=>a.Anio).Select( c=>new {Año=c.Key,CantidadCoches=c.Count()});


                     
                }
            }
            else
            {
                Console.WriteLine("No existe el archivo.");
            }
        }
    }
}
