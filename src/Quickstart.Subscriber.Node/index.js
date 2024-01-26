/**
 * This file contains the code for a Redis pub/sub subscriber in Node.js.
 * It establishes a connection to Redis, subscribes to a channel, and handles incoming messages.
 * The server exposes several routes for retrieving messages, configuration details, and health status.
 */
const express = require('express');
const Redis = require('ioredis');
const app = express();
const config = require('./config'); // Assuming configuration is stored in config.js

// Establish connection to Redis
const redis = new Redis(config.redis.connectionString);
const messages = [];

// Subscribe to the Redis channel specified in the configuration.
redis.subscribe(config.redis.channelName, (error, count) => {
    if (error) {
        console.error('Subscription error:', error);
        return;
    }
    console.log(`Subscribed to ${count} channel(s). Currently subscribed to ${config.redis.channelName}`);
});

// When a message is received on the channel, it is added to the 'messages' list.
redis.on('message', (channel, message) => {
    messages.push(message);
});

// Endpoint to retrieve the list of messages received from the Redis channel.
app.get('/', (req, res) => {
    res.json({ messages });
});

// Endpoint to retrieve the Redis connection configuration.
app.get('/config', (req, res) => {
    res.json({
        Redis: {
            ConnectionString: config.redis.connectionString,
            ChannelName: config.redis.channelName
        }
    });
});

// Endpoint to check if the application is ready.
app.get('/ready', (req, res) => {
    res.send(true);
});

// Endpoint to check the health of the Redis subscriber.
app.get('/healthz', (req, res) => {
    res.send(redis.status === 'ready');
});

// Start the server
const port = process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});
