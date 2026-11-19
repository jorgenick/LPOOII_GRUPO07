using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;

namespace ClasesBase.Dao
{
    public class TrabajarRoles
    {

        public static ObservableCollection<Rol> TraerRoles()
        {
            ObservableCollection<Rol> lista = new ObservableCollection<Rol>();
            string sql = "SELECT Rol_ID, Rol_Codigo, Rol_Descripcion FROM Roles ORDER BY Rol_Descripcion ASC";

            using (SqlConnection cnn = DatabaseConnectionManager.getConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Rol
                        {
                            Rol_ID = Convert.ToInt32(reader["Rol_ID"]),
                            Rol_Codigo = reader["Rol_Codigo"].ToString(),
                            Rol_Descripcion = reader["Rol_Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}
