using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Readers
{
    using Models;
    using System.Data.Common;

    /// <summary>
    /// This abstract class for schema reading.
    /// </summary>
    public abstract class SchemaReader : IDisposable
    {
        /// <summary>
        /// Tạo kết nối csdl
        /// </summary>
        /// <param name="connectionString"></param>
        protected abstract void CreateConnection(string connectionString);

        /// <summary>
        /// DS Columns cho từng table trong tables
        /// </summary>
        /// <param name="result"></param>
        protected abstract void ReadColumnsInTables(Tables result);

        /// <summary>
        /// Cập nhật khóa cho bảng
        /// </summary>
        /// <param name="tables"></param>
        protected abstract void LoadReferencesKeysInfo(Tables tables);

        /// <summary>
        /// Tên bảng, View
        /// </summary>
        /// <returns></returns>
        protected abstract Tables ReadTablesStructural();

        /// <summary>
        /// Reads the Schema returning all tables in the databse.
        /// </summary>
        public virtual Tables ReadSchema(string connectionString)
        {
            CreateConnection(connectionString);

            var result = ReadTablesStructural();

            ReadColumnsInTables(result);

            LoadReferencesKeysInfo(result);


            return result;
        }







        /// <summary>
        /// Disposes of objects
        /// </summary>
        public abstract void Dispose();






        //public Tables LoadAllTable()
        //{
        //    return null;
        //}
        //public Table LoadColumn(Table table)
        //{
        //    return null;
        //}
        //public List<Key> GetOuterKeys(Table tbl)
        //{
        //    return null;
        //}
        //public List<Key> GetInnerKeys(Table tbl)
        //{
        //    return null;
        //}
    }
}
