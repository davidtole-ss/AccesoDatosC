using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej16
{
    internal class Producto
    {

        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }

        public Producto(string nombre, string categoria, decimal precio)
        {
            this.Nombre = nombre;
            this.Categoria = categoria;
            this.Precio = precio;
        }

        public override string ToString()
        {
            return $"{Nombre}: {Precio:0.00} €";
        }
    }
}
