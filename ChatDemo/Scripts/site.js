var chatter = function () {

    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.onMessage = function (message) {
        $("#chatMsg").append(message);
    };

    var initilize = function () {
        debugger;

        $.connection.hub.start().done(function () {
            alert("connected to server...");
        }).fail(function () {
            debugger;
        });

        $("#btnSendMessage").click(function () {
            sendMessage();
        });

        $("#btnConnect").click(function () {
            subscribe();
        });
    };

    var subscribe = function () {
        var res = chatHubProxy.server.subscribe($('#userName').val());
        $('#userName').val("");
    };

    var sendMessage = function () {
        var res = chatHubProxy.server.sendMessage($('#message').val());
        $('#message').val("");
    };


    return {
        initilize: initilize
    }
}();