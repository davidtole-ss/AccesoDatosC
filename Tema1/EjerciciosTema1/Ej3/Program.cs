using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Introduce la altura: ");
            //int altura;
            //while ((!int.TryParse(Console.ReadLine(), out altura))||!(altura>0&&altura<=20))
            //{
            //    Console.WriteLine("La altura tiene que ser un número entre 1-20");
            //}
            //for (int i = 1; i <= altura; i++)
            //{
            //    for (int j = 1; j <= altura - i; j++)
            //    {
            //        Console.Write(" ");
            //    }
            //    for (int j = 1; j <= i*2-1; j++)
            //    {
            //        Console.Write("*");
            //    }
            //    Console.WriteLine();
            //}

            //Console.WriteLine("Introduce un número entero y positivo:");
            //int numero;
            //while (!int.TryParse(Console.ReadLine(), out  numero) || numero<0) 
            //{
            //    Console.WriteLine("Introduce un número entero y positivo:");
            //}
            //int cifra=0;
            //int digitos=0;
            //int numeroInverso = 0;
            //while (numero > 0)  //232
            //{
            //    cifra = numero%10;
            //    numeroInverso = numeroInverso * 10 + cifra;
            //    numero = numero / 10;
            //    digitos++;
            //}
            //Console.WriteLine("Numero inverso:"+numeroInverso);
            //Console.WriteLine($"Numero digitos: {digitos}");
            //Console.WriteLine($"{(numero==numeroInverso?"Numero capicua.":"No es un número capicua.")}");

            for (int i = 1; i <= 30; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Console.Write("FizzBuzz ");
                }
                else if (i % 3 == 0)
                {
                    Console.Write("Fizz ");
                }
                else if (i % 5 == 0)
                {
                    Console.Write("Buzz ");

                }
                else
                {
                    Console.Write(i+" ");
                }
            }
        }

           
        }
    }

