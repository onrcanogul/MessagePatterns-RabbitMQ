using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


string queueName = "example-queue";
channel.QueueDeclare(
    queue: "example-queue",
    durable: false,
    exclusive: false,
    autoDelete: false
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:queueName,autoAck:true,consumer:consumer);

consumer.Received += (receive, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
