using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


string requestQueueName = "example-request-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

//Response side
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue:requestQueueName,
    autoAck:true,
    consumer:consumer
    );

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    //...

    //Request side
    byte[] responseMessage = Encoding.UTF8.GetBytes($"Process is completed : {message}");
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;
    channel.BasicPublish(
        exchange:String.Empty,
        routingKey:e.BasicProperties.ReplyTo,
        basicProperties: properties,
        body:responseMessage);
};

Console.Read();