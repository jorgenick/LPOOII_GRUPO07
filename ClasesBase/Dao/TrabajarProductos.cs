using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
{
    public class TrabajarProductos
    {
        public static DataTable TraerProductos()
        {
            //Conexion
            SqlConnection cnn = DatabaseConnectionManager.getConnection();
            //Consulta
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT CodProducto, Categoria, Color, Descripcion, Precio, RutaImagen FROM Producto ORDER BY CodProducto";
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
            string sql = "INSERT INTO Producto (CodProducto, Categoria, Color, Descripcion, Precio, RutaImagen) " +
                         "VALUES (@CodProducto, @Categoria, @Color, @Descripcion, @Precio, @RutaImagen)";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CodProducto", oProducto.CodProducto);
                cmd.Parameters.AddWithValue("@Categoria", oProducto.Categoria);
                cmd.Parameters.AddWithValue("@Color", oProducto.Color);
                cmd.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = oProducto.Precio ?? 0;
                cmd.Parameters.AddWithValue("@RutaImagen", (object)oProducto.NombreArchivoImagen ?? DBNull.Value);

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
                p.NombreArchivoImagen = fila["RutaImagen"] == DBNull.Value ? null : fila["RutaImagen"].ToString();
                lista.Add(p);
            }
            return lista;
        }

        public static int ModificarProducto(Producto oProducto)
        {
            string sql = "UPDATE Producto SET Categoria = @Categoria, Color = @Color, " +
                         "Descripcion = @Descripcion, Precio = @Precio, RutaImagen = @RutaImagen " +
                         "WHERE CodProducto = @CodProducto";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@CodProducto", oProducto.CodProducto);
                cmd.Parameters.AddWithValue("@Categoria", oProducto.Categoria);
                cmd.Parameters.AddWithValue("@Color", oProducto.Color);
                cmd.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                cmd.Parameters.Add("@Precio", SqlDbType.Decimal).Value = oProducto.Precio ?? 0;
                cmd.Parameters.AddWithValue("@RutaImagen", (object)oProducto.NombreArchivoImagen ?? DBNull.Value);

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int EliminarProducto(string codProducto)
        {
            string sql = "DELETE FROM Producto WHERE CodProducto = @CodProducto";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@CodProducto", codProducto);
                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // metodo para generar un siguiente codigo de producto
        public static string GenerarSiguienteCodigo()
        {
            string sql = "SELECT MAX(CodProducto) FROM Producto";
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                object resultado = cmd.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                    return "P001"; //  si la tabla esta vacia se retorna P001

                string ultimoCodigo = resultado.ToString().Trim(); // extraemos ultimo codigo
                // extraemos solo parte numerica
                string parteNumerica = ultimoCodigo.StartsWith("P") ? ultimoCodigo.Substring(1) : ultimoCodigo;

                // incrementamos y se formatea el numero
                int numero;
                if (int.TryParse(parteNumerica, out numero))
                {
                    return "P" + (numero + 1).ToString("D3"); // Devuelve "P006"
                }

                return "P001";
            }
        }


    }
}