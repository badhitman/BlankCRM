////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Kladr.control;

/// <summary>
/// KladrFindTableResultComponent
/// </summary>
public partial class KladrFindTableResultComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FindText { get; set; }


    List<KladrResponseModel>? PartData;


    private string _value = "Nothing selected";
    private IEnumerable<string> _options = new HashSet<string> { "Alaska" };
    private readonly string[] _states =
    [
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
        "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Marshall Islands", "Maryland",
        "Massachusetts", "Michigan", "Minnesota", "Mississippi",
        "Missouri", "Montana", "Nebraska", "Nevada",
        "New Hampshire", "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio",
        "Oklahoma", "Oregon", "Palau", "Pennsylvania", "Puerto Rico",
        "Rhode Island", "South Carolina", "South Dakota", "Tennessee",
        "Texas", "Utah", "Vermont", "Virgin Island", "Virginia",
        "Washington", "West Virginia", "Wisconsin", "Wyoming"
    ];


    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        KladrFindRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            FindText = $"%{FindText}%",
        };

        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsFind(req);
        await SetBusy(false, token: token);
        PartData = res.Response ?? [];
        return new TableData<KladrResponseModel>()
        {
            TotalItems = res.TotalRowsCount,
            Items = PartData
        };
    }
}