using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Register the Redis connection as a singleton.
// This ensures that the same Redis connection is used throughout the application.
// https://stackexchange.github.io/StackExchange.Redis/Basics
builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

// Register the Redis subscriber as a scoped service.
// This allows the Redis subscriber to be injected into other components with a limited lifetime.
// https://stackexchange.github.io/StackExchange.Redis/Basics#using-redis-pubsub
builder.Services.AddScoped<ISubscriber>(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetSubscriber());

var app = builder.Build();

/// <summary>
/// Publishes a message to a Redis channel and returns the message.
/// </summary>
app.MapGet("/", async (ISubscriber subscriber, [FromHeader(Name = "User-Agent")] string agent) => {
    var message = $"[{DateTime.UtcNow}] Hello World! from agent: {agent}";
    var channelName = builder.Configuration["Redis:ChannelName"] ?? "*";
    await subscriber.PublishAsync(
        RedisChannel.Literal(channelName), 
        new RedisValue(message)
    );
    return message;
});

/// <summary>
/// Retrieves the Redis configuration settings.
/// </summary>
app.MapGet("/config", (IConfiguration configuration) => new {
    Redis = new {
        ConnectionString = configuration.GetConnectionString("Redis"),
        ChannelName = configuration["Redis:ChannelName"]
    }
});

/// <summary>
/// Checks if the application is ready.
/// </summary>
app.MapGet("/ready", () => true);

/// <summary>
/// Checks the health status of the Redis subscriber.
/// </summary>
app.MapGet("/healthz", (ISubscriber subscriber) => subscriber.IsConnected());

app.Run();
