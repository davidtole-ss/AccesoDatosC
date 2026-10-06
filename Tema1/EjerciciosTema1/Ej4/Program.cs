using System;

namespace Ej4
{
    internal class Program
    {
        const decimal LIMITE_RETIRADA = 300m;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            decimal saldo = 1000m;
            int operaciones = 0;
            int opcion;

            string[] menu =
            {
                "1) Consultar saldo",
                "2) Ingresar dinero",
                "3) Retirar dinero",
                "4) Salir"
            };

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== CAJERO AUTOMÁTICO ===");
                foreach (string linea in menu)
                {
                    Console.WriteLine(linea);
                }
                Console.Write("Elige una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción no válida: escribe un número.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        Console.WriteLine($"Saldo actual: {saldo:F2} €");
                        break;

                    case 2:
                        {
                            Console.Write("Importe a ingresar: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal importe))
                            {
                                Console.WriteLine("Importe no válido: escribe un número.");
                            }
                            else if (importe <= 0)
                            {
                                Console.WriteLine("El importe debe ser mayor que 0.");
                            }
                            else
                            {
                                saldo += importe;
                                operaciones++;
                                Console.WriteLine($"Ingreso realizado. Nuevo saldo: {saldo:F2} €");
                            }
                            break;
                        }

                    case 3:
                        {
                            Console.Write("Importe a retirar: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal importe))
                            {
                                Console.WriteLine("Importe no válido: escribe un número.");
                            }
                            else if (importe <= 0)
                            {
                                Console.WriteLine("El importe debe ser mayor que 0.");
                            }
                            else if (importe > LIMITE_RETIRADA)
                            {
                                Console.WriteLine($"El límite por operación es de {LIMITE_RETIRADA:F2} €.");
                            }
                            else if (importe > saldo)
                            {
                                Console.WriteLine($"Saldo insuficiente. Saldo disponible: {saldo:F2} €");
                            }
                            else
                            {
                                saldo -= importe;
                                operaciones++;
                                Console.WriteLine($"Retirada realizada. Nuevo saldo: {saldo:F2} €");
                            }
                            break;
                        }

                    case 4:
                        Console.WriteLine("Gracias por usar el cajero.");
                        Console.WriteLine($"Operaciones realizadas: {operaciones}. Saldo final: {saldo:F2} €");
                        break;

                    default:
                        Console.WriteLine("Opción no válida: elige un número entre 1 y 4.");
                        break;
                }
            } while (opcion != 4);
        }
    }
}