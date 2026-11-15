using Ejercicio09.Datos;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
namespace Ejercicio09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ListarPorEdad(18);
            int id = InsertarPersona("Laura Sanz", "laura@correo.com", 25);
            Console.WriteLine($"Insertada con Id {id}");
        }
        static void Transferir(int idOrigen, int idDestino, decimal cantidad)
        {
            using SqlConnection conexion =Conexion.Conectar();
            using SqlTransaction transaccion =conexion.BeginTransaction();

            try
            {
  
                using SqlCommand comando = new SqlCommand("Select saldo From Cuentas where id=@id"
                    , conexion,transaccion);
                comando.Parameters.AddWithValue("@id", idOrigen);
                decimal? saldo1 = (decimal?)comando.ExecuteScalar();
                if (saldo1== null)
                {
                    throw new InvalidOperationException("La cuenta no existe.");
                }
                if (saldo1<cantidad)
                {
                    throw new InvalidOperationException("El saldo es insufuciente, no se puede realizar la transaccion.");
                }

                using SqlCommand comando2 = new SqlCommand("Update Cuentas set saldo=saldo-@cantidad where id=@id",conexion,transaccion);
                comando2.Parameters.AddWithValue("@id",idOrigen);
                comando2.Parameters.AddWithValue("@cantidad",cantidad);
                if (comando2.ExecuteNonQuery() != 1) {
                    throw new InvalidOperationException("[Error al restar el saldo del origen.]");
                }
                using SqlCommand comando3 = new SqlCommand("Update Cuentas set saldo=saldo+@cantidad where id=@id", conexion, transaccion);
                comando3.Parameters.AddWithValue("@id", idDestino);
                comando3.Parameters.AddWithValue("@cantidad", cantidad);
                if (comando3.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException("[Error al sumar el saldo del destino.]");
                }
                 transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();   
                throw; // y avisar a quien llamó
            }
        }

        public static void ListarPorEdad(int edadMinima)
        {
            using SqlConnection conexion = Conexion.Conectar();
            using SqlCommand comando = new SqlCommand("usp_PersonasPorEdad", conexion);  
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@EdadMinima",edadMinima);   
            using SqlDataReader lector= comando.ExecuteReader();
            while (lector.Read()) {
                Console.WriteLine(lector.GetString(lector.GetOrdinal("Nombre"))); // lector["Nombre"]
            }
        }


        static int InsertarPersona(string nombre, string email, int edad)
        {
            using SqlConnection conexion = Conexion.Conectar();
            using SqlCommand comando = new SqlCommand("usp_InsertarPersona", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@Nombre",nombre);
            comando.Parameters.AddWithValue("@Email", email);
            comando.Parameters.AddWithValue("@Edad", edad);

            SqlParameter pNuevoId = comando.Parameters.Add("@NuevoId", SqlDbType.Int);
            pNuevoId.Direction = ParameterDirection.Output;

            comando.ExecuteNonQuery();

            return (int)pNuevoId.Value;

        }

        
    }
}
