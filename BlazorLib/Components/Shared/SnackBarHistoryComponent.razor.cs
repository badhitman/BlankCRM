////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using static BlazorLib.Extensions;
using MudBlazor;

namespace BlazorLib.Components.Shared;

/// <summary>
/// SnackBarHistoryComponent
/// </summary>
public partial class SnackBarHistoryComponent
{
    /// <summary>
    /// SnackBar
    /// </summary>
    [Inject]
    public ISnackbar SnackBarRepo { get; set; } = default!;


    string searchString1 = "";

    IEnumerable<MessageViewModel> Elements = [];

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        Elements = SnackBarRepo.GetHistoryMessages();
    }

    bool FilterFunc1(MessageViewModel? element) => FilterFunc(element, searchString1);

    bool FilterFunc(MessageViewModel? element, string searchString)
    {
        return element?.Message.Text?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true;
    }
}