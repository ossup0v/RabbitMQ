using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RabbitMQ.Common;
using RabbitMQ.Connections.Models;

namespace RabbitMQ.Connections.MySQL
{
  public sealed class MySqlServerDatabase : IServerDatabase, IDisposable
  {
    private readonly Func<ConnectionKind, IDbConnection> _dbConnections;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public MySqlServerDatabase(Func<ConnectionKind, IDbConnection> dbConnections)
    {
      _dbConnections = dbConnections;
    }

    public async Task CreateTest(TestDBModel model)
    {
      using (var connection = _dbConnections(ConnectionKind.Create))
      using (var command = GetCommand(connection, _CreateTestSql))
      using (var tx = connection.BeginTransaction())
      {
        command.Transaction = (MySqlTransaction) tx;

        AddParameter(command, "@SomeIntData", DbType.Int32, model.SomeIntData);
        AddParameter(command, "@SomeTextData", DbType.String, model.SomeTextData);
        AddParameter(command, "@SomeSerializableData", DbType.Object, model.SomeSerializableData);
        await command.ExecuteScalarAsync(_cancellationTokenSource.Token);
        tx.Commit();
      }
    }

    public void DeleteTest(int uId)
    {
      throw new NotImplementedException();
    }

    public TestDBModel ReadTest(int uId)
    {
      throw new NotImplementedException();
    }

    public void UpdateTest(int uId, TestDBModel model)
    {
      throw new NotImplementedException();
    }

    #region Auxiliary commands

    private MySqlCommand CreateCommand(IDbConnection connection)
    {
      var command = new MySqlCommand();
      command.Connection = (MySqlConnection)connection;
      return command;
    }

    private MySqlCommand GetCommand(IDbConnection connection, string sql)
    {
      var command = CreateCommand(connection);
      command.CommandText = sql;
      command.CommandTimeout = Configurator.Config.DbCommandTimeOut;
      return command;
    }

    private void AddParameter(MySqlCommand command, string parameterName, DbType dbType, object value)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = dbType;
      parameter.Value = value;
      //(Osipov) todo ?
      command.Parameters.Add(parameter);
    }


    #endregion

    public void Dispose()
    {
      _cancellationTokenSource?.Cancel();
    }

    #region Sql string consts

    private readonly string _CreateTestSql = $@"INSERT INTO {DatabaseConfig.TestTableName} 
            ({DatabaseConfig.TestSomeIntDataName}, {DatabaseConfig.TestSomeTextDataName}, {DatabaseConfig.TestSomeSerializableDataName}) 
            VALUES (@Int, @Text, @Serializable)";

    private readonly string _ReadTestSql = "";
    private readonly string _UpdateTestSql = "";
    private readonly string _DeleteTestSql = "";

    #endregion
  }
}
