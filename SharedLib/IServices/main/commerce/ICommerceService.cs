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
    /// ContractorsOrganizationsFind
    /// </summary>
    public Task<OrganizationContractorModel[]> ContractorsOrganizationsFindAsync(ContractorsOrganizationsRequestModel req, CancellationToken token = default);
}