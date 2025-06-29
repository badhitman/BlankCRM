﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Document;

/// <summary>
/// Documents view
/// </summary>
public partial class DocumentsSchemesTableComponent : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Inject]
    protected IDialogService DialogServiceRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    MudTable<DocumentSchemeConstructorModelDB>? table;

    /// <inheritdoc/>
    protected string? searchString;
    TPaginationResponseModel<DocumentSchemeConstructorModelDB> data = new() { Response = [] };

    /// <inheritdoc/>
    protected static MarkupString Descr(string? html) => (MarkupString)(html ?? "");

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
    }

    /// <inheritdoc/>
    protected async Task DeleteDocument(int questionnaire_id)
    {
        await SetBusyAsync();

        ResponseBaseModel rest = await ConstructorRepo.DeleteDocumentSchemeAsync(new() { Payload = questionnaire_id, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Error($"Ошибка F1AADB25-31FF-4305-90A9-4B71184434CC Action: {rest.Message()}");
            return;
        }

        if (table is not null)
            await table.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    protected async Task<TableData<DocumentSchemeConstructorModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("No main/used project selected");

        SimplePaginationRequestModel req = new();
        await SetBusyAsync(token: token);

        data = await ConstructorRepo.RequestDocumentsSchemesAsync(new() { RequestPayload = req, ProjectId = ParentFormsPage.MainProject.Id }, token);
        IsBusyProgress = false;

        if (data.Response is null)
        {
            SnackbarRepo.Error($"rest.Content.Documents is null. error 62D3109B-7349-48E8-932B-762D5B0EA585");
            return new TableData<DocumentSchemeConstructorModelDB>() { TotalItems = data.TotalRowsCount, Items = data.Response };
        }

        return new TableData<DocumentSchemeConstructorModelDB>() { TotalItems = data.TotalRowsCount, Items = data.Response };
    }

    /// <inheritdoc/>
    protected async Task DocumentOpenDialog(DocumentSchemeConstructorModelDB? document_scheme = null)
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("No main/used project selected");

        document_scheme ??= DocumentSchemeConstructorModelDB.BuildEmpty(ParentFormsPage.MainProject.Id);
        DialogParameters<EditDocumentSchemeDialogComponent> parameters = new()
        {
            { x => x.DocumentScheme, document_scheme },
            { x => x.ParentFormsPage, ParentFormsPage },
        };

        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<EditDocumentSchemeDialogComponent>(document_scheme.Id < 1 ? "Creating a new questionnaire/survey" : $"Editing a questionnaire/survey #{document_scheme.Id}", parameters, options);
        if (table is not null)
            await table.ReloadServerData();
    }

    /// <inheritdoc/>
    protected async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}