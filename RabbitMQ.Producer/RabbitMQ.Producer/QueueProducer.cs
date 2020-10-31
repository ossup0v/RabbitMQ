using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.Producer
{
  public static class QueueProducer
  {
    public static void Publish(IModel channel)
    {
      channel.QueueDeclare(
          queue: "demo-queue"
        , durable: true
        , exclusive: false
        , autoDelete: false
        , arguments: null);

      var count = 0;

      while (true)
      {
        var message = new { Name = "Producer", Message = "Hello count is" + count .ToString() };
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        channel.BasicPublish("", "demo-queue", null, body);
        Thread.Sleep(TimeSpan.FromSeconds(1));
        count++;
      }
    }
  }
}
