////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;

namespace BlazorWebLib.Components;

/// <summary>
/// UserSelectInputComponent
/// </summary>
public partial class UserSelectInputComponent : LazySelectorComponent<UserInfoModel>
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;

    /// <summary>
    /// Selected chat
    /// </summary>
    [Parameter]
    public string? SelectedUser { get; set; }


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
        IsBusyProgress = false;
        
        if (rest.Response is not null)
        {
            TotalRowsCount = rest.TotalRowsCount;
            LoadedData.AddRange(rest.Response);

            if (PageNum == 0)
                LoadedData.Insert(0, new() { UserId = "", UserName = "Not select" });

            PageNum++;
        }
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedUser))
        {
            SelectedObject = new()
            {
                UserId = "",
                UserName = "Not select",
            };
            SelectHandleAction(SelectedObject);
            return;
        }

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> rest = await IdentityRepo.GetUsersIdentityAsync([SelectedUser]);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Response is null || rest.Response.Length == 0)
        {
            SnackBarRepo.Error($"Не найден запрашиваемый пользователь #{SelectedUser}");
            return;
        }
        SelectedObject = rest.Response.Single();
        _selectedValueText = SelectedObject.ToString();
        SelectHandleAction(SelectedObject);
    }
}