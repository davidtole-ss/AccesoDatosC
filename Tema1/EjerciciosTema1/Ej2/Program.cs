using System;

namespace Ej2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Introduce una fecha válida:");

            int año;
            Console.Write("Introduce el año: ");
            while (!int.TryParse(Console.ReadLine(), out año) || año <= 0)
            {
                Console.Write("Valor no válido. Introduce un año mayor que 0: ");
            }

            int mes;
            Console.Write("Introduce el mes (1-12): ");
            while (!int.TryParse(Console.ReadLine(), out mes) || mes < 1 || mes > 12)
            {
                Console.Write("Valor no válido. Introduce un número entre 1 y 12: ");
            }

            bool esBisiesto = año % 4 == 0 && (año % 100 != 0 || año % 400 == 0);
            if (esBisiesto)
            {
                Console.WriteLine($"{año} es un año bisiesto.");
            }

            string nombreMes = "";
            int numeroDias = 0;
            string estacion = "";

            switch (mes)
            {
                case 1: nombreMes = "Enero"; break;
                case 2: nombreMes = "Febrero"; break;
                case 3: nombreMes = "Marzo"; break;
                case 4: nombreMes = "Abril"; break;
                case 5: nombreMes = "Mayo"; break;
                case 6: nombreMes = "Junio"; break;
                case 7: nombreMes = "Julio"; break;
                case 8: nombreMes = "Agosto"; break;
                case 9: nombreMes = "Septiembre"; break;
                case 10: nombreMes = "Octubre"; break;
                case 11: nombreMes = "Noviembre"; break;
                case 12: nombreMes = "Diciembre"; break;
            }

            switch (mes)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    numeroDias = 31; break;
                case 4:
                case 6:
                case 9:
                case 11:
                    numeroDias = 30; break;
                case 2:
                    numeroDias = esBisiesto ? 29 : 28; break;
            }

            switch (mes)
            {
                case 12: case 1: case 2: estacion = "invierno"; break;
                case 3: case 4: case 5: estacion = "primavera"; break;
                case 6: case 7: case 8: estacion = "verano"; break;
                case 9: case 10: case 11: estacion = "otoño"; break;
            }

            int dia;
            Console.Write($"Introduce el día (1-{numeroDias}): ");
            while (!int.TryParse(Console.ReadLine(), out dia) || dia < 1 || dia > numeroDias)
            {
                Console.Write($"Valor no válido. {nombreMes} de {año} tiene {numeroDias} días. Introduce un día entre 1 y {numeroDias}: ");
            }

            Console.WriteLine($"La fecha {dia}/{mes}/{año} es válida.");
            Console.WriteLine($"{nombreMes} de {año} tiene {numeroDias} días.");
            Console.WriteLine($"Días restantes del mes: {numeroDias - dia}");
            Console.WriteLine($"Estación aproximada: {estacion}");
        }
    }
}