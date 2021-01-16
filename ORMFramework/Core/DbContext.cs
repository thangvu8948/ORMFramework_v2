using ORMFramework.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Core
{
    public abstract class DbContext
    {
        protected DBManager dBManager;
        protected string name;
        public DbContext(string _name)
        {
            name = _name;
            dBManager = DBManager.getInstance(_name);
        }
        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return new DbSet<TEntity>(dBManager);
        }
    }
}
