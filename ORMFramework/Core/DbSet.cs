using ORMFramework.Community;
using ORMFramework.Enum;
using ORMFramework.Static;
using System;
using System.Collections;
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
        public IList Join<JT>(Tuple<string, string> frontToEnd, bool? forced = false) where JT : class
        {
            var tableCommand = string.Format(SqlQuery.selectSQL(table), "");
            var dataT = _dbManager.GetDataTable(tableCommand, CommandType.Text);
            string joinedTable = typeof(JT).Name;
            var joinedCommand = string.Format(SqlQuery.selectSQL(joinedTable), "");
            var dataJ = _dbManager.GetDataTable(joinedCommand, CommandType.Text);
            var dataSet = new DataSet();
            dataT.TableName = table;
            dataSet.Tables.Add(dataT);
            dataJ.TableName = joinedTable;
            dataSet.Tables.Add(dataJ);
            string constraint = "";
            bool flag = false; ;
            try
            {
                constraint = $"{table}_{joinedTable}";
                dataSet.Relations.Add(new DataRelation(constraint,
                       dataT.Columns[frontToEnd.Item1],
                       dataJ.Columns[frontToEnd.Item2]));
            }
            catch (Exception ex)
            {
                if (forced.Value)
                {
                    constraint = $"{joinedTable}_{table}";
                    dataSet.Relations.Add(new DataRelation(constraint,
                           dataJ.Columns[frontToEnd.Item2],
                           dataT.Columns[frontToEnd.Item1]));
                    flag = true;
                }
                else
                {
                    throw ex;
                }

            }

            var res = Helpers.CreateList(flag ? typeof(JT) : typeof(TEntity));
            foreach (DataRow item in !flag ? dataT.Rows : dataJ.Rows)
            {
                IEnumerable<DataRow> memberRows = item.GetChildRows(constraint);
                var temp = new object();
                var obj = Helpers.CreateList(!flag ? typeof(JT) : typeof(TEntity));
                if (!flag)
                {
                    temp = item.ToEntity<TEntity>();
                    foreach (var dr in memberRows)
                    {
                        obj.Add(dr.ToEntity<JT>());
                    }

                }
                temp = item.ToEntity<JT>();
                foreach (var dr in memberRows)
                {
                    obj.Add(dr.ToEntity<TEntity>());
                }
                var propertyNeedSetData = (!flag ? typeof(TEntity) : typeof(JT)).GetProperties()
                    .Where(x => x.PropertyType.ToString().ToLower().Contains(constraint.Split('_')[1].ToLower())).FirstOrDefault();
                propertyNeedSetData.SetValue(temp, obj);
                res.Add(temp);
            }
            //currentCommand += $"JOIN {joinedTable} j  ON t.{frontToEnd.Item1}= j.{frontToEnd.Item2} ";
            return res;
        }

    }
}
