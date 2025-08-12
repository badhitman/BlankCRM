////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// IManageStockSharpService
/// </summary>
public interface IManageStockSharpService
{
    /// <summary>
    /// Информация о базах данных
    /// </summary>
    public Task<AboutDatabasesResponseModel> AboutDatabases(CancellationToken cancellationToken = default);

    /// <summary>
    /// UpdateOrCreateAdapter
    /// </summary>
    public Task<TResponseModel<FixMessageAdapterModelDB>> UpdateOrCreateAdapterAsync(FixMessageAdapterModelDB req, CancellationToken cancellationToken = default);

    /// <summary>
    /// AdaptersSelectAsync
    /// </summary>
    public Task<TPaginationResponseModel<OrderStockSharpViewModel>> OrdersSelectAsync(TPaginationRequestStandardModel<OrdersSelectStockSharpRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// AdaptersSelectAsync
    /// </summary>
    public Task<TPaginationResponseModel<FixMessageAdapterModelDB>> AdaptersSelectAsync(TPaginationRequestStandardModel<AdaptersRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// AdaptersGetAsync
    /// </summary>
    public Task<TResponseModel<FixMessageAdapterModelDB[]>> AdaptersGetAsync(int[] req, CancellationToken cancellationToken = default);

    /// <summary>
    /// DeleteAdapterAsync
    /// </summary>
    public Task<ResponseBaseModel> DeleteAdapterAsync(int adapterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// GenerateRegularCashFlowsAsync
    /// </summary>
    public Task<ResponseBaseModel> GenerateRegularCashFlowsAsync(CashFlowStockSharpRequestModel req, CancellationToken cancellationToken = default);
}