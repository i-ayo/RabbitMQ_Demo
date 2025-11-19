# RabbitMQ Producer–Consumer Demo (.NET 7 + CloudAMQP)

This repository demonstrates a simple **Producer–Consumer pattern** using **RabbitMQ** hosted on **CloudAMQP**.
It includes a producer that publishes messages to a queue and a consumer that retrieves, processes, and acknowledges them.

---

## Overview

* **Message Broker:** RabbitMQ (CloudAMQP)
* **Producer:**

  * Connects to CloudAMQP using a connection URI
  * Declares a queue (`IruoQ`)
  * Publishes 5 test messages
* **Consumer:**

  * Listens on the same queue
  * Processes each message
  * Simulates a short workload
  * Acknowledges messages manually

---

## Prerequisites

* .NET SDK 7.0 or higher
* Visual Studio 2022 or `dotnet` CLI
* CloudAMQP account with a valid connection URI

### Optional: Environment Variable for Connection URI

```bash
set CLOUDAMQP_URL=amqps://username:password@host/vhost
```

---

## Running the Demo

### Producer

```bash
dotnet run --project RabbitMQProducer
```

The producer will:

* Connect to CloudAMQP
* Publish 5 messages
* Pause 1 second between messages

### Consumer

```bash
dotnet run --project RabbitMQConsumer
```

The consumer will:

* Connect to CloudAMQP
* Wait for messages
* Print and process each message
* Acknowledge messages after processing

---

## Example Output

### Producer

```
[Producer] Connected to CloudAMQP.
[Producer] Sending messages...
[Producer] Sent: message #1
...
[Producer] Done.
```

### Consumer

```
[Consumer] Connected to CloudAMQP. Waiting for messages in 'IruoQ'...
[Consumer] Received: Hello CloudAMQP message #1
[Consumer] Received: Hello CloudAMQP message #2
...
```

---

## Key Concepts

### Producer vs. Consumer

* **Producer** sends messages.
* **Consumer** receives and processes them.
* **RabbitMQ** acts as the broker between them.

### Queue Properties

* `durable: true` → queue persists across broker restarts
* `exclusive: false` → allows multiple consumers
* `autoDelete: false` → queue remains until manually removed

### Message Acknowledgments

* `autoAck: false` → consumer manually acknowledges messages
* `BasicAck()` ensures messages are removed only after successful processing

### Processing Simulation

* `Task.Delay(500)` imitates work (e.g., writing to DB, sending emails)
* `Task.Delay(Timeout.Infinite)` keeps the consumer running
