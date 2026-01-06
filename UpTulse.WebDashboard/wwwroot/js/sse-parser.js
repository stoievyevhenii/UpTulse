window.sseHandlers = {
    connect: function (dotNetHelper, url) {
        const eventSource = new EventSource(url);

        eventSource.onmessage = function (e) {
            dotNetHelper.invokeMethodAsync('ReceiveMessage', e.data);
        };

        eventSource.onerror = function (e) {
            console.error("SSE Connection Failed", e);
        };

        return eventSource;
    }
};