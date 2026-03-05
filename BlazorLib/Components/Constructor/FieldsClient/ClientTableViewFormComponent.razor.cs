////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

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
    [Parameter, EditorRequired]
    public required FormToTabJoinConstructorModelDB PageJoinForm { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public SessionOfDocumentDataModelDB? SessionOfDocumentData { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public TabOfDocumentSchemeConstructorModelDB? DocumentPage { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required FormConstructorModelDB Form { get; set; }

    /// <summary>
    /// Текущий пользователь (сессия)
    /// </summary>
    [Parameter]
    public UserInfoModel? CurrentUserSession { get; set; }



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
            { x => x.SessionDocument, SessionOfDocumentData },
            { x => x.DocumentPage, DocumentPage },
            { x => x.PageJoinForm, PageJoinForm },
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
        if (SessionOfDocumentData is null)
        {
            SnackBarRepo.Error("SessionDocument is null. error 6146B0D1-0BF3-4CA5-BBF5-5EA64ACA709E");
            return;
        }

        StateHasChanged();
        _ = InvokeAsync(async () =>
        {
            await SetBusyAsync();
            StateHasChanged();
            ValueFieldSessionDocumentDataBaseModel req = new() { GroupByRowNum = row_num, JoinFormId = PageJoinForm.Id, SessionId = SessionOfDocumentData.Id };
            ResponseBaseModel rest = await ConstructorRepo.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(req);
            
            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            if (!rest.Success())
            {
                SnackBarRepo.Error($"Ошибка E7BD5ADD-8CAF-434B-8AB8-94167CCB3337 Action: {rest.Message()}");
                await SetBusyAsync(false);
                return;
            }
            await ReloadSession();
            await SetBusyAsync(false);
        });
    }

    /// <inheritdoc/>
    protected async Task AddRowToTable()
    {
        if (SessionOfDocumentData is null)
        {
            SnackBarRepo.Error("SessionDocument is null. error DDD591F2-F3DE-4BF1-91DB-B0C4E5D7C93C");
            return;
        }

        FieldSessionDocumentDataBaseModel row_obj = new()
        {
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionOfDocumentData.Id
        };
        await SetBusyAsync();

        TResponseModel<int> rest = await ConstructorRepo.AddRowToTableAsync(row_obj);
        
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка B4812CDF-E4F0-46D5-981B-422DC3F966D7 Action: {rest.Message()}");
            await SetBusyAsync(false);
            return;
        }
        uint row_num = (uint)rest.Response;
        DialogParameters<ClientTableRowEditDialogComponent> parameters = new()
        {
            { x => x.RowNum, row_num },
            { x => x.SessionDocument, SessionOfDocumentData },
            { x => x.DocumentPage, DocumentPage },
            { x => x.PageJoinForm, PageJoinForm },
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<ClientTableRowEditDialogComponent>($"Созданная строка данных №{rest.Response}", parameters, options);
        ValueFieldSessionDocumentDataBaseModel req = new()
        {
            GroupByRowNum = row_num,
            JoinFormId = PageJoinForm.Id,
            SessionId = SessionOfDocumentData.Id,
            IsSelf = true
        };
        _ = await ConstructorRepo.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(req);
        await ReloadSession();
        await SetBusyAsync(false);
    }

    async Task ReloadSession()
    {
        if (SessionOfDocumentData is null)
        {
            SnackBarRepo.Error("SessionDocument is null. error BCBB2599-4CC1-433A-A5BC-21114935105F");
            return;
        }
        await SetBusyAsync();
        TResponseModel<SessionOfDocumentDataModelDB> rest = string.IsNullOrWhiteSpace(SessionOfDocumentData.SessionToken)
        ? await ConstructorRepo.GetSessionDocumentAsync(new() { SessionId = SessionOfDocumentData.Id, IncludeExtra = false })
        : await ConstructorRepo.GetSessionDocumentDataAsync(SessionOfDocumentData.SessionToken);
        
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка 3755827F-4811-4927-8ABC-66896D12803B Action: {rest.Message()}");
            await SetBusyAsync(false);
            return;
        }

        if (rest.Response is not null)
            SessionOfDocumentData.Reload(rest.Response);

        _table_kit_ref?.Update();
        await SetBusyAsync(false);
    }
}