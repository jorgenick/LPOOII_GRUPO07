using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

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
    }
}
