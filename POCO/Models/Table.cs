﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class represents a database table.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Initializes a new instance of the Table class
        /// </summary>
        public Table()
        {
            InnerKeys = new List<Key>();
            OuterKeys = new List<Key>();
        }

        /// <summary>
        /// Gets or sets list of columns.
        /// </summary>
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Gets or sets list of inner keys.
        /// </summary>
        public List<Key> InnerKeys { get; set; }

        /// <summary>
        /// Gets or sets list of outer keys.
        /// </summary>
        public List<Key> OuterKeys { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets database scheman name.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether table is a view.
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// Gets or sets cleaned table name.
        /// </summary>
        public string CleanName { get; set; }

        /// <summary>
        /// Gets or sets class name derived from table name.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets table sequence name.
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether table is to be ignored in mapping.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets the primary key column.
        /// </summary>
        public Column Pk
        {
            get
            {
                return this.Columns.SingleOrDefault(x => x.IsPk);
            }
        }

        /// <summary>
        /// Gets column object based on column name
        /// </summary>
        public Column GetColumn(string columnName)
        {
            return Columns.Single(x => System.String.Compare(x.Name, columnName, System.StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// Gets column object based on column name indexed.
        /// </summary>
        public Column this[string columnName]
        {
            get
            {
                return GetColumn(columnName);
            }
        }

    }
}
