using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
    autoDelete: false);


EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true, //eğer false ise manuel silme işlemi yapmalıyız.(Bir şekilde silme işlemi yapılmalı manuel veya auto)
    consumer:consumer);

channel.BasicQos(
    prefetchSize: 1, //her bir consumer 1 mesaj
    prefetchCount: 0, //totalde ise her bir consumer sınırsız mesaj alabilir
    global: false);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};