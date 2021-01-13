using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Static
{
    public static class SqlQuery
    {
        public static string selectSQL(string tableName)
        {
            return string.Format("SELECT {0} *   FROM {1} t", "{0}", tableName);
            //return $"SELECT {0} *   FROM {tableName} t  ";
        }
    }
}
