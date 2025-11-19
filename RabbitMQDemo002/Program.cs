// Basic RabbitMQ Consumer Example using CloudAMQP
// This program connects to a RabbitMQ queue hosted on CloudAMQP,
// waits for messages, processes them, and acknowledges receipt.

// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//var uri = Environment.GetEnvironmentVariable("CLOUDAMQP_URL")
//    ?? throw new InvalidOperationException("Missing CLOUDAMQP_URL environment variable.");

// Create a connection factory with the CloudAMQP URI.
// DispatchConsumersAsync = true allows asynchronous message handling.
var uri = "amqps://fwjruorn:o1vm3OUqlSnMGVltJKpIFza-J6sF8rFI@ostrich.lmq.cloudamqp.com/fwjruorn";

var factory = new ConnectionFactory
{
    Uri = new Uri(uri),
    DispatchConsumersAsync = true
};

// Establish a connection to RabbitMQ broker.
using var connection = factory.CreateConnection();

// Create a channel(send/recieve messages) (a lightweight communication session over the connection).
using var channel = connection.CreateModel();

// Define the queue name we want to consume messages from.
const string queue = "IruoQ";

// Ensure the queue exists (create it if not).
// durable: true → queue survives broker restarts
// exclusive: false → multiple consumers can use it
// autoDelete: false → queue is not deleted when last consumer disconnects
channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

Console.WriteLine($"[Consumer] Connected to CloudAMQP. Waiting for messages in '{queue}'...");


// Create an asynchronous consumer that listens for messages on the channel.
var consumer = new AsyncEventingBasicConsumer(channel);


// Define what happens when a message is received.
consumer.Received += async (_, ea) =>
{
    // Convert the message body(byte array) into a readable string.
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

    // Print the received message to the console.
    Console.WriteLine($"[Consumer] Received: {message}");

    // Simulate doing some work with the message (e.g., processing, saving to DB).
    await Task.Delay(500); // simulate processing

    // Send an acknowledgment back to RabbitMQ to confirm the message was handled.
    channel.BasicAck(ea.DeliveryTag, false);
};


// Start consuming messages from the queue.
// autoAck: false → we manually acknowledge messages after processing.
channel.BasicConsume(queue, autoAck: false, consumer);

// Keep the program running indefinitely so it can continue listening for messages.
await Task.Delay(Timeout.Infinite);

