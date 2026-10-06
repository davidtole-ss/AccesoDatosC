using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ej5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int MIN = 1;
            const int MAX =100;
            Random random = new Random();
            int secreto = random.Next(MIN, MAX + 1);
            const int maxIntentos = 7;
            int numeroIntentos = 1;
            Boolean acertado=false;
            int numero;
            int partida = 1;
            string respuesta="";
            int partidasGanadas = 0;
            do
            {
                while (numeroIntentos < 8 && !acertado)
                {
                    if(numeroIntentos==1) Console.WriteLine($"Partida {partida}: he pensado un número entre 1 y 100. Tienes 7 intentos.");
                    Console.Write($"Intento:{numeroIntentos}/7: ");
                    if (!int.TryParse(Console.ReadLine(), out numero) || (numero < MIN || numero > MAX))
                    {
                        Console.WriteLine("El numero introducido no cumple con las normas.");
                        Console.WriteLine("[Error ]Intento fallido, no cuenta.");
                        continue;
                    }
                    else
                    {
                        if (numero < secreto)
                        {
                            Console.WriteLine("El número secreto es mayor.");
                            numeroIntentos++;
                        }
                        else if (numero > secreto)
                        {
                            Console.WriteLine("El número secreto es menor.");
                            numeroIntentos++;
                        }
                        else
                        {
                            Console.WriteLine("¡Correcto! Lo has adivinado.");
                            acertado = true;
                        }

                    }

                }
                switch (numeroIntentos)
                {
                    case 1:
                        Console.WriteLine("Increible");
                        partidasGanadas++;
                        break;
                    case 2:
                    case 3:
                        Console.WriteLine("Excelente");
                        partidasGanadas++;
                        break;
                    case 4:
                    case 5:
                        Console.WriteLine("Bien jugado");
                        partidasGanadas++;
                        break;
                    case 6:
                    case 7:
                        Console.WriteLine("Justito");
                        partidasGanadas++;
                        break;
                    default:
                        Console.WriteLine("Maximo de intentos alcanzados.");
                        Console.WriteLine("El numero secreto era el: " + secreto);
                        break;
                       
                }
                Console.WriteLine("¿Otra partida? s/n");
                respuesta = Console.ReadLine();
                if (respuesta.Trim().ToLower()=="s")
                {
                  numeroIntentos = 1;
                  partida++;
                  secreto = random.Next(MIN, MAX + 1);
                  acertado=false;  
                }
            } while(respuesta=="s");

            Console.WriteLine($"Has ganado {partidasGanadas} de {partida} partidas.");
              
        }
    }
}
