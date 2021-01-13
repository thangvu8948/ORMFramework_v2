using MySql.Data.MySqlClient;
using ORMFramework.Community;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.SupportedAccess
{
    class MySqlDataAccess : IDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public MySqlDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public void CloseConnection(IDbConnection connection)
        {
            var mysqlConnection = (MySqlConnection)connection;
            mysqlConnection.Close();
            mysqlConnection.Dispose();
        }

        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new MySqlCommand
            {
                CommandText = commandText,
                Connection = (MySqlConnection)connection,
                CommandType = commandType
            };
        }

        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new MySqlDataAdapter((MySqlCommand)command);
        }

        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            MySqlCommand SQLcommand = (MySqlCommand)command;
            return SQLcommand.CreateParameter();
        }
    }
}
