using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Create a reusable Redis connection.
// This ensures that the same Redis connection is used throughout the application.
// https://stackexchange.github.io/StackExchange.Redis/Basics
var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));

// Subscribe to the configured Redis channel.
// https://stackexchange.github.io/StackExchange.Redis/Basics#using-redis-pubsub
var subscriber = connection.GetSubscriber();

// Create a list to store the messages received from the Redis channel.
var messages = new List<RedisValue>();

// Subscribe to the Redis channel specified in the configuration.
// When a message is received on the channel, it is added to the 'messages' list.
subscriber.Subscribe(RedisChannel.Literal(builder.Configuration["Redis:ChannelName"] ?? "*"), (channel, message) => {
    messages.Add(message);
});

var app = builder.Build();
/// <summary>
/// Endpoint to retrieve the list of messages received from the Redis channel.
/// </summary>
app.MapGet("/", () => {
    return new {
        messages = messages.Select(message => message.ToString())
    };
});

/// <summary>
/// Endpoint to retrieve the Redis connection configuration.
/// </summary>
app.MapGet("/config", (IConfiguration configuration) => new {
    Redis = new {
        ConnectionString = configuration.GetConnectionString("Redis"),
        ChannelName = configuration["Redis:ChannelName"]
    }
});

/// <summary>
/// Endpoint to check if the application is ready.
/// </summary>
app.MapGet("/ready", () => true);

/// <summary>
/// Endpoint to check the health of the Redis subscriber.
/// </summary>
app.MapGet("/healthz", () => subscriber.IsConnected());

app.Run();
