using System.Data;
using System.Globalization;
using BibliotecaBarrio.Datos;
using BibliotecaBarrio.Modelos;
using Microsoft.Data.SqlClient;

namespace BibliotecaBarrio.Repositorios;

public class PrestamoRepository
{
    private const string Columnas =
        "IdPrestamo, IdLibro, NombreLector, EsSocio, FechaPrestamo, Dias, Fianza, Estado";

    private static Prestamo Mapear(SqlDataReader lector)
    {
        return new Prestamo
        {
            IdPrestamo = lector.GetInt32(lector.GetOrdinal("IdPrestamo")),
            IdLibro = lector.GetInt32(lector.GetOrdinal("IdLibro")),
            NombreLector = lector.GetString(lector.GetOrdinal("NombreLector")),
            EsSocio = lector.GetBoolean(lector.GetOrdinal("EsSocio")),
            FechaPrestamo = lector.GetDateTime(lector.GetOrdinal("FechaPrestamo")),
            Dias = lector.GetInt32(lector.GetOrdinal("Dias")),
            Fianza = lector.GetDecimal(lector.GetOrdinal("Fianza")),
            Estado = lector.GetString(lector.GetOrdinal("Estado"))
        };
    }

    private static List<Prestamo> LeerLista(SqlCommand comando)
    {
        List<Prestamo> prestamos = new List<Prestamo>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            prestamos.Add(Mapear(lector));
        }
        return prestamos;
    }

    public int Insertar(Prestamo p)
    {
        try
        {
            using SqlConnection conexion = Conexion.Conectar();
            return InsertarEn(p, conexion, null);
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            throw new InvalidOperationException(
                $"No se pudo registrar el préstamo: el libro {p.IdLibro} no existe o algún dato no es válido", ex);
        }
    }

    private static int InsertarEn(Prestamo p, SqlConnection conexion, SqlTransaction? transaccion)
    {
        string sql = @"INSERT INTO Prestamos (IdLibro, NombreLector, EsSocio, FechaPrestamo, Dias, Fianza, Estado)
                       OUTPUT INSERTED.IdPrestamo
                       VALUES (@IdLibro, @NombreLector, @EsSocio, @FechaPrestamo, @Dias, @Fianza, @Estado)";

        using SqlCommand comando = new SqlCommand(sql, conexion, transaccion);
        comando.Parameters.AddWithValue("@IdLibro", p.IdLibro);
        comando.Parameters.AddWithValue("@NombreLector", p.NombreLector);
        comando.Parameters.AddWithValue("@EsSocio", p.EsSocio);
        comando.Parameters.AddWithValue("@FechaPrestamo", p.FechaPrestamo);
        comando.Parameters.AddWithValue("@Dias", p.Dias);
        comando.Parameters.AddWithValue("@Fianza", p.Fianza);
        comando.Parameters.AddWithValue("@Estado", p.Estado);

        p.IdPrestamo = (int)comando.ExecuteScalar()!;
        return p.IdPrestamo;
    }

    public Prestamo? ObtenerPorId(int id)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            $"SELECT {Columnas} FROM Prestamos WHERE IdPrestamo = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        using SqlDataReader lector = comando.ExecuteReader();

        if (lector.Read())
        {
            return Mapear(lector);
        }
        return null;
    }

    public List<Prestamo> BuscarPorLector(string texto)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            $"SELECT {Columnas} FROM Prestamos WHERE NombreLector LIKE @patron ORDER BY FechaPrestamo DESC",
            conexion);
        comando.Parameters.AddWithValue("@patron", $"%{texto}%");

        return LeerLista(comando);
    }

    public List<Prestamo> ObtenerPendientes()
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            $"SELECT {Columnas} FROM Prestamos WHERE Estado = N'Prestado' ORDER BY FechaPrestamo", conexion);

        return LeerLista(comando);
    }

    public int ContarTotal()
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand("SELECT COUNT(*) FROM Prestamos", conexion);

        return (int)comando.ExecuteScalar()!;
    }

    public int ContarPorEstado(string estado)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            "SELECT COUNT(*) FROM Prestamos WHERE Estado = @estado", conexion);
        comando.Parameters.AddWithValue("@estado", estado);

        return (int)comando.ExecuteScalar()!;
    }

    public List<ResumenLibro> ObtenerResumen()
    {
        List<ResumenLibro> resumen = new List<ResumenLibro>();
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            @"SELECT IdLibro, Titulo, Ejemplares, TotalPrestamos, PrestadosAhora, Disponibles, FianzasCobradas
              FROM vw_ResumenLibros ORDER BY IdLibro", conexion);
        using SqlDataReader lector = comando.ExecuteReader();

        while (lector.Read())
        {
            resumen.Add(new ResumenLibro
            {
                IdLibro = lector.GetInt32(lector.GetOrdinal("IdLibro")),
                Titulo = lector.GetString(lector.GetOrdinal("Titulo")),
                Ejemplares = lector.GetInt32(lector.GetOrdinal("Ejemplares")),
                TotalPrestamos = lector.GetInt32(lector.GetOrdinal("TotalPrestamos")),
                PrestadosAhora = lector.GetInt32(lector.GetOrdinal("PrestadosAhora")),
                Disponibles = lector.GetInt32(lector.GetOrdinal("Disponibles")),
                FianzasCobradas = lector.GetDecimal(lector.GetOrdinal("FianzasCobradas"))
            });
        }
        return resumen;
    }

    public int? EjemplaresDisponibles(int idLibro)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand("usp_EjemplaresDisponibles", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.AddWithValue("@IdLibro", idLibro);

        SqlParameter pDisponibles = comando.Parameters.Add("@Disponibles", SqlDbType.Int);
        pDisponibles.Direction = ParameterDirection.Output;

        comando.ExecuteNonQuery();

        if (pDisponibles.Value == DBNull.Value)
        {
            return null;
        }
        return (int)pDisponibles.Value;
    }

    public bool MarcarDevuelto(int id)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand(
            "UPDATE Prestamos SET Estado = N'Devuelto' WHERE IdPrestamo = @id AND Estado = N'Prestado'", conexion);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() == 1;
    }

    public bool Eliminar(int id)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlCommand comando = new SqlCommand("DELETE FROM Prestamos WHERE IdPrestamo = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() == 1;
    }

    public List<Prestamo> CargarFichero(string ruta)
    {
        List<Prestamo> prestamos = new List<Prestamo>();
        string[] estados = { "Prestado", "Devuelto", "Perdido" };

        using StreamReader sr = new StreamReader(ruta);
        string? linea = sr.ReadLine();
        int numeroLinea = 1;

        while ((linea = sr.ReadLine()) != null)
        {
            numeroLinea++;
            string[] campos = linea.Split(';');

            if (campos.Length != 6)
            {
                Console.WriteLine($"Línea {numeroLinea}: no tiene 6 campos");
                continue;
            }
            if (!int.TryParse(campos[0], out int idLibro) || idLibro < 1 || idLibro > 6)
            {
                Console.WriteLine($"Línea {numeroLinea}: el libro {campos[0]} no existe");
                continue;
            }
            if (!DateTime.TryParseExact(campos[3], "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime fecha))
            {
                Console.WriteLine($"Línea {numeroLinea}: fecha no válida ({campos[3]})");
                continue;
            }
            if (!int.TryParse(campos[4], out int dias) || (dias != 7 && dias != 14 && dias != 21))
            {
                Console.WriteLine($"Línea {numeroLinea}: días no válidos ({campos[4]})");
                continue;
            }
            if (!estados.Contains(campos[5]))
            {
                Console.WriteLine($"Línea {numeroLinea}: estado no válido ({campos[5]})");
                continue;
            }

            Prestamo p = new Prestamo(idLibro, campos[1], campos[2] == "S", fecha, dias);
            p.Estado = campos[5];
            prestamos.Add(p);
        }
        return prestamos;
    }

    public void InsertarLote(List<Prestamo> prestamos)
    {
        using SqlConnection conexion = Conexion.Conectar();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            foreach (Prestamo p in prestamos)
            {
                InsertarEn(p, conexion, transaccion);
            }
            transaccion.Commit();
        }
        catch (Exception ex)
        {
            transaccion.Rollback();
            throw new InvalidOperationException("Error al cargar: no se ha insertado ningún préstamo.", ex);
        }
    }
}
