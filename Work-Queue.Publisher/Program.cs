using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string queueName = "example-queue";
channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false
    );

for(int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("message " + i);
    channel.BasicPublish(
        exchange: String.Empty,
        routingKey: queueName,
        body:message);
}

Console.Read();