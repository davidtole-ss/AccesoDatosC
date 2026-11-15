using Microsoft.Data.SqlClient;

namespace BibliotecaBarrio.Datos;

public static class Conexion
{
    private const string CadenaConexion =
        @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=BibliotecaBarrio;Integrated Security=true;";

    public static SqlConnection Conectar()
    {
        var conexion = new SqlConnection(CadenaConexion);
        try
        {
            conexion.Open();
            return conexion;
        }
        catch (SqlException ex)
        {
            conexion.Dispose();
            throw new InvalidOperationException(
                "No se pudo conectar con la base de datos BibliotecaBarrio", ex);
        }
    }
}
