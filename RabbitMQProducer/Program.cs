using System.Text;
using RabbitMQ.Client;


// Optionally, you could load the CloudAMQP connection string from an environment variable.
// This is commented out here and replaced with a hardcoded URI for demonstration.

//var uri = Environment.GetEnvironmentVariable("CLOUDAMQP_URL")
//    ?? throw new InvalidOperationException("Missing CLOUDAMQP_URL environment variable.");


// CloudAMQP connection string (amqps://...).
// This contains the username, password, host, and virtual host needed to connect.
var uri = "amqps://fwjruorn:o1vm3OUqlSnMGVltJKpIFza-J6sF8rFI@ostrich.lmq.cloudamqp.com/fwjruorn";


// Create a connection factory that knows how to connect to RabbitMQ.
// DispatchConsumersAsync = true allows asynchronous message handling (important for consumers).
var factory = new ConnectionFactory
{
    Uri = new Uri(uri),
    DispatchConsumersAsync = true
};


// Establish a connection to the RabbitMQ broker (CloudAMQP).
using var connection = factory.CreateConnection();

// Create a channel (a lightweight communication session over the connection).
using var channel = connection.CreateModel();


// Define the queue name we want to publish messages to.
const string queue = "IruoQ";


// Ensure the queue exists (create it if not).
// durable: true → queue survives broker restarts
// exclusive: false → multiple connections can use it
// autoDelete: false → queue is not deleted when last consumer disconnects
channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

Console.WriteLine("[Producer] Connected to CloudAMQP.");
Console.WriteLine("[Producer] Sending messages...");

// Loop to send 5 messages into the queue.
for (int i = 1; i <= 5; i++)
{
    var body = Encoding.UTF8.GetBytes($"Hello CloudAMQP message #{i}");

    // Publish the message to the default exchange ("") with the queue name as the routing key.
    channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);

    // Log to the console that the message was sent.
    Console.WriteLine($"[Producer] Sent: message #{i}");

    // Wait 1 second between messages to simulate spacing out work.
    await Task.Delay(1000);
}

Console.WriteLine("[Producer] Done.");
