////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorWebLib.Components;

/// <summary>
/// TagsViewComponent
/// </summary>
public partial class TagsViewComponent : MetaPropertyBaseComponent
{
    [Inject]
    IParametersStorageTransmission TagsRepo { get; set; } = default!;


    List<TagViewModel> TagsSets { get; set; } = [];

    MudAutocomplete<string?>? maRef;
    string? _value;
    string? TagAdding
    {
        get => _value; set
        {
            _value = value;
            InvokeAsync(AddChip);
        }
    }

    private async Task AddChip()
    {
        if (string.IsNullOrWhiteSpace(_value) || !OwnerPrimaryKey.HasValue)
            return;

        TResponseModel<bool> res = await TagsRepo.TagSetAsync(new()
        {
            PrefixPropertyName = PrefixPropertyName,
            ApplicationName = ApplicationsNames.Single(),
            PropertyName = PropertyName,
            Name = _value,
            Id = OwnerPrimaryKey.Value,
            Set = true
        });

        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        else
        {
            await ReloadTags();
            StateHasChanged();
        }
        _value = "";
        if (maRef is not null)
            await maRef.ClearAsync();
    }

    private async Task OnChipClosed(MudChip<TagViewModel> chip)
    {
        if (!string.IsNullOrWhiteSpace(chip.Value?.TagName) && OwnerPrimaryKey.HasValue)
        {
            await SetBusyAsync();
            TResponseModel<bool> res = await TagsRepo.TagSetAsync(new()
            {
                PrefixPropertyName = PrefixPropertyName,
                ApplicationName = ApplicationsNames.Single(),
                PropertyName = PropertyName,
                Name = chip.Value.TagName,
                Id = OwnerPrimaryKey.Value,
                Set = false
            });
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            await ReloadTags();
        }
    }

    private async Task<IEnumerable<string?>> Search(string value, CancellationToken token)
    {
        TPaginationRequestModel<SelectMetadataRequestModel> req = new()
        {
            Payload = new()
            {
                ApplicationsNames = [],
                IdentityUsersIds = [],
                PropertyName = "",
                OwnerPrimaryKey = 0,
                PrefixPropertyName = "",
                SearchQuery = value,
            },
            PageNum = 0,
            PageSize = 100,
            SortingDirection = DirectionsEnum.Down,
        };

        TPaginationResponseModel<TagViewModel> res = await TagsRepo.TagsSelectAsync(req, token);

        if (res.TotalRowsCount > req.PageSize)
            SnackBarRepo.Error($"Записей больше: {res.TotalRowsCount}");

        List<string> res_data = res.Response?.Where(x => TagsSets?.Any(y => y.TagName.Equals(x.TagName, StringComparison.OrdinalIgnoreCase)) != true).Select(x => x.TagName).ToList() ?? [];

        if (!string.IsNullOrWhiteSpace(value) && !res_data.Contains(value))
            res_data.Add(value);

        return res_data.DistinctBy(x => x.ToUpper());
    }

    async Task ReloadTags()
    {
        await SetBusyAsync();
        TPaginationRequestModel<SelectMetadataRequestModel> req = new()
        {
            Payload = new()
            {
                ApplicationsNames = this.ApplicationsNames,
                IdentityUsersIds = [],
                PropertyName = PropertyName,
                OwnerPrimaryKey = OwnerPrimaryKey,
                PrefixPropertyName = PrefixPropertyName,
            },
            PageNum = 0,
            PageSize = 100,
            SortingDirection = DirectionsEnum.Down,
        };

        TPaginationResponseModel<TagViewModel> res = await TagsRepo.TagsSelectAsync(req);

        if (res.TotalRowsCount > req.PageSize)
            SnackBarRepo.Error($"Записей больше: {res.TotalRowsCount}");

        await SetBusyAsync(false);
        if (res.Response is not null)
            TagsSets = res.Response;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await ReadCurrentUser();
        await ReloadTags();
    }
}