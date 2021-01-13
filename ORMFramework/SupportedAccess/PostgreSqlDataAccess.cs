using Npgsql;
using ORMFramework.Community;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.SupportedAccess
{
    class PostgreSqlDataAccess : IDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public PostgreSqlDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public void CloseConnection(IDbConnection connection)
        {
            var oracleConnection = (NpgsqlConnection)connection;
            oracleConnection.Close();
            oracleConnection.Dispose();
        }

        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new NpgsqlCommand
            {
                CommandText = commandText,
                Connection = (NpgsqlConnection)connection,
                CommandType = commandType
            };
        }

        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new NpgsqlDataAdapter((NpgsqlCommand)command);
        }

        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            NpgsqlCommand SQLcommand = (NpgsqlCommand)command;
            return SQLcommand.CreateParameter();
        }
    }
}
