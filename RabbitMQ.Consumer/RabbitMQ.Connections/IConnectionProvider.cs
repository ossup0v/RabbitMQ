using RabbitMQ.Connections.Models;
using System;
using System.Data;

namespace RabbitMQ.Connections
{
  public interface IConnectionProvider
  {
    Func<ConnectionKind, IDbConnection> GetConnectionFactory();
  }
}
