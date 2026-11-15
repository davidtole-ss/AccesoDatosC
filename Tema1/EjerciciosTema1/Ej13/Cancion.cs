using System;

namespace Ej13
{
    internal class Cancion : IReproducible, IDescargable
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }

        public Cancion(string titulo, string artista)
        {
            this.Titulo = titulo;
            this.Artista = artista;
        }

        public void Reproducir()
        {
            Console.WriteLine($"♪ Reproduciendo canción: {Titulo} - {Artista}");
        }

        public void Descargar()
        {
            Console.WriteLine($"⬇ Descargando canción: {Titulo}");
        }
    }
}
