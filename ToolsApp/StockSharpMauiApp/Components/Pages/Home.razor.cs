////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using NetMQ;
using NetMQ.Sockets;

namespace StockSharpMauiApp.Components.Pages;

public partial class Home
{
    static Task Click1()
    {
        using (RequestSocket client = new())
        {
            client.Connect("tcp://127.0.0.1:5556");
            client.SendFrame("Hello");
            string msg = client.ReceiveFrameString();
            Console.WriteLine("From Server: {0}", msg);
        }
        return Task.CompletedTask;
    }
}