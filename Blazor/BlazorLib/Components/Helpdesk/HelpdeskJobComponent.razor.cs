////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.Helpdesk;

/// <summary>
/// HelpdeskJobComponent
/// </summary>
public partial class HelpdeskJobComponent : BlazorBusyComponentBaseModel
{
    HelpdeskJournalComponent _tab = default!;

    UsersAreasHelpdeskEnum? _selectedOption = UsersAreasHelpdeskEnum.Executor;
    /// <summary>
    /// SelectedOption
    /// </summary>
    public UsersAreasHelpdeskEnum? SelectedOption
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

    void SetTab(HelpdeskJournalComponent page)
    {
        _tab = page;
    }
}