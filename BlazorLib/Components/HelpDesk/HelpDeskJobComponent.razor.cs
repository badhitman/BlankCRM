////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk;

/// <summary>
/// HelpDeskJobComponent
/// </summary>
public partial class HelpDeskJobComponent : BlazorBusyComponentBaseModel
{
    HelpDeskJournalComponent _tab = default!;

    UsersAreasHelpDeskEnum? _selectedOption = UsersAreasHelpDeskEnum.Executor;
    /// <summary>
    /// SelectedOption
    /// </summary>
    public UsersAreasHelpDeskEnum? SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            _tab.SetArea(value);
            InvokeAsync(UpdateState);
        }
    }

    async Task UpdateState()
    {
        await SetBusyAsync();
        await _tab.TableRef.ReloadServerData();

        _tab.StateHasChangedCall();

        await SetBusyAsync(false);
    }

    void SetTab(HelpDeskJournalComponent page)
    {
        _tab = page;
    }
}