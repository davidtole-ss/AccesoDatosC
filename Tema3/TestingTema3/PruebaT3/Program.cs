using Microsoft.Data.SqlClient;
namespace PruebaT3
{
    internal class Program
    {
        private const string CadenaConexion = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=AccesoDatos;Integrated Security=true;";
        static void Main(string[] args)
        {
            try
            {
                using SqlConnection conexion = Conectar();
                using SqlCommand comando = new SqlCommand("SELECT Id, Nombre, Email FROM Personas ORDER BY Nombre", conexion);
                using SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    int id = lector.GetInt32(0);    // columna 0 del SELECT: Id
                    string nombre = lector.GetString(1);   // columna 1: Nombre
                    string email = lector.GetString(2);   // columna 2: Email
                    Console.WriteLine($"{id,3} | {nombre,-15} | {email}");
                }
                Console.WriteLine($"Estado:   {conexion.State}");          // Open
                Console.WriteLine($"Servidor: {conexion.DataSource}");     // (localdb)\MSSQLLocalDB
                Console.WriteLine($"Base:     {conexion.Database}");       // AccesoDatos
                Console.WriteLine($"Versión:  {conexion.ServerVersion}");  // 16.00.xxxx
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ {ex.Message}: {ex.InnerException?.Message}");
            }

        }
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
                throw new InvalidOperationException("Error al conectar", ex);
            }
        }
    }

}

