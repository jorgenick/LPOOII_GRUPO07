using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
{
    public class TrabajarProveedores
    {
        public static DataTable TraerProveedoresDataTable()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT ProveedorCUIT, RazonSocial, Domicilio, Telefono FROM Proveedor WHERE Activo = 1 ORDER BY RazonSocial ASC";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                da.Fill(dt);
            }
            return dt;
        }


        public static ObservableCollection<Proveedor> TraerColeccionProveedores()
        {
            ObservableCollection<Proveedor> lista = new ObservableCollection<Proveedor>();
            DataTable dt = TraerProveedoresDataTable();

            foreach (DataRow fila in dt.Rows)
            {
                Proveedor p = new Proveedor();
                p.CUIT = fila["ProveedorCUIT"].ToString();
                p.RazonSocial = fila["RazonSocial"].ToString();
                p.Domicilio = fila["Domicilio"].ToString();
                p.Telefono = fila["Telefono"].ToString();
                lista.Add(p);
            }
            return lista;
        }

        public static int InsertarProveedor(Proveedor oProveedor)
        {
            // Si el CUIT existe dado de baja (Activo = 0), se reactiva con los datos nuevos
            string sqlReactivar = "UPDATE Proveedor SET RazonSocial = @RazonSocial, Domicilio = @Domicilio, Telefono = @Telefono, Activo = 1 " +
                                  "WHERE ProveedorCUIT = @ProveedorCUIT AND Activo = 0";
            string sql = "INSERT INTO Proveedor (ProveedorCUIT, RazonSocial, Domicilio, Telefono) " +
                         "VALUES (@ProveedorCUIT, @RazonSocial, @Domicilio, @Telefono)";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();

                using (SqlCommand cmdR = new SqlCommand(sqlReactivar, cnn))
                {
                    cmdR.Parameters.AddWithValue("@ProveedorCUIT", oProveedor.CUIT);
                    cmdR.Parameters.AddWithValue("@RazonSocial", oProveedor.RazonSocial);
                    cmdR.Parameters.AddWithValue("@Domicilio", oProveedor.Domicilio);
                    cmdR.Parameters.AddWithValue("@Telefono", oProveedor.Telefono);
                    int reactivados = cmdR.ExecuteNonQuery();
                    if (reactivados > 0) return reactivados;
                }

                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@ProveedorCUIT", oProveedor.CUIT);
                    cmd.Parameters.AddWithValue("@RazonSocial", oProveedor.RazonSocial);
                    cmd.Parameters.AddWithValue("@Domicilio", oProveedor.Domicilio);
                    cmd.Parameters.AddWithValue("@Telefono", oProveedor.Telefono);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static int ModificarProveedor(Proveedor oProveedor)
        {
            string sql = "UPDATE Proveedor SET RazonSocial = @RazonSocial, Domicilio = @Domicilio, Telefono = @Telefono " +
                         "WHERE ProveedorCUIT = @ProveedorCUIT";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@ProveedorCUIT", oProveedor.CUIT);
                cmd.Parameters.AddWithValue("@RazonSocial", oProveedor.RazonSocial);
                cmd.Parameters.AddWithValue("@Domicilio", oProveedor.Domicilio);
                cmd.Parameters.AddWithValue("@Telefono", oProveedor.Telefono);

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int EliminarProveedor(string cuit)
        {
            string sql = "UPDATE Proveedor SET Activo = 0 WHERE ProveedorCUIT = @ProveedorCUIT";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@ProveedorCUIT", cuit);

                cnn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

    }
}