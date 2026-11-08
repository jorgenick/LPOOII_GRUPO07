using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public static class TrabajarVentas
    {
        private static string Cadena()
        {
            return ClasesBase.Properties.Settings.Default.muebleriaConnectionString;
        }

        public static int InsertarVenta(Venta v)
        {
            string sql = @"INSERT INTO Venta (NroFactura, FechaFactura, Legajo, ClienteDNI, CodProducto, Precio, Cantidad, Importe)
                           VALUES (@nroFactura, @fecha, @legajo, @dni, @codProducto, @precio, @cantidad, @importe)";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@nroFactura", v.NroFactura);
                cmd.Parameters.AddWithValue("@fecha", v.FechaFactura);
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
    }
}
