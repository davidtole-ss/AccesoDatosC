using System;

namespace Ej13
{
    internal class Podcast : IReproducible
    {
        public string Titulo { get; set; }
        public int Episodio { get; set; }

        public Podcast(string titulo, int episodio)
        {
            this.Titulo = titulo;
            this.Episodio = episodio;
        }

        public void Reproducir()
        {
            Console.WriteLine($"🎙 Reproduciendo pódcast: {Titulo} (episodio {Episodio})");
        }
    }
}
