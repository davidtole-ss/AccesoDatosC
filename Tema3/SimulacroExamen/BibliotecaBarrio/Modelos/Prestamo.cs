namespace BibliotecaBarrio.Modelos;

public class Prestamo
{
    public int IdPrestamo { get; set; }
    public int IdLibro { get; set; }
    public string NombreLector { get; set; } = "";
    public bool EsSocio { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public int Dias { get; set; }
    public decimal Fianza { get; set; }
    public string Estado { get; set; } = "Prestado";

    public Prestamo() { }

    public Prestamo(int idLibro, string nombreLector, bool esSocio, DateTime fechaPrestamo, int dias)
    {
        this.IdLibro = idLibro;
        this.NombreLector = nombreLector;
        this.EsSocio = esSocio;
        this.FechaPrestamo = fechaPrestamo;
        this.Dias = dias;
        this.Fianza = CalcularFianza();
    }

    public decimal CalcularFianza()
    {
        if (this.EsSocio)
        {
            return 0;
        }
        return this.Dias / 7 * 1.50m;
    }

    public DateTime FechaDevolucion()
    {
        return this.FechaPrestamo.AddDays(this.Dias);
    }

    public override string ToString()
    {
        string tipo = this.EsSocio ? "socio" : "no socio";
        return $"[{this.IdPrestamo}] Libro {this.IdLibro} - {this.NombreLector} ({tipo}) - " +
               $"{this.FechaPrestamo:dd/MM/yyyy} a {this.FechaDevolucion():dd/MM/yyyy} ({this.Dias} días) - " +
               $"fianza {this.Fianza:0.00} € - {this.Estado}";
    }
}
