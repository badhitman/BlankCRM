////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using HtmlGenerator.html5.forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// CustomersBanksTableComponent
/// </summary>
public partial class CustomersBanksTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission identityRepo { get; set; } = default!;


    MudTable<CustomerBankIdModelDB>? table;
    string? searchString;

    BanksIdentifyTypesEnum bankInt;
    string? customerName;
    string? customerInn;
    string? userIdentityId;
    CustomerBankIdModelDB? elementBeforeEdit;
    UserSelectInputComponent? _usRef;

    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];


    /// <summary>
    /// CacheUsersUpdate
    /// </summary>
    protected async Task CacheUsersUpdate(IEnumerable<string> usersIds)
    {
        usersIds = usersIds.Where(x => !string.IsNullOrWhiteSpace(x) && !UsersCache.Any(y => y.UserId == x)).Distinct();
        if (!usersIds.Any())
            return;

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> users = await identityRepo.GetUsersIdentityAsync(usersIds);
        SnackBarRepo.ShowMessagesResponse(users.Messages);
        if (users.Success() && users.Response is not null && users.Response.Length != 0)
            lock (UsersCache)
            {
                UsersCache.AddRange(users.Response.Where(x => !UsersCache.Any(y => y.UserId == x.UserId)));
            }

        await SetBusyAsync(false);
    }
    void BackupItem(object element)
    {
        if (element is CustomerBankIdModelDB _re)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(_re);
    }

    async void ItemHasBeenCommitted(object element)
    {
        if(string.IsNullOrWhiteSpace(userIdentityId))
        {
            SnackBarRepo.Error($"User not set");
            return;
        }

        if (element is CustomerBankIdModelDB _re)
        {
            await SetBusyAsync();
            _re.UserIdentityId = userIdentityId;
            TResponseModel<int> res = await BankRepo.CustomerBankCreateOrUpdateAsync(_re);
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (table is not null)
                await table.ReloadServerData();

            elementBeforeEdit = null;
            userIdentityId = null;
            await SetBusyAsync(false);
        }
        else
            SnackBarRepo.Error($"can`t detect type: {element.GetType().Name}");

        _usRef?.ClearInput();
    }

    void ResetItemToOriginalValues(object element)
    {
        ((CustomerBankIdModelDB)element).Inn = elementBeforeEdit!.Inn;
        ((CustomerBankIdModelDB)element).Name = elementBeforeEdit.Name;
        ((CustomerBankIdModelDB)element).UserIdentityId = elementBeforeEdit.UserIdentityId;
        ((CustomerBankIdModelDB)element).BankIdentifyType = elementBeforeEdit.BankIdentifyType;
    }

    void SelectUserHandler(UserInfoModel selectedUser)
    {
        userIdentityId = selectedUser.UserId;
        StateHasChanged();
    }

    async Task AddCustomer()
    {
        if (string.IsNullOrWhiteSpace(userIdentityId) || (string.IsNullOrWhiteSpace(customerName) && string.IsNullOrWhiteSpace(customerInn)))
        {
            SnackBarRepo.Error("set name or inn for create customer identity (and choice user!)");
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await BankRepo.CustomerBankCreateOrUpdateAsync(new CustomerBankIdModelDB()
        {
            UserIdentityId = userIdentityId,
            Name = customerName,
            Inn = customerInn,
            BankIdentifyType = bankInt
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (table is not null)
            await table.ReloadServerData();

        customerName = null;
        customerInn = null;
        userIdentityId = null;

        _usRef?.ClearInput();

        await SetBusyAsync(false);
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<CustomerBankIdModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert()
        };
        TPaginationResponseModel<CustomerBankIdModelDB> res = await BankRepo.CustomersBanksSelectAsync(req, token);
        await CacheUsersUpdate(res.Response!.Select(x => x.UserIdentityId).Distinct());
        return new TableData<CustomerBankIdModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}