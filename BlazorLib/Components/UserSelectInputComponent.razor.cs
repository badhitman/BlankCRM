////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// UserSelectInputComponent
/// </summary>
public partial class UserSelectInputComponent : LazySelectorComponent<UserInfoModel>
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;

    /// <summary>
    /// Selected user
    /// </summary>
    [Parameter]
    public string? SelectedUserInit { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? StyleElement { get; set; }

    /// <inheritdoc/>
    public override async Task LoadPartData()
    {
        await SetBusyAsync();
        TPaginationResponseModel<UserInfoModel> rest = await IdentityRepo
            .SelectUsersOfIdentityAsync(new()
            {
                Payload = new() { SearchQuery = _selectedValueText },
                PageNum = PageNum,
                PageSize = page_size,
            });
        
        if (rest.Response is not null)
        {
            TotalRowsCount = rest.TotalRowsCount;
            LoadedData.AddRange(rest.Response);

            if (PageNum == 0)
                LoadedData.Insert(0, new() { UserId = "", UserName = "Not select" });

            PageNum++;
        }
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public void ClearInput()
    {
        SelectedObject = UserInfoModel.BuildEmpty();
        SelectHandleAction(SelectedObject);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedUserInit))
        {
            SelectedObject = UserInfoModel.BuildEmpty();
            SelectHandleAction(SelectedObject);
            return;
        }

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> rest = await IdentityRepo.GetUsersOfIdentityAsync([SelectedUserInit]);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Response is null || rest.Response.Length == 0)
        {
            SnackBarRepo.Error($"Не найден запрашиваемый пользователь #{SelectedUserInit}");
            return;
        }
        SelectedObject = rest.Response.Single();
        _selectedValueText = SelectedObject.ToString();
        SelectHandleAction(SelectedObject);
    }
}