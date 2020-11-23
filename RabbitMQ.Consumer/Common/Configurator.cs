using System;
using System.Configuration;

namespace RabbitMQ.Common
{
  public sealed class Configurator
  {
    public static ServerConfig Config { get; }
    static Configurator()
    {
      Config = new ServerConfig(
        dbCommandTimeOut: 30);
    }
  }

  public class ServerConfig
  {
    //(Osipov) todo add here mysql connection setting
    public int DbCommandTimeOut { get; } //(Osipov) todo split it to create/delete/update/read timeout ?
    public ServerConfig(int dbCommandTimeOut)
    {
      DbCommandTimeOut = dbCommandTimeOut;
    }
  }
}
