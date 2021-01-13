using ORMFramework.Community;
using ORMFramework.Enum;
using ORMFramework.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Core
{
    public class DbSet<TEntity> where TEntity : class
    {
        string table = typeof(TEntity).Name;
        private DBManager _dbManager;
        string currentCommand = "";
        public DbSet(DBManager dBManager)
        {
            _dbManager = dBManager;
            currentCommand = SqlQuery.selectSQL(table);
        }
        public long Insert(object item)
        {
            long insertedId;
            var sqlCommand = SqlQuery.insertSQL(table, item.ToDictionary());
            sqlCommand += "; SELECT LAST_INSERT_ID();";
            _dbManager.Insert(sqlCommand, CommandType.Text, out insertedId);
            return insertedId;
        }
        public bool Update(Expression<Func<TEntity, bool>> expression, object item)
        {
            var condition = Helpers.GetWhereClause<TEntity>(expression);
            var sqlCommand = SqlQuery.updateSQL(table, item.ToDictionary(), condition);
            return _dbManager.Update(sqlCommand, CommandType.Text);
        }
        public bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            var condition = Helpers.GetWhereClause<TEntity>(expression);
            var sqlCommand = SqlQuery.deleteSQL(table, condition);
            return _dbManager.Delete(sqlCommand, CommandType.Text);
        }
        public DbSet<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            var condition = Helpers.GetWhereClause<TEntity>(expression);
            currentCommand += $" WHERE t.{condition} ";
            return this;
        }
        public void GroupBy()
        {

        }
        public void Having()
        {

        }
        public DbSet<TEntity> OrderBy(string field, Order order = Order.ASC)
        {
            currentCommand += $" ORDER BY t.{field}  {order.ToString()} ";
            return this;
        }
        public DbSet<TEntity> Top(int number)
        {
            currentCommand = string.Format(currentCommand, $" TOP {number} ");
            return this;
        }
        public IEnumerable<TEntity> Excute()
        {
            var clone = string.Format(currentCommand, "");
            currentCommand = SqlQuery.selectSQL(table);
            return _dbManager.Query<TEntity>(clone, CommandType.Text);

        }

    }
}
