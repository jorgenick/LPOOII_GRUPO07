using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    class DatabaseConnectionManager
    {   
        //private static string stringConnection = ClasesBase.Properties.Settings.Default.muebleriaConnectionString;
        private static string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DEV\LPOOII_GRUPO07\ClasesBase\muebleria.mdf;Integrated Security=True;Connect Timeout=30";
            


        public static SqlConnection getConnection()
        {
            return new SqlConnection(stringConnection);
        } 
    }
}
