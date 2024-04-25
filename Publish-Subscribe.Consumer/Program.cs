using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string exchangeName = "example-exchange";
channel.ExchangeDeclare(
    exchange: exchangeName,
    type: ExchangeType.Fanout,
    durable: false,
    autoDelete: false
    );

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName,
    exchange: exchangeName,
    routingKey: String.Empty
    );


EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer:consumer);


consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
