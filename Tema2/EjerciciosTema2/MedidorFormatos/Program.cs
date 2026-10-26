using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace MedidorFormatos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            const string ficheroMaestro = "coches.xml";
            const string carpetaSalida = "salida";
            const string carpetaPruebas = "pruebas";

            if (!File.Exists(ficheroMaestro))
            {
                Console.WriteLine($"No se encuentra {ficheroMaestro}.");
                return;
            }

            // A.1: leer el catálogo maestro (XML)
            List<Coche> coches = LeerXml(ficheroMaestro);
            Console.WriteLine($"Coches leídos de {ficheroMaestro}: {coches.Count}");
            Console.WriteLine();

            // A.1: generar salida/ en los 3 formatos
            Directory.CreateDirectory(carpetaSalida);
            string rutaCsv = Path.Combine(carpetaSalida, "coches.csv");
            string rutaJson = Path.Combine(carpetaSalida, "coches.json");
            string rutaXml = Path.Combine(carpetaSalida, "coches.xml");

            EscribirCsv(rutaCsv, coches);
            EscribirJson(rutaJson, coches);
            EscribirXml(rutaXml, coches);

            MostrarTablaTamanos(rutaCsv, rutaJson, rutaXml);

            // A.1: releer salida/ y contar sospechosos
            Console.WriteLine("--- Releyendo los ficheros generados ---");
            MostrarLectura("coches.csv", LeerCsv(rutaCsv));
            MostrarLectura("coches.json", LeerJson(rutaJson));
            MostrarLectura("coches.xml", LeerXml(rutaXml));
            Console.WriteLine();

            // A.2: laboratorio de errores
            GenerarPruebas(carpetaSalida, carpetaPruebas);

            Console.WriteLine("--- Laboratorio de errores ---");
            EjecutarLaboratorio(carpetaPruebas);
            Console.WriteLine();

            // Extra: coche Id=1000 en los 3 formatos (para la infografía)
            MostrarCoche1000(coches);
        }

        // Lectura

        static List<Coche> LeerXml(string ruta)
        {
            using (FileStream fs = File.OpenRead(ruta))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(List<Coche>));
                return (List<Coche>)serializador.Deserialize(fs);
            }
        }

        static List<Coche> LeerCsv(string ruta)
        {
            List<Coche> coches = new List<Coche>();
            using (StreamReader sr = new StreamReader(ruta))
            {
                string linea = sr.ReadLine(); // descarta la cabecera
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] campo = linea.Split(';');
                    coches.Add(new Coche(
                        int.Parse(campo[0]), campo[1], campo[2], int.Parse(campo[3]),
                        campo[4], campo[5], int.Parse(campo[6]), int.Parse(campo[7])));
                }
            }
            return coches;
        }

        static List<Coche> LeerJson(string ruta)
        {
            string json = File.ReadAllText(ruta);
            return JsonSerializer.Deserialize<List<Coche>>(json);
        }

        // Escritura

        static void EscribirCsv(string ruta, List<Coche> coches)
        {
            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine("Id;Marca;Modelo;Anio;Combustible;Color;Kilometros;Precio");
                foreach (Coche c in coches)
                {
                    sw.WriteLine($"{c.Id};{c.Marca};{c.Modelo};{c.Anio};{c.Combustible};{c.Color};{c.Kilometros};{c.Precio}");
                }
            }
        }

        static void EscribirJson(string ruta, List<Coche> coches)
        {
            JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(coches, opciones);
            File.WriteAllText(ruta, json);
        }

        static void EscribirXml(string ruta, List<Coche> coches)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(List<Coche>));
            using (StreamWriter sw = new StreamWriter(ruta))
            {
                serializador.Serialize(sw, coches);
            }
        }

        // Medidas y comprobaciones

        static void MostrarTablaTamanos(string rutaCsv, string rutaJson, string rutaXml)
        {
            Console.WriteLine("--- Tamaño de los ficheros generados ---");
            Console.WriteLine($"{"Formato",-8}{"Bytes",15}{"MB",10}");
            MostrarFila("CSV", rutaCsv);
            MostrarFila("JSON", rutaJson);
            MostrarFila("XML", rutaXml);
            Console.WriteLine();
        }

        static void MostrarFila(string formato, string ruta)
        {
            FileInfo info = new FileInfo(ruta);
            double mb = info.Length / (1024.0 * 1024.0);
            Console.WriteLine($"{formato,-8}{info.Length,15:N0}{mb,10:N2}");
        }

        static bool EsSospechoso(Coche c)
        {
            return string.IsNullOrEmpty(c.Marca) || string.IsNullOrEmpty(c.Modelo) ||
                   string.IsNullOrEmpty(c.Combustible) || string.IsNullOrEmpty(c.Color) ||
                   c.Anio == 0 || c.Kilometros == 0 || c.Precio == 0;
        }

        static void MostrarLectura(string nombre, List<Coche> coches)
        {
            int sospechosos = coches.Count(EsSospechoso);
            Console.WriteLine($"{nombre}: {coches.Count} coches, {sospechosos} sospechosos");
        }

        // A.2: Laboratorio de errores

        static void GenerarPruebas(string carpetaSalida, string carpetaPruebas)
        {
            Directory.CreateDirectory(carpetaPruebas);

            // CSV
            string[] lineasCsv = File.ReadAllLines(Path.Combine(carpetaSalida, "coches.csv"));

            // Separador de más: inserta un campo vacío extra en la primera fila de datos.
            List<string> conSeparadorDeMas = lineasCsv[1].Split(';').ToList();
            conSeparadorDeMas.Insert(2, "");
            string[] csvSeparador = (string[])lineasCsv.Clone();
            csvSeparador[1] = string.Join(";", conSeparadorDeMas);
            File.WriteAllLines(Path.Combine(carpetaPruebas, "csv_separador_de_mas.csv"), csvSeparador);

            // Falta un campo: quita el último campo (Precio) de la primera fila de datos.
            string[] sinUltimoCampo = lineasCsv[1].Split(';').Take(7).ToArray();
            string[] csvFaltaCampo = (string[])lineasCsv.Clone();
            csvFaltaCampo[1] = string.Join(";", sinUltimoCampo);
            File.WriteAllLines(Path.Combine(carpetaPruebas, "csv_falta_un_campo.csv"), csvFaltaCampo);

            // JSON
            string json = File.ReadAllText(Path.Combine(carpetaSalida, "coches.json"));

            // Clave con mayúscula distinta: "Precio" -> "precio" en el primer coche (error silencioso).
            int posicionPrecio = json.IndexOf("\"Precio\":");
            string jsonMayuscula = json.Substring(0, posicionPrecio) + "\"precio\":" + json.Substring(posicionPrecio + "\"Precio\":".Length);
            File.WriteAllText(Path.Combine(carpetaPruebas, "json_clave_con_mayuscula_distinta.json"), jsonMayuscula);

            // Coma que falta: quita la coma que separa Marca de Modelo en el primer coche.
            int posicionModelo = json.IndexOf("\"Modelo\":");
            int posicionComa = json.LastIndexOf(',', posicionModelo);
            string jsonSinComa = json.Remove(posicionComa, 1);
            File.WriteAllText(Path.Combine(carpetaPruebas, "json_coma_que_falta.json"), jsonSinComa);

            // XML
            string xml = File.ReadAllText(Path.Combine(carpetaSalida, "coches.xml"));

            // Etiqueta sin cerrar: quita el </Marca> del primer coche.
            int posicionCierreMarca = xml.IndexOf("</Marca>");
            string xmlSinCierre = xml.Remove(posicionCierreMarca, "</Marca>".Length);
            File.WriteAllText(Path.Combine(carpetaPruebas, "xml_etiqueta_sin_cerrar.xml"), xmlSinCierre);

            // Campo vacío: vacía el contenido del primer <Marca>.
            int posicionAbreMarca = xml.IndexOf("<Marca>") + "<Marca>".Length;
            int posicionCierreMarca2 = xml.IndexOf("</Marca>", posicionAbreMarca);
            string xmlVacio = xml.Remove(posicionAbreMarca, posicionCierreMarca2 - posicionAbreMarca);
            File.WriteAllText(Path.Combine(carpetaPruebas, "xml_campo_vacio.xml"), xmlVacio);

            Console.WriteLine($"Generados 6 ficheros de pruebas en \"{carpetaPruebas}\".");
            Console.WriteLine();
        }

        static void EjecutarLaboratorio(string carpetaPruebas)
        {
            foreach (string archivo in Directory.GetFiles(carpetaPruebas))
            {
                string nombre = Path.GetFileName(archivo);
                string extension = Path.GetExtension(archivo).ToLower();
                try
                {
                    List<Coche> coches;
                    switch (extension)
                    {
                        case ".csv":
                            coches = LeerCsv(archivo);
                            break;
                        case ".json":
                            coches = LeerJson(archivo);
                            break;
                        case ".xml":
                            coches = LeerXml(archivo);
                            break;
                        default:
                            throw new NotSupportedException($"Extensión no soportada: {extension}");
                    }
                    int sospechosos = coches.Count(EsSospechoso);
                    Console.WriteLine($"{nombre}: OK -> {coches.Count} coches, {sospechosos} sospechosos");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{nombre}: ERROR -> {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        // Extra: coche para la infografía

        static void MostrarCoche1000(List<Coche> coches)
        {
            Coche c = coches.FirstOrDefault(x => x.Id == 1000);
            if (c == null)
            {
                Console.WriteLine("No se ha encontrado el coche con Id 1000.");
                return;
            }

            Console.WriteLine("--- Coche Id=1000 en los tres formatos (para la infografía) ---");

            Console.WriteLine("CSV:");
            Console.WriteLine("Id;Marca;Modelo;Anio;Combustible;Color;Kilometros;Precio");
            Console.WriteLine($"{c.Id};{c.Marca};{c.Modelo};{c.Anio};{c.Combustible};{c.Color};{c.Kilometros};{c.Precio}");
            Console.WriteLine();

            Console.WriteLine("JSON:");
            JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
            Console.WriteLine(JsonSerializer.Serialize(c, opciones));
            Console.WriteLine();

            Console.WriteLine("XML:");
            XmlSerializer serializadorUnCoche = new XmlSerializer(typeof(Coche));
            using (StringWriter sw = new StringWriter())
            {
                serializadorUnCoche.Serialize(sw, c);
                Console.WriteLine(sw.ToString());
            }
        }
    }
}
