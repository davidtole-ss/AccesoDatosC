using System;

namespace Ej2
{
    internal class RecursoDemo : IDisposable
    {
        private string nombre;

        public RecursoDemo(string nombre)
        {
            this.nombre = nombre;
            Console.WriteLine("[" + nombre + "] Abierto");
        }

        public void Usar()
        {
            Console.WriteLine("[" + nombre + "] Trabajando...");
        }

        public void Dispose()
        {
            Console.WriteLine("[" + nombre + "] Cerrado (Dispose)");
        }
    }
}
