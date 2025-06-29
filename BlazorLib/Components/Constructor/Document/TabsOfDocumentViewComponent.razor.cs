////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Document;

/// <summary>
/// Page questionnaire forms - view
/// </summary>
public partial class TabsOfDocumentViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// DocumentScheme page
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required TabOfDocumentSchemeConstructorModelDB TabOfDocumentScheme { get; set; }


    int _join_form_id;

    /// <summary>
    /// Join form
    /// </summary>
    protected void JoinFormHoldAction(int join_form_id)
    {
        _join_form_id = join_form_id;
        StateHasChanged();
    }

    /// <summary>
    /// Update page
    /// </summary>
    protected void UpdatePageAction(TabOfDocumentSchemeConstructorModelDB? page = null)
    {
        if (page is not null)
        {
            TabOfDocumentScheme = page;
            StateHasChanged();
            return;
        }
        _ = InvokeAsync(async () =>
        {
            await SetBusyAsync();
            StateHasChanged();
            TResponseModel<TabOfDocumentSchemeConstructorModelDB> rest = await ConstructorRepo.GetTabOfDocumentSchemeAsync(TabOfDocumentScheme.Id);
            IsBusyProgress = false;

            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            if (!rest.Success())
            {
                SnackBarRepo.Error($"Ошибка 16188CA3-EC20-4743-A31C-DA497CABDEB5 Action: {rest.Message()}");
                return;
            }
            if (rest.Response is null)
            {
                SnackBarRepo.Error($"Ошибка E7427B3A-68CB-4560-B2E0-4AF69F2EDA72 [rest.Content.DocumentPage is null]");
                return;
            }
            TabOfDocumentScheme = rest.Response;
            TabOfDocumentScheme.JoinsForms = TabOfDocumentScheme.JoinsForms?.OrderBy(x => x.SortIndex).ToList();
            StateHasChanged();
        });
    }

    /// <summary>
    /// Форму можно сдвинуть выше?
    /// </summary>
    protected bool CanUpJoinForm(FormToTabJoinConstructorModelDB pjf)
    {
        int min_index = TabOfDocumentScheme.JoinsForms?.Any(x => x.Id != pjf.Id) == true
        ? TabOfDocumentScheme.JoinsForms.Where(x => x.Id != pjf.Id).Min(x => x.SortIndex)
        : 1;
        return _join_form_id == 0 && pjf.SortIndex > min_index;
    }

    /// <summary>
    /// Форму можно сдвинуть ниже?
    /// </summary>
    protected bool CanDownJoinForm(FormToTabJoinConstructorModelDB pjf)
    {
        int max_index = TabOfDocumentScheme.JoinsForms?.Any(x => x.Id != pjf.Id) == true
        ? TabOfDocumentScheme.JoinsForms.Where(x => x.Id != pjf.Id).Max(x => x.SortIndex)
        : 1;
        return _join_form_id == 0 && pjf.SortIndex < max_index;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (TabOfDocumentScheme.JoinsForms is null)
        {
            SnackBarRepo.Info($"Дозагрузка `{nameof(TabOfDocumentScheme.JoinsForms)}` в `{nameof(TabOfDocumentScheme)} ['{TabOfDocumentScheme.Name}' #{TabOfDocumentScheme.Id}]`");
            await SetBusyAsync();
            TResponseModel<TabOfDocumentSchemeConstructorModelDB> rest = await ConstructorRepo.GetTabOfDocumentSchemeAsync(TabOfDocumentScheme.Id);
            IsBusyProgress = false;

            SnackBarRepo.ShowMessagesResponse(rest.Messages);
            TabOfDocumentScheme.JoinsForms = rest.Response?.JoinsForms;
        }
    }
}