namespace Ej17
{
    internal class Alumno
    {
        public string Nombre { get; set; }
        public double Nota { get; set; }

        public Alumno(string nombre, double nota)
        {
            this.Nombre = nombre;
            this.Nota = nota;
        }
    }
}
