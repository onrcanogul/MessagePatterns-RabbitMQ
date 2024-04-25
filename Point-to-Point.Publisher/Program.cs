using RabbitMQ.Client;
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

string message = "Hello";
byte[] byteMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange:string.Empty,
    routingKey:queueName,
    body: byteMessage
    );

