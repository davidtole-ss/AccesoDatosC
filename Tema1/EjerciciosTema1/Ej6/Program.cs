using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string[] opciones = { "1) Serie de Fibonacci", "2) Números primos hasta N", "3) Secuencia de Collatz", "4) Salir" };
            int consultas = 0;
            int opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== LABORATORIO DE NÚMEROS ===");
                foreach (string texto in opciones)
                {
                    Console.WriteLine(texto);
                }
                opcion = LeerEntero("Elige una opción: ", 1, 4);

                switch (opcion)
                {
                    case 1:
                        int terminos = LeerEntero("¿Cuántos términos (1-50)? ", 1, 50);
                        long a = 0;
                        long b = 1;
                        for (int i = 0; i < terminos; i++)
                        {
                            Console.Write(a + " ");
                            long siguiente = a + b;
                            a = b;
                            b = siguiente;
                        }
                        Console.WriteLine();
                        consultas++;
                        break;

                    case 2:
                        int n = LeerEntero("¿Hasta qué número (2-1000)? ", 2, 1000);
                        int total = 0;
                        for (int candidato = 2; candidato <= n; candidato++)
                        {
                            bool esPrimo = true;
                            for (int divisor = 2; divisor * divisor <= candidato; divisor++)
                            {
                                if (candidato % divisor == 0)
                                {
                                    esPrimo = false;
                                    break;
                                }
                            }
                            if (!esPrimo)
                            {
                                continue;
                            }
                            Console.Write(candidato + " ");
                            total++;
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Total: {total} primos.");
                        consultas++;
                        break;

                    case 3:
                        long inicial = LeerEntero("Número inicial (1-1000000): ", 1, 1000000);
                        long valor = inicial;
                        long maximo = inicial;
                        int pasos = 0;
                        while (valor != 1)
                        {
                            if (valor % 2 == 0)
                            {
                                valor = valor / 2;
                            }
                            else
                            {
                                valor = 3 * valor + 1;
                            }
                            pasos++;
                            if (valor > maximo)
                            {
                                maximo = valor;
                            }
                        }
                        Console.WriteLine($"Collatz({inicial}) llega a 1 en {pasos} pasos. Valor máximo alcanzado: {maximo}.");
                        consultas++;
                        break;

                    case 4:
                        Console.WriteLine("Saliendo...");
                        break;
                }
            } while (opcion != 4);

            Console.WriteLine($"Has realizado {consultas} consultas. ¡Hasta luego!");
        }

        static int LeerEntero(string mensaje, int minimo, int maximo)
        {
            int valor;
            Console.Write(mensaje);
            while (!int.TryParse(Console.ReadLine(), out valor) || valor < minimo || valor > maximo)
            {
                Console.Write($"  Escribe un número entre {minimo} y {maximo}: ");
            }
            return valor;
        }
    }
}
