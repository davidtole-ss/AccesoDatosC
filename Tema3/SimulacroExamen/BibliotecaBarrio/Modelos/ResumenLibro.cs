namespace BibliotecaBarrio.Modelos;

public class ResumenLibro
{
    public int IdLibro { get; set; }
    public string Titulo { get; set; } = "";
    public int Ejemplares { get; set; }
    public int TotalPrestamos { get; set; }
    public int PrestadosAhora { get; set; }
    public int Disponibles { get; set; }
    public decimal FianzasCobradas { get; set; }

    public override string ToString()
    {
        return $"[{this.IdLibro}] {this.Titulo}: {this.TotalPrestamos} préstamos, " +
               $"{this.PrestadosAhora} fuera, {this.Disponibles} de {this.Ejemplares} disponibles, " +
               $"fianzas {this.FianzasCobradas:0.00} €";
    }
}
