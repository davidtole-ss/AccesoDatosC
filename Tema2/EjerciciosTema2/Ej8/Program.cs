using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Ej8
{
    internal class Program
    {
        static void Main(string[] args)
        { string ruta = "coches.xml";
            if (File.Exists(ruta))
            {
                List<Coche> cochesCargados;
                using (FileStream fs = File.OpenRead(ruta))
                {
                    XmlSerializer serializadorXML = new XmlSerializer(typeof(List<Coche>));
                    cochesCargados=(List<Coche>)serializadorXML.Deserialize(fs);
                    Console.WriteLine(cochesCargados.Count+" leidos y cargados.");
                    List<Coche> chollos = cochesCargados.Where(c => c.Anio >= 2018 && c.Kilometros<60000&&c.Precio<15000).OrderBy(c=>c.Precio).ToList();
                    Console.WriteLine($"Hay {chollos.Count} chollos.");
                    var chollosMasBaratos = chollos.Take(5);
                    Console.WriteLine("Los 5 chollos mas baratos:");
                    foreach(var chollo in chollosMasBaratos)
                    {
                        Console.WriteLine(chollo);
                    }
                    using (StreamWriter escritor = new StreamWriter("chollos.xml"))
                    {
                        serializadorXML.Serialize(escritor, chollos);
                        Console.WriteLine($"Guardados {chollos.Count} en chollos.xml");
                    }

                    List<Coche> cargarChollos;
                    using (FileStream leer = File.OpenRead("chollos.xml"))
                    {
                       cargarChollos = (List<Coche>)serializadorXML.Deserialize(leer);
                        Console.WriteLine($"Releídos {cargarChollos.Count} chollos.");
                    }
                    Console.WriteLine("Primero: "+cargarChollos[0]);
                   

                }
            }
        }
    }
}
