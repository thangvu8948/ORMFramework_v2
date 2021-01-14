using POCO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Readers
{
    public class SchemaReaderProvider
    {
        /// <summary>
        /// This class represents a provier class to get right schema reader based on database type.
        /// </summary>
        public static SchemaReader GetReader(DbServerType dbServerType)
        {
            SchemaReader schemaReader = null;
            switch (dbServerType)
            {
                case DbServerType.MsSql:
                    schemaReader = new SqlServerSchemaReader();
                    break;
                case DbServerType.MySql:
                    schemaReader = new MySqlSchemaReader();
                    break;
                case DbServerType.Postgres:
                    schemaReader = new PostgreSqlSchemaReader();
                    break;
                default: break;
            }

            return schemaReader;
        }
    }
}
