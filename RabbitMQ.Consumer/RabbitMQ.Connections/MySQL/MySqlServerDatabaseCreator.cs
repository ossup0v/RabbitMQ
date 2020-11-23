using Dapper;
using MySql.Data.MySqlClient;
using RabbitMQ.Logger;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Connections.MySQL
{
  internal sealed class MySqlServerDatabaseCreator
  {
    private string _serverAddress;
    private string _authenticationString;
    private string _databaseNamePattern;

    public MySqlServerDatabaseCreator(
        string serverAddress
      , string authenticationString
      , string databaseNamePattern)
    {
      _serverAddress = serverAddress;
      _authenticationString = authenticationString;
      _databaseNamePattern = databaseNamePattern;
    }

    public void CreateOrUpdateDatabase()
    {
      using (var connection = new MySqlConnection($"Server={_serverAddress};{_authenticationString}"))
      {
        connection.Open();
        var exists = connection.Query($"SHOW DATABASES LIKE '{_databaseNamePattern}'").Any();

        if (exists)
        {
          CurrentLogger.Log($"Using MySQL database {_databaseNamePattern}");
        }
        else
        {
          CurrentLogger.Log($"Creating new MySQL database {_databaseNamePattern}");

          CreateDatabase(connection);
          CreateTables(connection);
        }

        using (var t = connection.BeginTransaction())
        {
          connection.Execute($"USE {_databaseNamePattern}");
          Migrate(connection).Wait();
          t.Commit();
        }
      }
    }

    private async Task Migrate(MySqlConnection connection)
    {
      //(Osipov) todo?
    }

    private void CreateDatabase(MySqlConnection connection)
    {
      connection.Execute($@"CREATE DATABASE IF NOT EXISTS {_databaseNamePattern};");
    }

    private void CreateTables(MySqlConnection connection)
    {
      CreateTestTable(connection);
    }

    private void CreateTestTable(MySqlConnection connection)
    {
      connection.Execute($@"
        USE {_databaseNamePattern};

        CREATE TABLE IF NOT EXISTS test_table (
            some_int_data           BIGINT       NOT NULL,
            some_text_data          VARCHAR(500) NOT NULL,
            some_serializable_data  LONGBLOB     NOT NULL
        );
      ");
    }
  }
}
