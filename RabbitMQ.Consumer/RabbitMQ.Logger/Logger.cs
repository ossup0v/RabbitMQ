using System;

namespace RabbitMQ.Logger
{
  public static class CurrentLogger
  {
    public static void Log(string message)
    {
      Console.WriteLine(message);
    }
  }
}
