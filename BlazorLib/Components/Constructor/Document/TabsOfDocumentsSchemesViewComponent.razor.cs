////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Document;

/// <summary>
/// Pages questionnaires view
/// </summary>
public partial class TabsOfDocumentsSchemesViewComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DocumentSchemeConstructorModelDB DocumentScheme { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    /// <inheritdoc/>
    protected bool _stateHasChanged;
    /// <inheritdoc/>
    public int DocumentIndex;

    /// <inheritdoc/>
    protected IEnumerable<FormBaseConstructorModel> AllForms = default!;

    bool _tabs_is_hold = false;
    /// <inheritdoc/>
    protected void SetHoldAction(bool _is_hold)
    {
        _tabs_is_hold = _is_hold;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected bool TabIsDisabled(int questionnaire_page_id)
    {
        return _tabs_is_hold && DocumentScheme.Tabs?.FindIndex(x => x.Id == questionnaire_page_id) != DocumentIndex;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("No main/used project selected");

        SelectFormsModel reqForms = new()
        {
            ProjectId = ParentFormsPage.MainProject.Id,
            Request = AltSimplePaginationRequestModel.Build(null, 100, 0, true)
        };
        await SetBusyAsync();
        TPaginationResponseModel<FormConstructorModelDB> rest = await ConstructorRepo.SelectFormsAsync(reqForms);
        
        if (rest.TotalRowsCount > rest.PageSize)
            SnackBarRepo.Error($"Записей больше: {rest.TotalRowsCount}");

        if (rest.Response is null)
            throw new Exception($"Ошибка 973D18EE-ED49-442D-B12B-CDC5A32C8A51 rest.Content.Elements is null");

        AllForms = rest.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected void DocumentReloadAction()
    {
        InvokeAsync(async () =>
        {
            await SetBusyAsync();
            StateHasChanged();
            TResponseModel<DocumentSchemeConstructorModelDB> rest = await ConstructorRepo.GetDocumentSchemeAsync(DocumentScheme.Id);
            
            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            if (rest.Response is null)
            {
                SnackBarRepo.Error($"Ошибка A3D19BFC-38BB-4932-85DD-54C306D8C83E rest.Content.DocumentScheme");
                await SetBusyAsync(false);
                return;
            }
            if (!rest.Success())
            {
                SnackBarRepo.Error($"Ошибка 55685D9E-E9D0-4937-A727-5BCC9FAD4381 Action: {rest.Message()}");
                await SetBusyAsync(false);
                return;
            }
            DocumentScheme = rest.Response;
            await SetBusyAsync(false);
        });
    }

    /// <inheritdoc/>
    protected void SetIdForPageAction(int init_id, TabOfDocumentSchemeConstructorModelDB new_page)
    {
        if (PagesNotExist)
        {
            SnackBarRepo.Error($"PagesNotExist. Ошибка 6264F83F-DF3F-4860-BC18-5288AB335985");
            return;
        }
        int i = DocumentScheme.Tabs!.FindIndex(x => x.Id == init_id);
        if (i >= 0 && init_id != new_page.Id)
            new_page.Id = i;

        i = DocumentScheme.Tabs.FindIndex(x => x.Id == new_page.Id);
        if (i >= 0)
            DocumentScheme.Tabs[i].JoinsForms = new_page.JoinsForms;

        StateHasChanged();
    }

    /// <inheritdoc/>
    protected bool CanUpPage(TabOfDocumentSchemeConstructorModelDB questionnaire_page) => questionnaire_page.SortIndex > (DocumentScheme.Tabs!.Any(x => x.Id != questionnaire_page.Id) ? DocumentScheme.Tabs!.Where(x => x.Id != questionnaire_page.Id)!.Min(y => y.SortIndex) : 1) && !DocumentScheme.Tabs!.Any(x => x.Id < 1);
    /// <inheritdoc/>
    protected bool CanDownPage(TabOfDocumentSchemeConstructorModelDB questionnaire_page) => questionnaire_page.SortIndex < (DocumentScheme.Tabs!.Any(x => x.Id != questionnaire_page.Id) ? DocumentScheme.Tabs!.Where(x => x.Id != questionnaire_page.Id)!.Max(y => y.SortIndex) : DocumentScheme.Tabs!.Count) && !DocumentScheme.Tabs!.Any(x => x.Id < 1);

    bool PagesNotExist => DocumentScheme.Tabs is null || DocumentScheme.Tabs.Count == 0;

    /// <inheritdoc/>
    protected void SetNameForPage(int id, string name)
    {
        if (PagesNotExist)
        {
            SnackBarRepo.Error($"PagesNotExist. Ошибка 5FD8058E-50B0-49DA-8808-DB7C2BED80C2");
            return;
        }

        TabOfDocumentSchemeConstructorModelDB? _page = DocumentScheme.Tabs!.FirstOrDefault(x => x.Id == id);
        if (_page is null)
        {
            SnackBarRepo.Error($"_page is null. Ошибка CBDF9789-4A28-4869-A214-A4702A432DA6");
            return;
        }
        _page.Name = name;
        StateHasChanged();
    }

    /// <inheritdoc/>
    public void AddTab()
    {
        DocumentScheme.Tabs ??= [];
        int new_page_id = PagesNotExist
            ? 0
            : DocumentScheme.Tabs.Min(x => x.Id) - 1;

        if (new_page_id > 0)
            new_page_id = 0;

        int i = 1;
        while (i <= 100 && DocumentScheme.Tabs.Any(x => x.Name.Equals($"New {i}", StringComparison.OrdinalIgnoreCase)))
            i++;

        DocumentScheme.Tabs.Add(new TabOfDocumentSchemeConstructorModelDB { OwnerId = DocumentScheme.Id, Id = new_page_id, Name = $"New {(i < 100 ? i.ToString() : Guid.NewGuid().ToString())}", JoinsForms = new(), SortIndex = (DocumentScheme.Tabs.Any() ? DocumentScheme.Tabs.Max(x => x.SortIndex) + 1 : 1) });
        DocumentIndex = DocumentScheme.Tabs.Count - 1;
        _stateHasChanged = true;
    }

    /// <inheritdoc/>
    public void Update(DocumentSchemeConstructorModelDB document_scheme, TabOfDocumentSchemeConstructorModelDB? page = null)
    {
        DocumentScheme = document_scheme;
        if (page is not null && DocumentScheme.Tabs?.Any(x => x.Id == page.Id) == true)
            DocumentIndex = DocumentScheme.Tabs.FindIndex(x => x.Id == page.Id);
        StateHasChanged();
    }

    /// <inheritdoc/>
    public void RemoveTab(int id)
    {
        if (id == int.MinValue)
            return;

        TabOfDocumentSchemeConstructorModelDB? tabView = DocumentScheme.Tabs!.SingleOrDefault((t) => Equals(t.Id, id));
        if (tabView is not null)
        {
            DocumentScheme.Tabs!.Remove(tabView);
            _stateHasChanged = true;
        }
    }
    /// <inheritdoc/>
    protected void CloseTabCallback(MudTabPanel panel)
    {
        if (panel.ID is null)
            return;

        RemoveTab((int)panel.ID);
    }
}