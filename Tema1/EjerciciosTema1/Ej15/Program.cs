using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> nombres = new List<string> { "ana", "luis", "marta", "pablo" };
            var nombresMayuscula=nombres.Select(a=>a.ToUpper());
            var numeroLetras = nombres.Select(x => x.Length);
            var saludo = nombres.Select(x=>$"Hola {x}");
            Console.WriteLine("Mayusculas: "+string.Join("|",nombresMayuscula));
            Console.WriteLine("Numero letras: " + string.Join("|",numeroLetras));
            Console.WriteLine(string.Join("|",saludo));
            List<string> nombre4Letras = nombres
                .Where(n => n.Length > 4)
                .Select(n => n.ToUpper())
                .ToList();
            Console.WriteLine("Mas de 4 letras: " + string.Join("|", nombre4Letras));

        }
    }
}
