////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Rest (API) interfaces (end-point`s) for Bank`s connection`s
/// </summary>
public enum BankInterfacesEnum
{
    /// <inheritdoc/>
    [ConnectionMetadata(BaseUrl = "https://business.tbank.ru/openapi/api/", GetStatementRequest = "v1/statement", AccListRequest = "v4/bank-accounts")]
    [Description("T-Bank")]
    TBank,
}