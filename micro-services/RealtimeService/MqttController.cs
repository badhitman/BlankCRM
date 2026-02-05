////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MQTTnet.Server;

namespace RealtimeService;

public class MqttController
{
    public static Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' connected.");
        return Task.CompletedTask;
    }


    public static Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ValidateConnection)}. Accepting!");
        return Task.CompletedTask;
    }

    public static Task ClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ClientDisconnected)}. Accepting!");
        return Task.CompletedTask;
    }

    public static Task ClientSubscribedTopic(ClientSubscribedTopicEventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ClientSubscribedTopic)}. Accepting!");
        return Task.CompletedTask;
    }

    public static Task PreparingSession(EventArgs eventArgs)
    {
        Console.WriteLine($"Client '{eventArgs}' wants to {nameof(PreparingSession)}. Accepting!");
        return Task.CompletedTask;
    }
}