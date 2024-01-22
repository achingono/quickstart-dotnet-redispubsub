using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddScoped<ISubscriber>(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetSubscriber());
var app = builder.Build();

app.MapGet("/", async (ISubscriber subscriber) => {
    var message = "Hello World!";
    await subscriber.PublishAsync(RedisChannel.Literal(builder.Configuration["Redis:ChannelName"]), new RedisValue(message));
    return message;
});

app.MapGet("/ready", () => true);
app.MapGet("/healthz", (ISubscriber subscriber) => subscriber.IsConnected());

app.Run();
