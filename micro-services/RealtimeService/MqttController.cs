////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MQTTnet.Packets;
using MQTTnet.Server;
using Newtonsoft.Json;
using SharedLib;
using System.Text;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace RealtimeService;

public class MqttController(IEventsWebChatsNotifies NotifyWebChatRepo)
{
    public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        //Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ValidateConnection)}. Accepting!");
        return Task.CompletedTask;
    }

    public async Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        KeyValuePair<string, UserInfoModel?>? _ui = null;
        // 
        MqttUserProperty? prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME));
        if (prop is not null)
            _ui = JsonConvert.DeserializeObject<KeyValuePair<string, UserInfoModel?>>(Encoding.UTF8.GetString([.. prop.ValueBuffer.ToArray()]));

        //Console.WriteLine($"Client '{eventArgs.ClientId}' connected.");
        if (eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.MUTE_CONTROLLER_NAME)) != true && eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME)) == true)
            await NotifyWebChatRepo.OnClientConnectedWebChatHandle(new() { UserInfoBaseModel = _ui });
    }

    public async Task ClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        UserInfoModel? _ui = null;
        // 
        MqttUserProperty? prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME));
        if (prop is not null)
            _ui = JsonConvert.DeserializeObject<UserInfoModel>(Encoding.UTF8.GetString([.. prop.ValueBuffer.ToArray()]));

        //Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ClientDisconnected)}. Accepting!");
        if (eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.MUTE_CONTROLLER_NAME)) != true && eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME)) == true)
            await NotifyWebChatRepo.ClientDisconnectedWebChatHandle(new() { UserInfoBaseModel = _ui });
    }

    public Task ClientSubscribedTopic(ClientSubscribedTopicEventArgs eventArgs)
    {
        //Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ClientSubscribedTopic)}. Accepting!");
        return Task.CompletedTask;
    }

    public Task PreparingSession(EventArgs eventArgs)
    {
        //Console.WriteLine($"Client '{eventArgs}' wants to {nameof(PreparingSession)}. Accepting!");
        return Task.CompletedTask;
    }
}