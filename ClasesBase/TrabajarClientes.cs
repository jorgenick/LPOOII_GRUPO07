using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarClientes
    {

        private static string Cadena()
        {
            return ClasesBase.Properties.Settings.Default.muebleriaConnectionString;
        }

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
            string sql = "SELECT ClienteDNI, Apellido, Nombre, Direccion FROM Cliente ORDER BY Apellido ASC";

            using (SqlConnection cnn = new SqlConnection(Cadena()))

            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                da.Fill(dataTableClientes);

            }
            return dataTableClientes;
        }

        // Agregar un Cliente a la Base de Datos
        public static void AgregarCliente(Cliente oCliente)
        {
            string sql = "INSERT INTO Cliente (ClienteDNI, Apellido, Nombre, Direccion) VALUES (@dni, @apellido, @nombre, @direccion)";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
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

        // Modificar un Cliente en la Base de Datos
        public static void ModificarCliente(Cliente oCliente)
        {
            string sql = "UPDATE Cliente SET Apellido = @apellido, Nombre = @nombre, Direccion = @direccion WHERE ClienteDNI = @dni";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
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

        //  Eliminar un Cliente de la Base de Datos
        public static void EliminarCliente(string dni)
        {
            string sql = "DELETE FROM Cliente WHERE ClienteDNI = @dni";

            using (SqlConnection cnn = new SqlConnection(Cadena()))
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
