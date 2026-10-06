using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Coche> coches = LeerCsv("coches.csv");
            Console.WriteLine("Coches leidos: "+coches.Count + "\n");

            var tiposCombustible = coches
                .GroupBy(c => c.Combustible).OrderByDescending(o=>o.Count())
                .Select(s => new{Combustible=s.Key,CantidadCoches=s.Count()});
            Console.WriteLine("-----Coches por combustible--------" + "\n");
            foreach (var c in tiposCombustible) {
                Console.WriteLine(c);
            }
            Console.WriteLine();
            var precioMedioCoches = coches.Average(a=>a.Precio);
            Console.WriteLine($"Precio medio coches:{precioMedioCoches:N0} €" + "\n");

            List<Coche> precioMenorMayor = coches.OrderBy(a => a.Precio).ToList();
            EscribirCsv("coches3.csv",precioMenorMayor);

            List<Coche> cochesElectricos = LeerCsv("cochesElectricos.csv");
            Console.WriteLine($"Releídos {cochesElectricos.Count} coches de cochesElectricos.csv ");

            var masCaro=coches.OrderByDescending(a => a.Precio).FirstOrDefault();
            var masBarato = coches.OrderBy(a => a.Precio).FirstOrDefault();
            Console.WriteLine("Coche mas caro: "+masCaro+" €");
            Console.WriteLine("Coche mas barato: "+masBarato+" €");



        }


        public static void EscribirCsv(string ruta, List<Coche> precioMenorMayor)
        {
            int numeroExportados = 0;
            using (StreamWriter sw = new StreamWriter("cochesElectricos.csv")) 
            {
                sw.WriteLine("id;marca;modelo;año;combustible;color;kilometros;precio");
                foreach(Coche c in precioMenorMayor)
                {
                    if (c.Combustible == "Electrico")
                    {
                        numeroExportados++;
                        sw.WriteLine($"{c.Id};{c.Marca};{c.Modelo};{c.Anio};{c.Combustible};{c.Color};{c.Kilometros};{c.Precio}");
                    }
                }
                Console.WriteLine("Coches exportados a cochesElectricos.csv :"+numeroExportados+"\n");
            }
        }
        

        public static List<Coche> LeerCsv(String ruta)
        {

            List<Coche> coches= new List<Coche>(); 
            using (StreamReader sr = new StreamReader(ruta)) { 
                string linea=sr.ReadLine();
                while ((linea = sr.ReadLine())!=null) { 
                    string[] coche = linea.Split(';');
                    coches.Add(new Coche(int.Parse(coche[0]), coche[1], coche[2], int.Parse(coche[3]), coche[4], coche[5], int.Parse(coche[6]), int.Parse(coche[7])));
                }

            }
            return coches;
        }
    }
}
