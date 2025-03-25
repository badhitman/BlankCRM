////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Организации
/// </summary>
public partial interface ICommerceService : ICommerceServiceBase
{
    /// <summary>
    /// OrganizationOfferContractUpdate
    /// </summary>
    public Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default);

    /// <summary>
    /// ContractorsOrganizationsFind
    /// </summary>
    public Task<OrganizationContractorModel[]> ContractorsOrganizationsFindAsync(ContractorsOrganizationsRequestModel req, CancellationToken token = default);
}