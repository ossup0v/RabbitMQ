using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Producer
{
  static class Program
  {
    private static void Main(string[] args)
    {
      var factory = new ConnectionFactory
      {
        Uri = new Uri("amqp://guest:guest@localhost:5672")
      };

      using (var conn = factory.CreateConnection())
      using (var channel = conn.CreateModel())
      {
        QueueProducer.Publish(channel);
      }
    }
  }
}
