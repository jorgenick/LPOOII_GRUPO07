using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace ClasesBase
{
    public class TrabajarProductos
    {
        public static DataTable TraerProductos()
        {
            //Conexion
            SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.muebleriaConnectionString);

            //Consulta
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CodProducto, Categoria, Color, Descripcion, Precio FROM Producto ORDER BY CodProducto";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            //DataAdapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public static int InsertarProducto(Producto oProducto)
        {
            string sql = "INSERT INTO Producto (CodProducto, Categoria, Color, Descripcion, Precio) " +
                         "VALUES (@CodProducto, @Categoria, @Color, @Descripcion, @Precio)";

            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.muebleriaConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CodProducto", oProducto.CodProducto);
                cmd.Parameters.AddWithValue("@Categoria", oProducto.Categoria);
                cmd.Parameters.AddWithValue("@Color", oProducto.Color);
                cmd.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = oProducto.Precio;

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }//insertar

        public static ObservableCollection<Producto> TraerColeccionProductos()
        {
            ObservableCollection<Producto> lista = new ObservableCollection<Producto>();
            DataTable dt = TraerProductos();

            foreach (DataRow fila in dt.Rows)
            {
                Producto p = new Producto();
                p.CodProducto = fila["CodProducto"].ToString();
                p.Categoria = fila["Categoria"].ToString();
                p.Color = fila["Color"].ToString();
                p.Descripcion = fila["Descripcion"].ToString();
                p.Precio = Convert.ToDecimal(fila["Precio"]);
                lista.Add(p);
            }
            return lista;
        }

        public static int ModificarProducto(Producto oProducto)
        {
            string sql = "UPDATE Producto SET Categoria = @Categoria, Color = @Color, " +
                         "Descripcion = @Descripcion, Precio = @Precio " +
                         "WHERE CodProducto = @CodProducto";

            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.muebleriaConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@CodProducto", oProducto.CodProducto);
                cmd.Parameters.AddWithValue("@Categoria", oProducto.Categoria);
                cmd.Parameters.AddWithValue("@Color", oProducto.Color);
                cmd.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = oProducto.Precio;

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int EliminarProducto(string codProducto)
        {
            string sql = "DELETE FROM Producto WHERE CodProducto = @CodProducto";

            using (SqlConnection cnn = new SqlConnection(ClasesBase.Properties.Settings.Default.muebleriaConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@CodProducto", codProducto);
                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

    }
}
