using ORMFramework.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Community
{
    public class DBManager
    {
        private static DBManager instance;
        private DatabaseHandlerFactory dbFactory;
        private IDatabaseHandler database;
        private string providerName;

        public static DBManager getInstance(string connectionStringName)
        {
            if (instance == null)
            {
                instance = new DBManager(connectionStringName);
            }
            return instance;
        }
        private DBManager(string connectionStringName)
        {
            dbFactory = new DatabaseHandlerFactory(connectionStringName);
            database = dbFactory.CreateDatabase();
            providerName = dbFactory.GetProviderName();
        }
        public string GetJustInsertedID()
        {
            return dbFactory.GetJustInsertedID();
        }
        public IDbConnection GetDatabasecOnnection()
        {
            return database.CreateConnection();
        }

        public void CloseConnection(IDbConnection connection)
        {
            database.CloseConnection(connection);
        }

        public IDbDataParameter CreateParameter(string name, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(providerName, name, value, dbType, ParameterDirection.Input);
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(providerName, name, size, value, dbType, ParameterDirection.Input);
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return DataParameterManager.CreateParameter(providerName, name, size, value, dbType, direction);
        }

        public DataTable GetDataTable(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);
                    return dataset.Tables[0].Copy();
                }
            }
        }

        public DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);

                    return dataset;
                }
            }
        }

        public IDataReader GetDataReader(string commandText, CommandType commandType, out IDbConnection connection, IDbDataParameter[] parameters = null)
        {
            IDataReader reader = null;
            connection = database.CreateConnection();
            connection.Open();

            var command = database.CreateCommand(commandText, commandType, connection);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            reader = command.ExecuteReader();

            return reader;
        }

        public IEnumerable<T> Query<T>(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    return Helpers.DataReaderMapToList<T>(command.ExecuteReader());
                }
            }
        }
        public bool Delete(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    int affected = command.ExecuteNonQuery();
                    if (affected >= 1) return true;
                }
            }
            return false;
        }

        //public void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        //{
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public int Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out int lastId)
        //{
        //    lastId = 0;
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            object newId = command.ExecuteScalar();
        //            lastId = Convert.ToInt32(newId);
        //        }
        //    }

        //    return lastId;
        //}

        public long Insert(string commandText, CommandType commandType, out long lastId, IDbDataParameter[] parameters = null)
        {
            lastId = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    object newId = command.ExecuteScalar();
                    try
                    {
                        lastId = Convert.ToInt64(newId);
                    }
                    catch (Exception)
                    {

                        lastId = 0;
                    }

                }
            }

            return lastId;
        }

        //public void InsertWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        //{
        //    IDbTransaction transactionScope = null;
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();
        //        transactionScope = connection.BeginTransaction();

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            try
        //            {
        //                command.ExecuteNonQuery();
        //                transactionScope.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transactionScope.Rollback();
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        //public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        //{
        //    IDbTransaction transactionScope = null;
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();
        //        transactionScope = connection.BeginTransaction(isolationLevel);

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            try
        //            {
        //                command.ExecuteNonQuery();
        //                transactionScope.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transactionScope.Rollback();
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        public bool Update(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    int affected = command.ExecuteNonQuery();
                    if (affected >= 1) return true;
                }
            }
            return false;
        }

        //public void UpdateWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        //{
        //    IDbTransaction transactionScope = null;
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();
        //        transactionScope = connection.BeginTransaction();

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            try
        //            {
        //                command.ExecuteNonQuery();
        //                transactionScope.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transactionScope.Rollback();
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        //public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        //{
        //    IDbTransaction transactionScope = null;
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();
        //        transactionScope = connection.BeginTransaction(isolationLevel);

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            try
        //            {
        //                command.ExecuteNonQuery();
        //                transactionScope.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transactionScope.Rollback();
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        //public object GetScalarValue(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        //{
        //    using (var connection = database.CreateConnection())
        //    {
        //        connection.Open();

        //        using (var command = database.CreateCommand(commandText, commandType, connection))
        //        {
        //            if (parameters != null)
        //            {
        //                foreach (var parameter in parameters)
        //                {
        //                    command.Parameters.Add(parameter);
        //                }
        //            }

        //            return command.ExecuteScalar();
        //        }
        //    }
        //}
    }
}
