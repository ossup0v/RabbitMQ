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
        channel.QueueDeclare(
            queue: "demo-queue"
          , durable: false
          , exclusive: false
          , autoDelete: false
          , arguments: null);
        var message = new { Name = "Producer", Message = "Hello!!" };

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        channel.BasicPublish("", "demo-queue", null, body);
        Console.WriteLine($"Message {message.ToString()} was seneded");
        Console.ReadLine();
      }

    }
  }
}
