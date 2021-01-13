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
        /**
* <h1>Get INSERT SQL Query</h1>
* <p>It is a generic function. It can be use for any DB Table</p>
*
* @author Debopam Pal, Software Developer, NIC, India.
* @param tableName Table on which the INSERT Operation will be performed.
* @param columnValueMappingForInsert List of Column & Value pair to Insert.
* @return Final generated INSERT SQL Statement.
*/
        public static string insertSQL(string tableName, Dictionary<string, object> columnValueMappingForInsert)
        {
            StringBuilder insertSQLBuilder = new StringBuilder();

            /**
             * Removing column that holds NULL value or Blank value...
             */
            if (columnValueMappingForInsert.Count() != 0)
            {
                foreach (var item in columnValueMappingForInsert)
                {
                    if (item.Value == null || item.Value.Equals(""))
                    {
                        columnValueMappingForInsert.Remove(item.Key);
                    }
                }
            }

            /* Making the INSERT Query... */
            insertSQLBuilder.Append("INSERT INTO");
            insertSQLBuilder.Append(" ").Append(tableName);
            insertSQLBuilder.Append("(");

            if (columnValueMappingForInsert.Count != 0)
            {
                foreach (var item in columnValueMappingForInsert)
                {
                    insertSQLBuilder.Append(item.Key);
                    insertSQLBuilder.Append(",");
                }
            }

            insertSQLBuilder = new StringBuilder(insertSQLBuilder.Remove(insertSQLBuilder.Length - 1, 1).ToString());
            insertSQLBuilder.Append(")");
            insertSQLBuilder.Append(" VALUES");
            insertSQLBuilder.Append("(");

            if (columnValueMappingForInsert.Count != 0)
            {
                foreach (var item in columnValueMappingForInsert)
                {
                    insertSQLBuilder.Append(Helpers.format(item.Value));
                    insertSQLBuilder.Append(",");
                }
            }

            insertSQLBuilder = new StringBuilder(insertSQLBuilder.Remove(insertSQLBuilder.Length - 1, 1).ToString());
            insertSQLBuilder.Append(")");

            // Returning the generated INSERT SQL Query as a String...
            return insertSQLBuilder.ToString();
        }
    }
}
