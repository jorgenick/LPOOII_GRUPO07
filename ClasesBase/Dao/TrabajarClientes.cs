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
    public class TrabajarClientes
    {

        public static ObservableCollection<Cliente> TraerClientes()
        {
            ObservableCollection<Cliente> lista = new ObservableCollection<Cliente>();

            DataTable dt = TraerClientesDataTable();

            foreach (DataRow fila in dt.Rows)
            {
                Cliente cliente = new Cliente();
                cliente.Dni = fila["ClienteDNI"].ToString();
                cliente.Apellido = fila["Apellido"].ToString();
                cliente.Nombre = fila["Nombre"].ToString();
                cliente.Direccion = fila["Direccion"].ToString();
                lista.Add(cliente);
            }
            return lista;
        }

        public static DataTable TraerClientesDataTable()
        {
            DataTable dataTableClientes = new DataTable();
            string sql = "SELECT ClienteDNI, Apellido, Nombre, Direccion FROM Cliente WHERE Activo = 1 ORDER BY Apellido ASC";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())

            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                da.Fill(dataTableClientes);

            }
            return dataTableClientes;
        }

        // Agregar un Cliente a la Base de Datos
        public static void AgregarCliente(Cliente oCliente)
        {
            // Si el DNI existe dado de baja (Activo = 0), se reactiva con los datos nuevos
            string sqlReactivar = "UPDATE Cliente SET Apellido = @apellido, Nombre = @nombre, Direccion = @direccion, Activo = 1 " +
                                  "WHERE ClienteDNI = @dni AND Activo = 0";
            string sql = "INSERT INTO Cliente (ClienteDNI, Apellido, Nombre, Direccion) VALUES (@dni, @apellido, @nombre, @direccion)";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();

                using (SqlCommand cmdR = new SqlCommand(sqlReactivar, cnn))
                {
                    cmdR.Parameters.AddWithValue("@dni", oCliente.Dni);
                    cmdR.Parameters.AddWithValue("@apellido", oCliente.Apellido);
                    cmdR.Parameters.AddWithValue("@nombre", oCliente.Nombre);
                    cmdR.Parameters.AddWithValue("@direccion", oCliente.Direccion);
                    if (cmdR.ExecuteNonQuery() > 0) return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@dni", oCliente.Dni);
                    cmd.Parameters.AddWithValue("@apellido", oCliente.Apellido);
                    cmd.Parameters.AddWithValue("@nombre", oCliente.Nombre);
                    cmd.Parameters.AddWithValue("@direccion", oCliente.Direccion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Modificar un Cliente en la Base de Datos
        public static void ModificarCliente(Cliente oCliente)
        {
            string sql = "UPDATE Cliente SET Apellido = @apellido, Nombre = @nombre, Direccion = @direccion WHERE ClienteDNI = @dni";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@dni", oCliente.Dni);
                cmd.Parameters.AddWithValue("@apellido", oCliente.Apellido);
                cmd.Parameters.AddWithValue("@nombre", oCliente.Nombre);
                cmd.Parameters.AddWithValue("@direccion", oCliente.Direccion);

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //  Baja logica de un Cliente (Activo = 0)
        public static void EliminarCliente(string dni)
        {
            string sql = "UPDATE Cliente SET Activo = 0 WHERE ClienteDNI = @dni";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}