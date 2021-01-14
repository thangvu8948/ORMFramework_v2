using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Models
{
    /// <summary>
    /// This class represents a table column.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets name of the column name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets name of the class property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets name of the class property type.
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is a primary key column.
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column is an autocrement.
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether column should be ignored in mapping.
        /// </summary>
        public bool Ignore { get; set; }
    }
}
