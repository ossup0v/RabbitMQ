﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Consumer
{
  public static class QueueConsumer
  {
    public static void Consume(IModel channel)
    {
      channel.QueueDeclare(
          queue: "demo-queue"
        , durable: true
        , exclusive: false
        , autoDelete: false
        , arguments: null);

      var consumer = new EventingBasicConsumer(channel);

      consumer.Received += (sender, e) =>
      {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
      };

      channel.BasicConsume(
          queue: "demo-queue"
        , autoAck: true
        , consumer: consumer);
    }
  }
}
