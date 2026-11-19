using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ClasesBase
{
    public class TrabajarUsuarios
    {
        public static List<string> ValidarLogin(string nombreUsuario, string password)
        {
            List<string> codigosRoles = new List<string>();

            using (SqlConnection conexion = DatabaseConnectionManager.getConnection())
            {
                // Ahora traemos el código invariable (ej. 'ADM')
                string query = @"
                SELECT r.Rol_Codigo 
                FROM Usuarios u
                INNER JOIN Usuarios_Roles ur ON u.Usu_ID = ur.Usu_ID
                INNER JOIN Roles r ON ur.Usu_Rol = r.Rol_ID
                WHERE u.Usu_Nombre = @user AND u.Usu_Password = @pass";

                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@user", nombreUsuario);
                comando.Parameters.AddWithValue("@pass", password);

                conexion.Open();
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        codigosRoles.Add(reader["Rol_Codigo"].ToString());
                    }
                }
            }
            return codigosRoles;
        }
    }
}
