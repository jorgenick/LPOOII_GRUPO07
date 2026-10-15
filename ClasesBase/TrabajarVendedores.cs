using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarVendedores
    {
        private static string Cadena()
        {
            return ClasesBase.Properties.Settings.Default.muebleriaConnectionString;
        }

        public static ObservableCollection<Vendedor> TraerColeccionVendedores()
        {
            ObservableCollection<Vendedor> lista = new ObservableCollection<Vendedor>();
            string sql = "SELECT Legajo, Apellido, Nombre FROM Vendedor ORDER BY Apellido, Nombre";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
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
            string sql = "INSERT INTO Vendedor (Legajo, Apellido, Nombre) " +
                         "VALUES (@Legajo, @Apellido, @Nombre)";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@Legajo", oVendedor.Legajo);
                cmd.Parameters.AddWithValue("@Apellido", oVendedor.Apellido);
                cmd.Parameters.AddWithValue("@Nombre", oVendedor.Nombre);

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int ModificarVendedor(Vendedor oVendedor)
        {
            string sql = "UPDATE Vendedor SET Apellido = @Apellido, Nombre = @Nombre " +
                         "WHERE Legajo = @Legajo";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
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
            string sql = "DELETE FROM Vendedor WHERE Legajo = @Legajo";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@Legajo", legajo);
                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}