﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using MudBlazor;
using SharedLib;
using BlazorLib;
using BlazorLib.Components.Constructor;

namespace BlazorLib.Components.Constructor.FieldsClient;

/// <summary>
/// Client table view form
/// </summary>
public partial class ClientTableViewFormComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDialogService DialogServiceRepo { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? Title { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required FormToTabJoinConstructorModelDB PageJoinForm { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public SessionOfDocumentDataModelDB? SessionDocument { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public bool? InUse { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public TabOfDocumentSchemeConstructorModelDB? DocumentPage { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }

    /// <summary>
    /// Текущий пользователь (сессия)
    /// </summary>
    [CascadingParameter]
    public UserInfoModel? CurrentUser { get; set; }



    /// <inheritdoc/>
    protected bool TableCalculationKit { get; set; } = false;

    /// <inheritdoc/>
    protected TableCalculationKitComponent? _table_kit_ref;

    /// <inheritdoc/>
    protected void OpenEditRowAction(uint row_num)
    {
        DialogParameters<ClientTableRowEditDialogComponent> parameters = new()
        {
            { x => x.RowNum, row_num },
            { x => x.SessionDocument, SessionDocument },
            { x => x.DocumentPage, DocumentPage },
            { x => x.PageJoinForm, PageJoinForm },
            { x => x.ParentFormsPage, ParentFormsPage }
        };

        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        _ = InvokeAsync(async () =>
        {
            IDialogReference result = await DialogServiceRepo.ShowAsync<ClientTableRowEditDialogComponent>($"Редактирование строки данных №:{row_num}", parameters, options);
            
            await ReloadSession();
        });
    }

    /// <inheritdoc/>
    protected void DeleteRowAction(uint row_num)
    {
        if (SessionDocument is null)
        {
            SnackbarRepo.Error("SessionDocument is null. error 6146B0D1-0BF3-4CA5-BBF5-5EA64ACA709E");
            return;
        }

        StateHasChanged();
        _ = InvokeAsync(async () =>
        {
            await SetBusyAsync();
            StateHasChanged();
            ValueFieldSessionDocumentDataBaseModel req = new() { GroupByRowNum = row_num, JoinFormId = PageJoinForm.Id, SessionId = SessionDocument.Id };
            ResponseBaseModel rest = await ConstructorRepo.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(req);
            IsBusyProgress = false;

            SnackbarRepo.ShowMessagesResponse(rest.Messages);
            if (!rest.Success())
            {
                SnackbarRepo.Error($"Ошибка E7BD5ADD-8CAF-434B-8AB8-94167CCB3337 Action: {rest.Message()}");
                return;
            }
            await ReloadSession();
        });
    }

    /// <inheritdoc/>
    protected async Task AddRowToTable()
    {
        if (SessionDocument is null)
        {
            SnackbarRepo.Error("SessionDocument is null. error DDD591F2-F3DE-4BF1-91DB-B0C4E5D7C93C");
            return;
        }

        FieldSessionDocumentDataBaseModel row_obj = new()
        {
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionDocument.Id
        };
        await SetBusyAsync();

        TResponseModel<int> rest = await ConstructorRepo.AddRowToTableAsync(row_obj);
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Error($"Ошибка B4812CDF-E4F0-46D5-981B-422DC3F966D7 Action: {rest.Message()}");
            return;
        }
        uint row_num = (uint)rest.Response;
        DialogParameters<ClientTableRowEditDialogComponent> parameters = new()
        {
            { x => x.RowNum, row_num },
            { x => x.SessionDocument, SessionDocument },
            { x => x.DocumentPage, DocumentPage },
            { x => x.PageJoinForm, PageJoinForm },
            { x => x.ParentFormsPage, ParentFormsPage }
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<ClientTableRowEditDialogComponent>($"Созданная строка данных №{rest.Response}", parameters, options);
        ValueFieldSessionDocumentDataBaseModel req = new()
        {
            GroupByRowNum = row_num,
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionDocument.Id,
            IsSelf = true
        };
        _ = await ConstructorRepo.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(req);
        await ReloadSession();
    }

    async Task ReloadSession()
    {
        if (SessionDocument is null)
        {
            SnackbarRepo.Error("SessionDocument is null. error BCBB2599-4CC1-433A-A5BC-21114935105F");
            return;
        }
        await SetBusyAsync();
        TResponseModel<SessionOfDocumentDataModelDB> rest = string.IsNullOrWhiteSpace(SessionDocument.SessionToken)
        ? await ConstructorRepo.GetSessionDocumentAsync(new() { SessionId = SessionDocument.Id, IncludeExtra = false })
        : await ConstructorRepo.GetSessionDocumentDataAsync(SessionDocument.SessionToken);
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Error($"Ошибка 3755827F-4811-4927-8ABC-66896D12803B Action: {rest.Message()}");
            return;
        }

        if (rest.Response is not null)
            SessionDocument.Reload(rest.Response);

        _table_kit_ref?.Update();

        StateHasChanged();
    }
}