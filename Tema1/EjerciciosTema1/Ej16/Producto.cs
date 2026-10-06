using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej16
{
    internal class Producto
    {
    
        public string nombre {  get; set; }
        public string categoria { get; set; }
        public decimal precio { get; set; }

        public Producto(string nombre, string categoria,decimal precio) {

            this.nombre= nombre;
            this.categoria = categoria;
            this.precio= precio;
        }



    }
}
