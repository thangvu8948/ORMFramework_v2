using ORMFramework.Community;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //currentCommand = SqlQuery.selectSQL(table);
        }
    }
}
