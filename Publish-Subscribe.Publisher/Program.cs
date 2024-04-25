using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


string exchangeName = "example-exchange";
channel.ExchangeDeclare(
    exchange: exchangeName,
    type:ExchangeType.Fanout,
    durable:false,
    autoDelete:false
    );


byte[] message = Encoding.UTF8.GetBytes("message");

channel.BasicPublish(exchange: exchangeName, routingKey: String.Empty, body:message);