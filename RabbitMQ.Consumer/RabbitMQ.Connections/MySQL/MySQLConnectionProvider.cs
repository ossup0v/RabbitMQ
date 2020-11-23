using RabbitMQ.Connections.Models;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace RabbitMQ.Connections.MySQL
{
  internal sealed class MySQLConnectionProvider : IConnectionProvider, IDisposable
  {
    private readonly string _connectionString = string.Empty;
    private readonly string _authenticationString = string.Empty;
    private readonly string _userId = ConfigurationManager.AppSettings["MySqlUserId"];
    private readonly string _userPassword = ConfigurationManager.AppSettings["MySqlUserPassword"];
    private readonly string _serverAddress = ConfigurationManager.AppSettings["MySqlServerAddress"];
    private readonly string _databaseNamePattern = ConfigurationManager.AppSettings["MySqlDatabaseNamePattern"];

    public MySQLConnectionProvider()
    {
      _authenticationString = $"Uid={_userId};Pwd{_userPassword}";
      new MySqlServerDatabaseCreator(_serverAddress, _authenticationString, _databaseNamePattern).CreateOrUpdateDatabase();
      _connectionString = $"Server={_serverAddress};Database={_databaseNamePattern};{_authenticationString};Allow User Variables=True;Max Pool Size=50;";
    }

    public Func<ConnectionKind, IDbConnection> GetConnectionFactory()
    {
      return GetConnection;
    }

    public void Dispose()
    {
      //(Osipov) todo?
    }

    private IDbConnection GetConnection(ConnectionKind kind)
    {
      return new MySqlConnection(_connectionString);
    }
  }
}
