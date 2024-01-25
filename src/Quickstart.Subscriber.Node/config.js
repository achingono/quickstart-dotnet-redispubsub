const config = {
    redis: {
        connectionString: process.env.REDIS_CONNECTION_STRING,
        channelName: process.env.REDIS_CHANNEL_NAME
    }
};

module.exports = config;
