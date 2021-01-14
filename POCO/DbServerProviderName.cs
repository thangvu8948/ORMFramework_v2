using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO
{
    /// <summary>
    /// This class represents DbServerProviderName class.
    /// </summary>
    public static class DbServerProviderName
    {
        public static string MsSql = "System.Data.SqlClient";
        public static string MySql = "MySql.Data.MySqlClient";
        public static string Postgres = "Npgsql";
    }
}
