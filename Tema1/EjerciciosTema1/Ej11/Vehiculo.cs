using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej11
{
    internal class Vehiculo
    {
        private string marca { get; set; }
        private string modelo { get; set; }
        private int anio { get; set; }

        public Vehiculo(string marca, string modelo, int anio)
        {
            this.marca= marca;
            this.modelo= modelo;
            this.anio= anio;
        }

        public virtual void mostrarInfo()
        {
            Console.WriteLine($"Marca: {marca}\nModelo: {modelo}\nAño: {anio}");
        }


    }
}
