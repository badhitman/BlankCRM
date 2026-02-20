////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// AdaptersTypesNames
/// </summary>
public enum AdaptersTypesNames
{
    /// <summary>
    /// MarketData: Quik Lua
    /// </summary>
    [Description("Quik LUA Market data")]
    LuaFixMarketDataMessageAdapter,

    /// <summary>
    /// Transaction: Quik Lua
    /// </summary>
    [Description("Quik LUA Transactions")]
    LuaFixTransactionMessageAdapter,
}