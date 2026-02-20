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

    public async Task ClientConnected(ClientConnectedEventArgs eventArgs)
    {
        UserInfoModel? _ui = null;
        // 
        MqttUserProperty? prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME));
        if (prop is not null)
            _ui = JsonConvert.DeserializeObject<UserInfoModel?>(Encoding.UTF8.GetString([.. prop.ValueBuffer.ToArray()]));

        int dialogId = 0;
        prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.DIALOG_CONTROLLER_NAME));
        if (prop is not null)
            dialogId = BitConverter.ToInt32(prop.ValueBuffer.ToArray());

        //Console.WriteLine($"Client '{eventArgs.ClientId}' connected.");
        if (eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.MUTE_CONTROLLER_NAME)) != true && eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME)) == true)
            await NotifyWebChatRepo.OnClientConnectedWebChatAsync(new() { UserInfo = _ui, DialogId = dialogId });
    }

    public async Task ClientDisconnected(ClientDisconnectedEventArgs eventArgs)
    {
        UserInfoModel? _ui = null;
        // 
        MqttUserProperty? prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME));
        if (prop is not null)
            _ui = JsonConvert.DeserializeObject<UserInfoModel>(Encoding.UTF8.GetString([.. prop.ValueBuffer.ToArray()]));

        int dialogId = 0;
        prop = eventArgs.UserProperties?.FirstOrDefault(x => x.Name.Equals(Routes.DIALOG_CONTROLLER_NAME));
        if (prop is not null)
            dialogId = BitConverter.ToInt32(prop.ValueBuffer.ToArray());

        //Console.WriteLine($"Client '{eventArgs.ClientId}' wants to {nameof(ClientDisconnected)}. Accepting!");
        if (eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.MUTE_CONTROLLER_NAME)) != true && eventArgs.UserProperties?.Any(x => x.Name.Equals(Routes.USER_CONTROLLER_NAME)) == true && dialogId > 0)
            await NotifyWebChatRepo.ClientDisconnectedWebChatAsync(new() { UserInfoBaseModel = _ui, DialogId = dialogId });
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