﻿////////////////////////////////////////////////
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
    IStorageTransmission TagsRepo { get; set; } = default!;


    List<TagModelDB> TagsSets { get; set; } = [];

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
            SnackbarRepo.ShowMessagesResponse(res.Messages);
        else
        {
            await ReloadTags();
            StateHasChanged();
        }
        _value = "";
        if (maRef is not null)
            await maRef.ClearAsync();
    }

    private async Task OnChipClosed(MudChip<TagModelDB> chip)
    {
        if (!string.IsNullOrWhiteSpace(chip.Value?.TagName) && OwnerPrimaryKey.HasValue)
        {
            await SetBusyAsync();

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
            await ReloadTags();
            await SetBusyAsync(false);
            if (!res.Success())
                SnackbarRepo.ShowMessagesResponse(res.Messages);
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
            PageSize = int.MaxValue,
            SortingDirection = DirectionsEnum.Down,
        };

        TPaginationResponseModel<TagModelDB> res = await TagsRepo.TagsSelectAsync(req, token);

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
            PageSize = int.MaxValue,
            SortingDirection = DirectionsEnum.Down,
        };

        TPaginationResponseModel<TagModelDB> res = await TagsRepo.TagsSelectAsync(req);

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