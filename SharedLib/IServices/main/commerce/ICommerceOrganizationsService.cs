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
    public Task<TResponseModel<bool>> OrganizationOfferContractUpdate(TAuthRequestModel<OrganizationOfferToggleModel> req);

    /// <summary>
    /// ContractorsOrganizationsFind
    /// </summary>
    public Task<OrganizationContractorModel[]> ContractorsOrganizationsFind(ContractorsOrganizationsRequestModel req);
}