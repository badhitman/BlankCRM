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
    
    /// <summary>
    /// Смена статуса заказу по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Смена статуса записи/брони по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Поиск доступных услуг/броней
    /// </summary>
    public Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, int[]? organizationsFilter = null, CancellationToken token = default);
}