////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// Журнал документов (универсальный)
/// </summary>
public partial class JournalUniversalComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IJournalUniversalService JournalRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavigationRepo { get; set; } = default!;

    /// <summary>
    /// Тип документа
    /// </summary>
    [Parameter, EditorRequired]
    public required string DocumentNameOrIdType { get; set; }

    /// <summary>
    /// ProjectId
    /// </summary>
    [Parameter]
    public int? ProjectId { get; set; }

    /// <summary>
    /// Отображать навигацию между журналами
    /// </summary>
    [Parameter]
    public bool ShowNavigation { get; set; }

    string? SelectedJournal
    {
        get => DocumentNameOrIdType;
        set
        {
            if (value != DocumentNameOrIdType)
                NavigationRepo.NavigateTo($"/documents-journal/{value}");
        }
    }

    DocumentSchemeConstructorModelDB[]? DocumentsSchemes { get; set; }

    private string? searchString;

    /// <summary>
    /// ColumnsNames
    /// </summary>
    EntryAltModel[]? ColumnsNames { get; set; }

    private int totalItems;

    private MudTable<KeyValuePair<int, Dictionary<string, object>>>? table;

    EntryAltTagModel[]? MySchemas = null;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KeyValuePair<int, Dictionary<string, object>>>> ServerReload(TableState state, CancellationToken token)
    {
        if (DocumentNameOrIdType is null)
            throw new Exception();

        await SetBusyAsync(token: token);
        TPaginationResponseModel<KeyValuePair<int, Dictionary<string, object>>> res = await JournalRepo
            .SelectJournalPartAsync(new SelectJournalPartRequestModel()
            {
                SearchString = searchString,
                DocumentNameOrId = DocumentNameOrIdType,
                SortBy = state.SortLabel,
                PageNum = state.Page,
                PageSize = state.PageSize,
                SortingDirection = state.SortDirection.Convert()
            }, ProjectId, token);

        totalItems = res.TotalRowsCount;
        await SetBusyAsync(false, token);

        return new TableData<KeyValuePair<int, Dictionary<string, object>>>() { TotalItems = totalItems, Items = res.Response };
    }

    private void OnSearch(string text)
    {
        searchString = text;
        table?.ReloadServerData();
    }

    /// <inheritdoc/>
    protected override async void OnInitialized()
    {
        if (string.IsNullOrWhiteSpace(DocumentNameOrIdType) && !ShowNavigation)
            throw new Exception();

        if (ShowNavigation)
        {
            await SetBusyAsync();
            MySchemas = await JournalRepo.GetMyDocumentsSchemas();

            if (MySchemas.Length != 0 && !MySchemas.Any(x => x.Id == DocumentNameOrIdType))
            {
                SelectedJournal = MySchemas[0].Id;
                await SetBusyAsync(false);
                return;
            }
        }
        await SetBusyAsync();
        TResponseModel<DocumentSchemeConstructorModelDB[]?> res_fs = await JournalRepo.FindDocumentSchemes(DocumentNameOrIdType, ProjectId);
        DocumentsSchemes = res_fs.Response;
        await SetBusyAsync(false);

        if (SelectedJournal is null)
            ColumnsNames = null;
        else
        {
            await SetBusyAsync();
            TResponseModel<EntryAltModel[]?> res = await JournalRepo.GetColumnsForJournalAsync(SelectedJournal, ProjectId);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            ColumnsNames = res.Response;
        }
        if (table is not null)
            await table.ReloadServerData();

        await SetBusyAsync(false);
    }
}