using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
{
    public class TrabajarVendedores
    {

        public static ObservableCollection<Vendedor> TraerColeccionVendedores()
        {
            ObservableCollection<Vendedor> lista = new ObservableCollection<Vendedor>();
            string sql = "SELECT Legajo, Apellido, Nombre FROM Vendedor WHERE Activo = 1 ORDER BY Apellido, Nombre";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow fila in dt.Rows)
                {
                    Vendedor v = new Vendedor();
                    v.Legajo = fila["Legajo"].ToString();
                    v.Apellido = fila["Apellido"].ToString();
                    v.Nombre = fila["Nombre"].ToString();
                    lista.Add(v);
                }
            }
            return lista;
        }

        public static int InsertarVendedor(Vendedor oVendedor)
        {
            // Si el legajo existe dado de baja (Activo = 0), se reactiva con los datos nuevos
            string sqlReactivar = "UPDATE Vendedor SET Apellido = @Apellido, Nombre = @Nombre, Activo = 1 " +
                                  "WHERE Legajo = @Legajo AND Activo = 0";
            string sql = "INSERT INTO Vendedor (Legajo, Apellido, Nombre) " +
                         "VALUES (@Legajo, @Apellido, @Nombre)";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();

                using (SqlCommand cmdR = new SqlCommand(sqlReactivar, cnn))
                {
                    cmdR.Parameters.AddWithValue("@Legajo", oVendedor.Legajo);
                    cmdR.Parameters.AddWithValue("@Apellido", oVendedor.Apellido);
                    cmdR.Parameters.AddWithValue("@Nombre", oVendedor.Nombre);
                    int reactivados = cmdR.ExecuteNonQuery();
                    if (reactivados > 0) return reactivados;
                }

                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@Legajo", oVendedor.Legajo);
                    cmd.Parameters.AddWithValue("@Apellido", oVendedor.Apellido);
                    cmd.Parameters.AddWithValue("@Nombre", oVendedor.Nombre);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static int ModificarVendedor(Vendedor oVendedor)
        {
            string sql = "UPDATE Vendedor SET Apellido = @Apellido, Nombre = @Nombre " +
                         "WHERE Legajo = @Legajo";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@Legajo", oVendedor.Legajo);
                cmd.Parameters.AddWithValue("@Apellido", oVendedor.Apellido);
                cmd.Parameters.AddWithValue("@Nombre", oVendedor.Nombre);

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int EliminarVendedor(string legajo)
        {
            string sql = "UPDATE Vendedor SET Activo = 0 WHERE Legajo = @Legajo";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@Legajo", legajo);
                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}