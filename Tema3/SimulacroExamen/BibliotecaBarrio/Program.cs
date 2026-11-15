using System.Globalization;
using BibliotecaBarrio.Datos;
using BibliotecaBarrio.Modelos;
using BibliotecaBarrio.Repositorios;

namespace BibliotecaBarrio;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            using var prueba = Conexion.Conectar();
            Console.WriteLine($" Conectado a {prueba.Database} en {prueba.DataSource}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($" {ex.Message}: {ex.InnerException?.Message}");
            Console.WriteLine("¿Has ejecutado BibliotecaBarrio.sql en SSMS?");
            return;
        }

        PrestamoRepository repo = new PrestamoRepository();
        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("BIBLIOTECA DE BARRIO");
            Console.WriteLine("1. Cargar préstamos desde el fichero");
            Console.WriteLine("2. Nuevo préstamo");
            Console.WriteLine("3. Buscar préstamo por Id");
            Console.WriteLine("4. Buscar préstamos de un lector");
            Console.WriteLine("5. Préstamos pendientes de devolver");
            Console.WriteLine("6. Registrar devolución");
            Console.WriteLine("7. Eliminar préstamo");
            Console.WriteLine("8. Resumen por libro");
            Console.WriteLine("9. Ejemplares disponibles de un libro");
            Console.WriteLine("0. Salir");
            opcion = LeerEntero("Opción: ");
            Console.WriteLine();

            try
            {
                switch (opcion)
                {
                    case 1:
                        if (repo.ContarTotal() > 0)
                        {
                            Console.WriteLine("Los préstamos ya están cargados.");
                            break;
                        }
                        string ruta = Path.Combine(AppContext.BaseDirectory, "Datos", "prestamos.csv");
                        List<Prestamo> leidos = repo.CargarFichero(ruta);
                        repo.InsertarLote(leidos);
                        Console.WriteLine($" Cargados {leidos.Count} préstamos.");
                        break;

                    case 2:
                        int idLibro = LeerEntero("Id del libro: ");
                        int? disponibles = repo.EjemplaresDisponibles(idLibro);
                        if (disponibles == null)
                        {
                            Console.WriteLine("Ese libro no existe.");
                            break;
                        }
                        if (disponibles == 0)
                        {
                            Console.WriteLine("No quedan ejemplares disponibles de ese libro.");
                            break;
                        }
                        string lector = LeerTexto("Nombre del lector: ");
                        bool esSocio = LeerTexto("¿Es socio? (s/n): ").ToLower() == "s";
                        int dias = LeerEntero("Días (7, 14 o 21): ");
                        while (dias != 7 && dias != 14 && dias != 21)
                        {
                            dias = LeerEntero("  Solo 7, 14 o 21: ");
                        }
                        Prestamo nuevo = new Prestamo(idLibro, lector, esSocio, DateTime.Today, dias);
                        repo.Insertar(nuevo);
                        Console.WriteLine($"Préstamo {nuevo.IdPrestamo} registrado. Fianza: {nuevo.Fianza:0.00} €. " +
                                          $"Devolver antes del {nuevo.FechaDevolucion():dd/MM/yyyy}");
                        break;

                    case 3:
                        Prestamo? encontrado = repo.ObtenerPorId(LeerEntero("Id del préstamo: "));
                        if (encontrado == null)
                        {
                            Console.WriteLine("No existe ningún préstamo con ese Id.");
                        }
                        else
                        {
                            Console.WriteLine(encontrado);
                        }
                        break;

                    case 4:
                        Mostrar(repo.BuscarPorLector(LeerTexto("Nombre (o parte) del lector: ")));
                        break;

                    case 5:
                        Mostrar(repo.ObtenerPendientes());
                        break;

                    case 6:
                        if (repo.MarcarDevuelto(LeerEntero("Id del préstamo: ")))
                        {
                            Console.WriteLine("Devolución registrada.");
                        }
                        else
                        {
                            Console.WriteLine("No existe ese préstamo o no estaba prestado.");
                        }
                        break;

                    case 7:
                        int idBorrar = LeerEntero("Id del préstamo: ");
                        if (LeerTexto("¿Seguro? Se borrará para siempre (s/n): ").ToLower() == "s")
                        {
                            Console.WriteLine(repo.Eliminar(idBorrar) ? "Eliminado." : "No existe ese Id.");
                        }
                        break;

                    case 8:
                        foreach (ResumenLibro r in repo.ObtenerResumen())
                        {
                            Console.WriteLine(r);
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Prestados: {repo.ContarPorEstado("Prestado")} | " +
                                          $"Devueltos: {repo.ContarPorEstado("Devuelto")} | " +
                                          $"Perdidos: {repo.ContarPorEstado("Perdido")}");
                        break;

                    case 9:
                        int? quedan = repo.EjemplaresDisponibles(LeerEntero("Id del libro: "));
                        Console.WriteLine(quedan == null ? "Ese libro no existe." : $"Ejemplares disponibles: {quedan}");
                        break;

                    case 0:
                        Console.WriteLine("¡Hasta luego!");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        } while (opcion != 0);
    }

    private static void Mostrar(List<Prestamo> prestamos)
    {
        if (prestamos.Count == 0)
        {
            Console.WriteLine("(no hay resultados)");
            return;
        }
        foreach (Prestamo p in prestamos)
        {
            Console.WriteLine(p);
        }
    }

    private static int LeerEntero(string mensaje)
    {
        int valor;
        Console.Write(mensaje);
        while (!int.TryParse(Console.ReadLine(), out valor))
            Console.Write("  Escribe un número entero: ");
        return valor;
    }

    private static string LeerTexto(string mensaje)
    {
        string texto;
        do
        {
            Console.Write(mensaje);
            texto = (Console.ReadLine() ?? "").Trim();
        } while (texto == "");
        return texto;
    }

    private static DateTime LeerFecha(string mensaje)
    {
        DateTime fecha;
        Console.Write(mensaje);
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out fecha))
            Console.Write("  Formato AAAA-MM-DD, por ejemplo 2026-11-20: ");
        return fecha;
    }
}
