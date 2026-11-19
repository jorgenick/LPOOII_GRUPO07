using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
{
    public static class TrabajarVentas
    {
        public static int InsertarVenta(Venta v)
        {
            string sql = @"INSERT INTO Venta (NroFactura, FechaFactura, Legajo, ClienteDNI, CodProducto, Precio, Cantidad, Importe)
                           VALUES (@nroFactura, @fecha, @legajo, @dni, @codProducto, @precio, @cantidad, @importe)";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@nroFactura", v.NroFactura);
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = v.FechaFactura;
                cmd.Parameters.AddWithValue("@legajo", v.Legajo);
                cmd.Parameters.AddWithValue("@dni", v.DNI);
                cmd.Parameters.AddWithValue("@codProducto", v.CodigoProducto);
                cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = v.Precio;
                cmd.Parameters.AddWithValue("@cantidad", v.Cantidad);
                cmd.Parameters.Add("@importe", SqlDbType.Decimal).Value = v.Importe;

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static DataTable TraerVentas()
        {
            string sql = @"SELECT v.NroFactura, v.FechaFactura,
                                  LTRIM(RTRIM(v.ClienteDNI)) AS DNI,
                                  COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(c.Apellido, '') + ' ' + ISNULL(c.Nombre, ''))), ''),
                                           '(cliente no encontrado)') AS Cliente,
                                  RTRIM(ve.Apellido) + ', ' + RTRIM(ve.Nombre) AS Vendedor,
                                  RTRIM(p.Descripcion)                         AS Producto,
                                  v.Cantidad, v.Precio, v.Importe
                           FROM Venta v
                           LEFT JOIN Cliente  c  ON LTRIM(RTRIM(c.ClienteDNI)) = LTRIM(RTRIM(v.ClienteDNI))
                           LEFT JOIN Vendedor ve ON ve.Legajo      = v.Legajo
                           LEFT JOIN Producto p  ON p.CodProducto  = v.CodProducto
                           ORDER BY v.NroFactura DESC";

            DataTable dt = new DataTable();
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public static int GenerarSiguienteNroFactura()
        {
            string sql = "SELECT MAX(NroFactura) FROM Venta";
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                object resultado = cmd.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                    return 1;

                return Convert.ToInt32(resultado) + 1;
            }
        }

    }
}
