﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// ConnectionConfigComponent
/// </summary>
public partial class ConnectionConfigComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;

    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    IClientRestToolsService RestClientRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action ParentUpdate { get; set; }

    /// <inheritdoc/>
    [Parameter,EditorRequired]
    public required Action<int> SetActiveHandler { get; set; }


    /// <summary>
    /// Форма подключения заполнена?
    /// </summary>
    bool ValidateForm => !string.IsNullOrWhiteSpace(TokenAccess) &&
        !string.IsNullOrWhiteSpace(AddressBaseUri);

    /// <summary>
    /// Форма изменена?
    /// </summary>
    public bool IsEdited =>
        ApiConnect.Name != Name ||
        ApiConnect.HeaderName != HeaderName ||
        ApiConnect.TokenAccess != TokenAccess ||
        ApiConnect.AddressBaseUri != AddressBaseUri;

    /// <summary>
    /// Форма может быть сохранена?
    /// </summary>
    /// <remarks>
    /// Если форма изменена и корректно заполнена
    /// </remarks>
    bool CanSave => IsEdited && ValidateForm;

    /// <summary>
    /// Expander form
    /// </summary>
    public MudExpansionPanel? ExpFormRef;

    string name = string.Empty;
    string Name
    {
        get => name;
        set
        {
            name = value;
        }
    }

    string tokenAccess = string.Empty;
    string TokenAccess
    {
        get => tokenAccess;
        set
        {
            tokenAccess = value;
        }
    }

    string addressBaseUri = string.Empty;
    string AddressBaseUri
    {
        get => addressBaseUri;
        set
        {
            addressBaseUri = value;
        }
    }

    string headerName = "token-access";
    string HeaderName
    {
        get => headerName;
        set
        {
            headerName = value;
        }
    }

    /// <summary>
    /// getMe
    /// </summary>
    public TResponseModel<ExpressProfileResponseModel>? GetMe { get; set; }

    CancellationTokenSource cancelTokenSource = new();
    CancellationToken? token;

    /// <summary>
    /// Проверить подключение
    /// </summary>
    /// <param name="testForm">Если требуется проверить настройки из формы</param>
    public async Task TestConnect(bool testForm = false)
    {
        await SetBusyAsync();
        ApiRestConfigModelDB? backupConf = null;
        if (testForm)
        {
            backupConf = GlobalTools.CreateDeepCopy(ApiConnect);

            ApiConnect.AddressBaseUri = AddressBaseUri;
            ApiConnect.HeaderName = HeaderName;
            ApiConnect.TokenAccess = TokenAccess;
        }
        token = cancelTokenSource.Token;
        GetMe = await RestClientRepo.GetMeAsync(token.Value);

        if (backupConf is not null)
            ApiConnect.Update(backupConf);

        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(GetMe.Messages);

        if (!testForm && ExpFormRef is not null && GetMe.Success())
            await ExpFormRef.CollapseAsync();

        ParentUpdate();
    }

    async Task SaveToken()
    {
        ApiRestConfigModelDB req = new()
        {
            Id = ApiConnect.Id,
            Name = Name,
            AddressBaseUri = AddressBaseUri,
            TokenAccess = TokenAccess,
            HeaderName = HeaderName,
        };
        await SetBusyAsync();
        TResponseModel<int> res = await AppManagerRepo.UpdateOrCreateConfigAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (res.Success())
            SetActiveHandler(req.Id > 0 ? req.Id : res.Response);
    }

    /// <summary>
    /// ResetForm
    /// </summary>
    public void ResetForm()
    {
        name = ApiConnect.Name;
        tokenAccess = ApiConnect.TokenAccess;
        addressBaseUri = ApiConnect.AddressBaseUri;
        headerName = ApiConnect.HeaderName;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();
        ResetForm();
        if (ApiConnect.Id != 0)
           await TestConnect();
    }
}