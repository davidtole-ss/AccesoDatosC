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
            //JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };>>
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
                    var coches2020MasMediaKm = cargados.
                        Where(f => f.Anio >= 2020)
                        .GroupBy(a=>a.Anio)
                        .Select( c=>new {Año=c.Key,CantidadCoches=c.Count(),MediaKM=c.Average(a=>a.Kilometros)});
                    foreach (var c in coches2020MasMediaKm)
                    {
                        Console.WriteLine($"{c.Año}: {c.CantidadCoches} coches, media {c.MediaKM:N0} km");
                    }
                    Console.WriteLine();


                    List<Coche> toyota=cargados.Where(s=>s.Marca.ToLower()=="toyota").ToList();
                    Console.WriteLine("Toyotas: "+toyota.Count);
                    JsonSerializerOptions serializador = new JsonSerializerOptions { WriteIndented = true };
                    string json=JsonSerializer.Serialize(toyota, serializador);
                    File.WriteAllText("toyota.json",json);
                    serializador = new JsonSerializerOptions {};
                    json = JsonSerializer.Serialize(toyota, serializador);
                    File.WriteAllText("toyotaCompacto.json", json);

                    FileInfo noCompacto = new FileInfo("toyota.json");
                    Console.WriteLine($"toyota.json {noCompacto.Length} bytes");

                    FileInfo compacto = new FileInfo("toyotaCompacto.json");
                    Console.WriteLine($"toyotaCompacto.json {compacto.Length} bytes");
                    Double porcentaje = (noCompacto.Length - compacto.Length)*100.0 / compacto.Length;
                    Console.WriteLine($"Ocupa un {porcentaje:N0}% mas el indented.");
                    string ruta = "toyota.json";
                    string jsonReleido = File.ReadAllText(ruta);
                    toyota = JsonSerializer.Deserialize<List<Coche>>(jsonReleido);
                    Console.WriteLine("Releìdos: "+toyota.Count +" "+compacto.Name);

                }
            }
            else
            {
                Console.WriteLine("No existe el archivo.");
            }
        }
    }
}
