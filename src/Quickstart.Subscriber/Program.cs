using StackExchange.Redis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
var subscriber = connection.GetSubscriber();
var messages = new List<string>();

subscriber.Subscribe(RedisChannel.Literal(builder.Configuration["Redis:ChannelName"]), (channel, message) => {
    messages.Add(message);
});

var app = builder.Build();

app.MapGet("/", () => {
    return new {
        messages
    };
});

app.MapGet("/config", (IConfiguration configuration) => new {
    Redis = new {
        ConnectionString = configuration.GetConnectionString("Redis"),
        ChannelName = configuration["Redis:ChannelName"]
    }
});

app.MapGet("/ready", () => true);
app.MapGet("/healthz", () => subscriber.IsConnected());

app.Run();
