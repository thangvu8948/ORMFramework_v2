﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Readers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MySql.Data.MySqlClient;
    using System.Linq;
    using Models;

    /// <summary>
    /// This class represents a MySql database schema reader.
    /// Class inherits abstract class SchemaReader.
    /// </summary>
    public class MySqlSchemaReader : SchemaReader
    {
        /// <summary>
        /// MySql connection object.
        /// </summary>
        private MySqlConnection _connection;

        protected override void CreateConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        protected override Tables ReadTablesStructural()
        {
            var result = new Tables();//tất cả các tablelaayf đucợ
            using (var sqlCommand = new MySqlCommand(TableSql, _connection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {//chuyển từ read 1 dòng trong SQL sang kiểu Table, trả về Tables
                        Table tbl = new Table();
                        tbl.Name = reader["TABLE_NAME"].ToString();
                        tbl.Schema = reader["TABLE_SCHEMA"].ToString();
                        tbl.IsView = String.Compare(reader["TABLE_TYPE"].ToString(), "View", System.StringComparison.OrdinalIgnoreCase) == 0;//"BASE TABLE"
                        tbl.CleanName = Utils.CleanName(tbl.Name);//Rút gọn tên của table
                        tbl.ClassName = Utils.CleanNameToClassName(tbl.CleanName);
                        result.Add(tbl);
                    }
                }
            }
            return result;
        }

        protected override void ReadColumnsInTables(Tables result)
        {
            //loop again - but this time pull by table name
            foreach (var item in result)
            {
                item.Columns = LoadColumns(item);

                // Mark the primary key
                MarkPrimaryKey(item);

                //item.OuterKeys = LoadOuterKeys(item);
                //item.InnerKeys = LoadInnerKeys(item);

            }
        }

        private List<Column> LoadColumns(Table item)
        {
            //this will return everything for the DB
            var schema = _connection.GetSchema("COLUMNS");

            item.Columns = new List<Column>();

            //pull the columns from the schema
            var columns = schema.Select("TABLE_NAME='" + item.Name + "'");
            foreach (var row in columns)
            {
                Column col = new Column();
                col.Name = row["COLUMN_NAME"].ToString();
                col.PropertyName = Utils.CleanUp(col.Name);
                col.PropertyType = GetPropertyType(row);
                col.IsNullable = row["IS_NULLABLE"].ToString() == "YES";//true or false
                col.IsPk = row["COLUMN_KEY"].ToString() == "PRI";//true false
                col.IsAutoIncrement = row["extra"].ToString().ToLower().IndexOf("auto_increment", System.StringComparison.CurrentCultureIgnoreCase) >= 0;
                item.Columns.Add(col);
            }
            return item.Columns;
        }
        protected void MarkPrimaryKey(Table item)
        {
            // Only table with single primary key is allowed for this implementation
            // number of columns that are valid primary keys
            int pkeyCount = item.Columns.Count(x => x.IsPk);//đếm có bn khóa 9
            if (pkeyCount > 1)
            {
                foreach (var column in item.Columns)//mỗi bảng cho khóa = false
                {
                    column.IsPk = false;
                }
            }
        }

        /// <summary>
        /// Loads the reference keys info for the entire database.
        /// </summary>
        protected override void LoadReferencesKeysInfo(Tables tables)
        {
            var dataTable = _connection.GetSchema("Foreign Key Columns");

            var innerKeysDic = new Dictionary<string, List<Key>>();//<Tên cột, danh sách các khóa mà nó tham chiếu đến
            foreach (var item in tables)
            {
                item.OuterKeys = new List<Key>();
                item.InnerKeys = new List<Key>();

                //pull the foreign key details from the schema
                var columns = dataTable.Select("TABLE_NAME='" + item.Name + "'");
                foreach (DataRow row in columns)
                {
                    // Outer keys: Key relationship, outkey kiểm tra bảng đó tại thuộc tính này có tham chiếu đến ai không, lưu Reletionship đó và outer keys
                    var outerKey = new Key();
                    outerKey.Name = row["CONSTRAINT_NAME"].ToString();
                    var referencedTable = row["REFERENCED_TABLE_NAME"].ToString();//hiện tại là column role-> khóa ngoại đến bảng role (ID)
                    outerKey.ReferencedTableName = referencedTable;
                    outerKey.ReferencedTableColumnName = row["REFERENCED_COLUMN_NAME"].ToString();//ID
                    outerKey.ReferencingTableColumnName = row["COLUMN_NAME"].ToString();//Account(Role)
                    item.OuterKeys.Add(outerKey);//Cập nhật cột này tham chiếu ai vào thuộc tính OuterKeys

                    var innerKey = new Key();//key được tham chiếu đến, thêm vào dictionary
                    innerKey.Name = row["CONSTRAINT_NAME"].ToString();
                    innerKey.ReferencingTableName = row["TABLE_NAME"].ToString();//Account , Referencing bảng tham chiếu bảng khác
                    innerKey.ReferencedTableColumnName = row["REFERENCED_COLUMN_NAME"].ToString();//cột trong bảng được tham chiếu
                    innerKey.ReferencingTableColumnName = row["COLUMN_NAME"].ToString();

                    // add to inner keys references
                    if (innerKeysDic.ContainsKey(referencedTable))//nếu đã có trong dict, nghĩa là column này tham chiếu đến 1 bảng khác nữa, update lại
                    {
                        var innerKeys = innerKeysDic[referencedTable];
                        innerKeys.Add(innerKey);
                        innerKeysDic[referencedTable] = innerKeys;
                    }
                    else
                    {
                        var innerKeys = new List<Key>();
                        innerKeys.Add(innerKey);
                        innerKeysDic[referencedTable] = innerKeys;
                    }
                }
            }

            // add inner references to tables, với mỗi table, từ dictionary chứa tên bảng và các khóa của bảng khác mà nó trỏ tới,  lấy ds các khóa gán vào InnerKeys
            foreach (var item in tables)
            {
                if (innerKeysDic.ContainsKey(item.Name))
                {
                    var innerKeys = innerKeysDic[item.Name];
                    item.InnerKeys = innerKeys;
                }
            }
        }

        /// <summary>
        /// Reads the Schema returning all tables in the databse.
        /// </summary>
        //public override Tables ReadSchema(string connectionString)
        //{

        //    CreateConnection(connectionString);

        //    var result = ReadTablesStructural();

        //    ReadColumnsInTables(result);

        //    ////khóa ngoại
        //    LoadReferencesKeysInfo(result);//Convert khóa ngoại

        //    return result;
        //}

        //private List<Key> LoadOuterKeys(Table item)
        //{
        //    var dataTable = _connection.GetSchema("Foreign Key Columns");
        //    List<Key> OuterKeys = new List<Key>();

        //    //pull the foreign key details from the schema
        //    var columns = dataTable.Select("TABLE_NAME='" + item.Name + "'");
        //    foreach (DataRow row in columns)
        //    {
        //        // Outer keys: Key relationship, outkey kiểm tra bảng đó tại thuộc tính này có tham chiếu đến ai không, lưu Reletionship đó và outer keys
        //        var outerKey = new Key();
        //        outerKey.Name = row["CONSTRAINT_NAME"].ToString();
        //        var referencedTable = row["REFERENCED_TABLE_NAME"].ToString();//hiện tại là column role-> khóa ngoại đến bảng role (ID)
        //        outerKey.ReferencedTableName = referencedTable;
        //        outerKey.ReferencedTableColumnName = row["REFERENCED_COLUMN_NAME"].ToString();//ID
        //        outerKey.ReferencingTableColumnName = row["COLUMN_NAME"].ToString();//Account(Role)
        //        OuterKeys.Add(outerKey);//Cập nhật cột này tham chiếu ai vào thuộc tính OuterKeys
        //    }

        //    return OuterKeys;
        //}


        //private List<Key> LoadInnerKeys(Table table)
        //{
        //    Tables tables = ReadTablesStructural();
        //    var dataTable = _connection.GetSchema("Foreign Key Columns");
        //    List<Key> InnerKeys = new List<Key>();

        //    var innerKeysDic = new Dictionary<string, List<Key>>();//<Tên cột, danh sách các khóa mà nó tham chiếu đến
        //    foreach (var item in tables)
        //    {

        //        //pull the foreign key details from the schema
        //        var columns = dataTable.Select("TABLE_NAME='" + item.Name + "'");
        //        foreach (DataRow row in columns)
        //        {

        //            var innerKey = new Key();//key được tham chiếu đến, thêm vào dictionary
        //            innerKey.Name = row["CONSTRAINT_NAME"].ToString();
        //            innerKey.ReferencingTableName = row["TABLE_NAME"].ToString();//Account , Referencing bảng tham chiếu bảng khác
        //            innerKey.ReferencedTableColumnName = row["REFERENCED_COLUMN_NAME"].ToString();//cột trong bảng được tham chiếu
        //            innerKey.ReferencingTableColumnName = row["COLUMN_NAME"].ToString();

        //            var referencedTable = row["REFERENCED_TABLE_NAME"].ToString();
        //            // add to inner keys references
        //            if (innerKeysDic.ContainsKey(referencedTable))//nếu đã có trong dict, nghĩa là column này tham chiếu đến 1 bảng khác nữa, update lại
        //            {
        //                var innerKeys = innerKeysDic[referencedTable];
        //                innerKeys.Add(innerKey);
        //                innerKeysDic[referencedTable] = innerKeys;
        //            }
        //            else
        //            {
        //                var innerKeys = new List<Key>();
        //                innerKeys.Add(innerKey);
        //                innerKeysDic[referencedTable] = innerKeys;
        //            }
        //        }
        //    }

        //    // add inner references to tables, với mỗi table, từ dictionary chứa tên bảng và các khóa của bảng khác mà nó trỏ tới,  lấy ds các khóa gán vào InnerKeys
        //    if (innerKeysDic.ContainsKey(table.Name))
        //    {
        //        InnerKeys = innerKeysDic[table.Name];
        //    }
        //    return InnerKeys;
        //}



        /// <summary>
        /// Dispose resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the property type from the column.
        /// </summary>
        private static string GetPropertyType(DataRow row)//convert kiểu DL bên SQL sang code
        {
            bool bUnsigned = row["COLUMN_TYPE"].ToString().IndexOf("unsigned", System.StringComparison.CurrentCultureIgnoreCase) >= 0;
            string propType = "string";
            switch (row["DATA_TYPE"].ToString())
            {
                case "bigint":
                    propType = bUnsigned ? "ulong" : "long";
                    break;
                case "int":
                    propType = bUnsigned ? "uint" : "int";
                    break;
                case "smallint":
                    propType = bUnsigned ? "ushort" : "short";
                    break;
                case "guid":
                    propType = "Guid";
                    break;
                case "smalldatetime":
                case "date":
                case "datetime":
                case "timestamp":
                    propType = "DateTime";
                    break;
                case "float":
                    propType = "float";
                    break;
                case "double":
                    propType = "double";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    propType = "decimal";
                    break;
                case "bit":
                case "bool":
                case "boolean":
                    propType = "bool";
                    break;
                case "tinyint":
                    propType = bUnsigned ? "byte" : "sbyte";
                    break;
                case "image":
                case "binary":
                case "blob":
                case "mediumblob":
                case "longblob":
                case "varbinary":
                    propType = "byte[]";
                    break;

            }
            return propType;
        }

        /// <summary>
        /// Sql query to get table schema info, câu truy vấn gọi lên SQL thật để lấy ds bảng
        /// </summary>
        private const string TableSql = @"
			SELECT * 
			FROM information_schema.tables 
			WHERE (table_type='BASE TABLE' OR table_type='VIEW') AND TABLE_SCHEMA=DATABASE()
			";
    }
}
