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


    }
}
