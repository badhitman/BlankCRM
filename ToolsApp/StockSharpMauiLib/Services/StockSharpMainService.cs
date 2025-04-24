////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Ecng.Common;
using SharedLib;
using StockSharp.Fix.Quik.Lua;
using System.Net;
using System.Security;

namespace Transmission.Receives.StockSharpClient;

/// <summary>
/// StockSharpMainService
/// </summary>
public class StockSharpMainService : IStockSharpMainService
{
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        StockSharp.Algo.Connector Connector = new();
        LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = new (Connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            Login = "quik",
            Password = "quik".To<SecureString>(),
        };
        LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = new (Connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            Login = "quik",
            Password = "quik".To<SecureString>(),
        };
        Connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
        Connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);
        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpMainService)}"));
    }
}