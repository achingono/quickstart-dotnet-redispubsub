const express = require('express');
const Redis = require('ioredis');
const app = express();
const config = require('./config'); // Assuming configuration is stored in config.js

// Establish connection to Redis
const redis = new Redis(config.redis.connectionString);
const messages = [];

// Subscribe to the Redis channel
redis.subscribe(config.redis.channelName, (error, count) => {
    if (error) {
        console.error('Subscription error:', error);
        return;
    }
    console.log(`Subscribed to ${count} channel(s). Currently subscribed to ${config.redis.channelName}`);
});

redis.on('message', (channel, message) => {
    messages.push(message);
});

// Define the routes
app.get('/', (req, res) => {
    res.json({ messages });
});

app.get('/config', (req, res) => {
    res.json({
        Redis: {
            ConnectionString: config.redis.connectionString,
            ChannelName: config.redis.channelName
        }
    });
});

app.get('/ready', (req, res) => {
    res.send(true);
});

app.get('/healthz', (req, res) => {
    res.send(redis.status === 'ready');
});

// Start the server
const port = process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});
