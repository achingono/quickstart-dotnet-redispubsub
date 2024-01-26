# Quickstart: Redis Publisher and Subscribers

This project utilizes [Redis Pub/Sub](https://redis.io/docs/interact/pubsub/) to implement the [Publish/Subscribe messaging paradigm](https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern) to enable communication between different components. By utilizing the publisher-subscriber pattern, this project enables decoupled communication between different components, allowing for better scalability and flexibility. 

## Features
Here's a brief overview of the publisher and subscribers:

- The publisher is responsible for sending messages or events to the subscribers. In this project, the publisher component is implemented using the Redis Pub-Sub mechanism. It publishes messages to specific channels, which are then received by the subscribers.

- The subscribers are the components that listen for messages or events published by the publisher. In this project, there can be multiple subscribers, each interested in different types of messages. The subscribers subscribe to specific channels and receive messages whenever a publisher publishes a message to that channel.


## Getting Started

To get started with this project, follow these steps:

1. Clone the repository.
   ```
   https://github.com/achingono/quickstart-dotnet-redispubsub.git
   ```
2. Run the publisher and subscribers components
   ```
   docker compose up
   ```
3. Browse to `http://localhost:5010` to send the first message.
4. Browse to `http://localhost:5020` to view messages received by the dotnet subscriber.
5. Browse to `http://localhost:5030` to view messages received by the node subscriber.
6. Refresh `http://localhost:5010` multiple times to send multiple messages.
7. Refresh `http://localhost:5020` and/or `http://localhost:5020` to view the multiple messages.

## Related Resources
- [Publish-subscribe pattern](https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern)
- [Minimial APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview?view=aspnetcore-7.0)
- [Redis Pub/Sub](https://redis.io/docs/interact/pubsub/)
- [StackExchange.Redis | General purpose Redis client](https://stackexchange.github.io/StackExchange.Redis/)
- [ioredis](https://github.com/redis/ioredis#readme)
- [Express](https://expressjs.com/)
- [Docker Compose](https://docs.docker.com/compose/)