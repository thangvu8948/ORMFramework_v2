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
       /** <h1>Get UPDATE SQL Query</h1>
 * <p>It is a generic function.It can be use for any DB Table</p>
 *
 * @author Debopam Pal, Software Developer, NIC, India.
 * @param tableName Table on which the UPDATE Operation will be performed.
 * @param columnValueMappingForSet List of Column & Value pair to Update.
 * @param columnValueMappingForCondition List of Column & Value pair for WHERE clause.
 * @return Final generated UPDATE SQL Statement.
 */
        public static string updateSQL(string tableName, Dictionary<string,
        object> columnValueMappingForSet, Dictionary<string, object> columnValueMappingForCondition)
        {
            StringBuilder updateQueryBuilder = new StringBuilder();

            /**
             * Removing column that holds NULL value or Blank value...
             */
            if (columnValueMappingForSet.Count != 0)
            {
                foreach (var entry in columnValueMappingForSet)
                {
                    if (entry.Value == null || entry.Value.Equals(""))
                    {
                        columnValueMappingForSet.Remove(entry.Key);
                    }
                }
            }

            /**
             * Removing column that holds NULL value or Blank value...
             */
            if (columnValueMappingForCondition.Count != 0)
            {
                foreach (var entry in columnValueMappingForCondition)
                {
                    if (entry.Value == null || entry.Value.Equals(""))
                    {
                        columnValueMappingForCondition.Remove(entry.Key);
                    }
                }
            }

            /* Making the UPDATE Query */
            updateQueryBuilder.Append("UPDATE");
            updateQueryBuilder.Append(" ").Append(tableName);
            updateQueryBuilder.Append(" SET");
            updateQueryBuilder.Append(" ");

            if (columnValueMappingForSet.Count != 0)
            {
                foreach (var entry in columnValueMappingForSet)
                {
                    updateQueryBuilder.Append(entry.Key).Append("=").Append(Helpers.format(entry.Value));
                    updateQueryBuilder.Append(",");
                }
            }

            updateQueryBuilder = new StringBuilder
                                  (updateQueryBuilder.Remove(updateQueryBuilder.Length - 1, 1).ToString());
            updateQueryBuilder.Append(" WHERE");
            updateQueryBuilder.Append(" ");

            if (columnValueMappingForCondition.Count != 0)
            {
                foreach (var entry in columnValueMappingForCondition)
                {
                    updateQueryBuilder.Append(entry.Key).Append("=").Append(entry.Value);
                    updateQueryBuilder.Append(",");
                }
            }

            updateQueryBuilder = new StringBuilder
                                  (updateQueryBuilder.Remove(updateQueryBuilder.Length - 1, 1).ToString());

            // Returning the generated UPDATE SQL Query as a String...
            return updateQueryBuilder.ToString();
        }
        public static string updateSQL(string tableName, Dictionary<string,
       object> columnValueMappingForSet, string condtion)
        {
            var temp = condtion.Split('=');
            columnValueMappingForSet.Remove($"{temp[0]}");
            StringBuilder updateQueryBuilder = new StringBuilder();

            /**
             * Removing column that holds NULL value or Blank value...
             */
            if (columnValueMappingForSet.Count != 0)
            {
                foreach (var entry in columnValueMappingForSet)
                {
                    if (entry.Value == null || entry.Value.Equals(""))
                    {
                        columnValueMappingForSet.Remove(entry.Key);
                    }
                }
            }


            /* Making the UPDATE Query */
            updateQueryBuilder.Append("UPDATE");
            updateQueryBuilder.Append(" ").Append(tableName);
            updateQueryBuilder.Append(" SET");
            updateQueryBuilder.Append(" ");

            if (columnValueMappingForSet.Count != 0)
            {
                foreach (var entry in columnValueMappingForSet)
                {
                    updateQueryBuilder.Append(entry.Key).Append("=").Append(Helpers.format(entry.Value));
                    updateQueryBuilder.Append(",");
                }
            }

            updateQueryBuilder = new StringBuilder
                                  (updateQueryBuilder.Remove(updateQueryBuilder.Length - 1, 1).ToString());
            updateQueryBuilder.Append(" WHERE");
            updateQueryBuilder.Append(" ");

            if (!String.IsNullOrEmpty(condtion))
            {
                updateQueryBuilder.Append(condtion);
            }

            //updateQueryBuilder = new StringBuilder
            //                      (updateQueryBuilder.Remove(updateQueryBuilder.Length - 1, 1).ToString());

            // Returning the generated UPDATE SQL Query as a String...
            return updateQueryBuilder.ToString();
        }
        /**
 * <h1>Get DELETE SQL Query</h1>
 * <p>It is a generic function. It can be use for any DB Table.</p>
 *
 * @author Debopam Pal, Software Developer, NIC, India.
 * @param tableName Table on which the DELETE Operation will be performed.
 * @param columnValueMappingForCondition List of Column & Value pair for WHERE clause.
 * @return Final generated DELETE SQL Statement.
 */
        public static string deleteSQL(string tableName, Dictionary<string,
        object> columnValueMappingForCondition)
        {
            StringBuilder deleteSQLBuilder = new StringBuilder();

            /**
             * Removing column that holds NULL value or Blank value...
             */
            if (columnValueMappingForCondition.Count != 0)
            {
                foreach (var entry in columnValueMappingForCondition)
                {
                    if (entry.Value == null || entry.Value.Equals(""))
                    {
                        columnValueMappingForCondition.Remove(entry.Key);
                    }
                }
            }

            /* Making the DELETE Query */
            deleteSQLBuilder.Append("DELETE FROM");
            deleteSQLBuilder.Append(" ").Append(tableName);
            deleteSQLBuilder.Append(" WHERE");
            deleteSQLBuilder.Append(" ");

            if (columnValueMappingForCondition.Count != 0)
            {
                foreach (var entry in columnValueMappingForCondition)
                {
                    deleteSQLBuilder.Append(entry.Key).Append("=").Append(entry.Value);
                    deleteSQLBuilder.Append(" AND ");
                }
            }

            deleteSQLBuilder = new StringBuilder(deleteSQLBuilder.Remove(deleteSQLBuilder.Length - 5, 5).ToString());

            // Returning the generated DELETE SQL Query as a String...
            return deleteSQLBuilder.ToString();
        }
        public static string deleteSQL(string tableName, string condtion)
        {
            StringBuilder deleteSQLBuilder = new StringBuilder();


            /* Making the DELETE Query */
            deleteSQLBuilder.Append("DELETE FROM");
            deleteSQLBuilder.Append(" ").Append(tableName);
            deleteSQLBuilder.Append(" WHERE");
            deleteSQLBuilder.Append(" ");

            if (!String.IsNullOrEmpty(condtion))
            {
                deleteSQLBuilder.Append(condtion);
            }

            //deleteSQLBuilder = new StringBuilder(deleteSQLBuilder.Remove(deleteSQLBuilder.Length - 5, 5).ToString());

            // Returning the generated DELETE SQL Query as a String...
            return deleteSQLBuilder.ToString();
        }
    }
}
