using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
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

        public static ObservableCollection<Usuario> TraerUsuarios()
        {
            ObservableCollection<Usuario> lista = new ObservableCollection<Usuario>();
            string sql = "SELECT Usu_ID, Usu_Nombre, Usu_Password FROM Usuarios ORDER BY Usu_Nombre ASC";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cnn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow fila in dt.Rows)
                {
                    Usuario usu = new Usuario();
                    usu.Usu_ID = Convert.ToInt32(fila["Usu_ID"]);
                    usu.Usu_Nombre = fila["Usu_Nombre"].ToString();
                    usu.Usu_Password = fila["Usu_Password"].ToString();
                    // Cargamos la colección de roles de este usuario
                    usu.Roles = TraerRolesPorUsuario(usu.Usu_ID);
                    lista.Add(usu);
                }
            }
            return lista;
        }

        private static List<Rol> TraerRolesPorUsuario(int usuId)
        {
            List<Rol> roles = new List<Rol>();
            string sql = @"
                SELECT r.Rol_ID, r.Rol_Codigo, r.Rol_Descripcion
                FROM Roles r
                INNER JOIN Usuarios_Roles ur ON r.Rol_ID = ur.Usu_Rol
                WHERE ur.Usu_ID = @usuId";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cmd.Parameters.AddWithValue("@usuId", usuId);
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            Rol_ID = Convert.ToInt32(reader["Rol_ID"]),
                            Rol_Codigo = reader["Rol_Codigo"].ToString(),
                            Rol_Descripcion = reader["Rol_Descripcion"].ToString()
                        });
                    }
                }
            }
            return roles;
        }

        public static void AgregarUsuario(Usuario oUsuario)
        {
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();
                SqlTransaction transaccion = cnn.BeginTransaction();

                try
                {
                    string sqlUsuario = @"
                        INSERT INTO Usuarios (Usu_Nombre, Usu_Password) 
                        VALUES (@nombre, @pass);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdUsu = new SqlCommand(sqlUsuario, cnn, transaccion);
                    cmdUsu.Parameters.AddWithValue("@nombre", oUsuario.Usu_Nombre);
                    cmdUsu.Parameters.AddWithValue("@pass", oUsuario.Usu_Password);

                    int nuevoUsuId = Convert.ToInt32(cmdUsu.ExecuteScalar());

                    string sqlRol = "INSERT INTO Usuarios_Roles (Usu_ID, Usu_Rol) VALUES (@usuId, @rolId)";
                    foreach (Rol rol in oUsuario.Roles)
                    {
                        SqlCommand cmdRol = new SqlCommand(sqlRol, cnn, transaccion);
                        cmdRol.Parameters.AddWithValue("@usuId", nuevoUsuId);
                        cmdRol.Parameters.AddWithValue("@rolId", rol.Rol_ID);
                        cmdRol.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        public static void ModificarUsuario(Usuario oUsuario)
        {
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();
                SqlTransaction transaccion = cnn.BeginTransaction();

                try
                {
                    string sqlUsuario = "UPDATE Usuarios SET Usu_Nombre = @nombre, Usu_Password = @pass WHERE Usu_ID = @id";
                    SqlCommand cmdUsu = new SqlCommand(sqlUsuario, cnn, transaccion);
                    cmdUsu.Parameters.AddWithValue("@id", oUsuario.Usu_ID);
                    cmdUsu.Parameters.AddWithValue("@nombre", oUsuario.Usu_Nombre);
                    cmdUsu.Parameters.AddWithValue("@pass", oUsuario.Usu_Password);
                    cmdUsu.ExecuteNonQuery();

                    string sqlDeleteRoles = "DELETE FROM Usuarios_Roles WHERE Usu_ID = @id";
                    SqlCommand cmdDel = new SqlCommand(sqlDeleteRoles, cnn, transaccion);
                    cmdDel.Parameters.AddWithValue("@id", oUsuario.Usu_ID);
                    cmdDel.ExecuteNonQuery();

                    string sqlInsertRol = "INSERT INTO Usuarios_Roles (Usu_ID, Usu_Rol) VALUES (@usuId, @rolId)";
                    foreach (Rol rol in oUsuario.Roles)
                    {
                        SqlCommand cmdRol = new SqlCommand(sqlInsertRol, cnn, transaccion);
                        cmdRol.Parameters.AddWithValue("@usuId", oUsuario.Usu_ID);
                        cmdRol.Parameters.AddWithValue("@rolId", rol.Rol_ID);
                        cmdRol.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        public static void EliminarUsuario(int usuId)
        {
            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            {
                cnn.Open();
                SqlTransaction transaccion = cnn.BeginTransaction();

                try
                {
                    string sqlRoles = "DELETE FROM Usuarios_Roles WHERE Usu_ID = @id";
                    SqlCommand cmdRoles = new SqlCommand(sqlRoles, cnn, transaccion);
                    cmdRoles.Parameters.AddWithValue("@id", usuId);
                    cmdRoles.ExecuteNonQuery();

                    string sqlUsuario = "DELETE FROM Usuarios WHERE Usu_ID = @id";
                    SqlCommand cmdUsu = new SqlCommand(sqlUsuario, cnn, transaccion);
                    cmdUsu.Parameters.AddWithValue("@id", usuId);
                    cmdUsu.ExecuteNonQuery();

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }
    }
}
