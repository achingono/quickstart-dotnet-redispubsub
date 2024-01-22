using StackExchange.Redis;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var connection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
var subscriber = connection.GetSubscriber();
var messages = new List<string>();

subscriber.Subscribe(builder.Configuration["Redis:ChannelName"], (channel, message) => {
    messages.Add(message);
});

var app = builder.Build();

app.MapGet("/", () => {
    return $@"
    <!DOCTYPE html>
    <html lang=""en"">

    <head>
        <meta charset=""utf-8"" />
        <title>Quickstart.Subscriber</title>
        <base href=""/"" />
    </head>

    <body>
        {messages.Aggregate(string.Empty,(current, next) => current + "<p>" + next + "</p>")}
    </body>

    </html>
    ";
});

app.MapGet("/ready", () => true);
app.MapGet("/healthz", (ISubscriber subscriber) => subscriber.IsConnected());

app.Run();
