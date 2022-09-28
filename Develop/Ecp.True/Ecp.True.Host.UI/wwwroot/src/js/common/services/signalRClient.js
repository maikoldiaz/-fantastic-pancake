const signalR = require('@microsoft/signalr');

const signalRClient = (function () {
    const options = {
        transport: signalR.HttpTransportType.WebSockets,
        Credentials: 'same-origin'
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/concurrentedit', options)
        .configureLogging(signalR.LogLevel.Warning)
        .withAutomaticReconnect()
        .build();

    return {
        connect: async function () {
            // Start connection only if it does not exist already.
            if (connection.connection.connectionState === 'Disconnected') {
                connection.serverTimeoutInMilliseconds = 30000;
                await connection.start();
            }
        },
        register: function (callbacks) {
            callbacks.forEach(c => connection.on(c.operation, c.fn));
        },
        publish: async function (method, message) {
            if (message) {
                await connection.invoke(method, JSON.stringify(message));
            } else {
                await connection.invoke(method);
            }
        }
    };
}());

export { signalRClient };
