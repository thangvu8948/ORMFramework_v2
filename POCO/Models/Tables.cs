using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class represents all tables in the database.
    /// </summary>
    public class Tables : List<Table>
    {
        /// <summary>
        /// Gets a table based on table name.
        /// </summary>
        public Table GetTable(string tableName)
        {
            return this.Single(x => string.Compare(x.Name, tableName, true) == 0);
        }

        /// <summary>
        /// Gets a table based on table indexed name.
        /// </summary>
        public Table this[string tableName]
        {
            get
            {
                return GetTable(tableName);
            }
        }
    }
}
