using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Serialization;

namespace Ej9_Reto_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Coche> cochesCSV=LeerCsv("coches.csv");
            var sumaPreciosCSV = cochesCSV.Sum(s=>s.Precio);
            Console.WriteLine($"CSV: {cochesCSV.Count} coches, suma de precios {sumaPreciosCSV:N0}€");

            using (FileStream fs=File.OpenRead("coches.json"))
            {
                List<Coche> cochesJson=JsonSerializer.Deserialize<List<Coche>>(fs);
                var sumaPreciosJson = cochesJson.Sum(s => s.Precio);
                Console.WriteLine($"Json: {cochesJson.Count} coches, suma de precios {sumaPreciosJson:N0}€");
            }
            List<Coche> cochesXML;
            using (FileStream leer=File.OpenRead("coches.xml"))
            {
                XmlSerializer serializadorXML=new XmlSerializer(typeof(List<Coche>));
                cochesXML = (List<Coche>)serializadorXML.Deserialize(leer);           
                var sumaPreciosXML = cochesXML.Sum(s => s.Precio);
                Console.WriteLine($"Json: {cochesXML.Count} coches, suma de precios {sumaPreciosXML:N0}€");
            }

        }
        public static List<Coche> LeerCsv(String ruta)
        {

            List<Coche> coches = new List<Coche>();
            using (StreamReader sr = new StreamReader(ruta))
            {
                string linea = sr.ReadLine();
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] coche = linea.Split(';');
                    coches.Add(new Coche(int.Parse(coche[0]), coche[1], coche[2], int.Parse(coche[3]), coche[4], coche[5], int.Parse(coche[6]), int.Parse(coche[7])));
                }

            }
            return coches;
        }
    }
}
