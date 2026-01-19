////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <summary>
    /// Обновить/создать банковские реквизиты
    /// </summary>
    public Task<TResponseModel<int>> BankDetailsUpdateOrCreateAsync(TAuthRequestStandardModel<BankDetailsModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Удалить банковские реквизиты
    /// </summary>
    public Task<TResponseModel<BankDetailsModelDB>> BankDetailsForOrganizationDeleteAsync(TAuthRequestStandardModel<BankDetailsForOrganizationDeleteRequestModel> req, CancellationToken token = default);
}