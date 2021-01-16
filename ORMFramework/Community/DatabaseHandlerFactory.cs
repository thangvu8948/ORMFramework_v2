﻿using ORMFramework.SupportedAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Community
{
    public class DatabaseHandlerFactory
    {
        private ConnectionStringSettings connectionStringSettings;

        public DatabaseHandlerFactory(string connectionStringName)
        {
            connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
        }

        public IDatabaseHandler CreateDatabase()
        {
            IDatabaseHandler database = null;

            switch (connectionStringSettings.ProviderName.ToLower())
            {
                case "system.data.sqlclient":
                    database = new MsSqlDataAccess(connectionStringSettings.ConnectionString);
                    break;
                case "mysql.data.mysqlclient":
                    database = new MySqlDataAccess(connectionStringSettings.ConnectionString);
                    break;
                case "npgsql":
                    database = new PostgreSqlDataAccess(connectionStringSettings.ConnectionString);
                    break;
                default:
                    break;
            }

            return database;
        }

        public string GetProviderName()
        {
            return connectionStringSettings.ProviderName;
        }
        public string GetJustInsertedID()
        {
            switch (connectionStringSettings.ProviderName.ToLower())
            {
                case "system.data.sqlclient":
                    return "SCOPE_IDENTITY()";
                case "mysql.data.mysqlclient":
                    return "LAST_INSERT_ID()";
                case "npgsql":
                    return "LASTVAL()";
                default:
                    return "";
            }
        }
    }
}
