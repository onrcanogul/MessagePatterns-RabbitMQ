using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://yejguzib:NuvHgMGxrkfxK8SHh3VRyAYLi3B3ENf1@shark.rmq.cloudamqp.com/yejguzib");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


string requestQueueName = "example-request-queue";
channel.QueueDeclare(
    queue:requestQueueName,
    durable:false,
    exclusive:false,
    autoDelete:false);

string responseQueueName = channel.QueueDeclare().QueueName;
string correlationId = Guid.NewGuid().ToString();


//Request side
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = responseQueueName;


for(int i=0 ; i<10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("message " + i);
    channel.BasicPublish(
        exchange:String.Empty,// direct
        routingKey: requestQueueName,
        basicProperties:properties,
        body:message);
}

//Response side
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: responseQueueName,
    autoAck: true,
    consumer: consumer);
consumer.Received += (sender, e) =>
{
    if(e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};

Console.Read();

