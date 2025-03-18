////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <summary>
/// LinksKladrComponent
/// </summary>
public partial class LinksKladrComponent
{
    [Inject]
    IJSRuntime JSRepo { get; set; } = default!;


    /// <summary>
    /// Почтовый индекс
    /// </summary>
    [Parameter, EditorRequired]
    public string? PostIndex { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required QueryNavKladrComponent Owner { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required KladrResponseModel Payload { get; set; }


    async Task GoToMap()
    {
        string resUri = $"{Payload.Socr} {Payload.Name}";

        QueryNavKladrComponent? parent = Owner.Parent;
        string? codeLikeFilter = Owner.CodeLikeFilter;
        while (parent is not null && !string.IsNullOrWhiteSpace(codeLikeFilter))
        {
            KladrResponseModel _el = parent.PartData.First(z => z.Code == codeLikeFilter);
            resUri = $"{_el.Socr} {_el.Name}, {resUri}";
            codeLikeFilter = parent.CodeLikeFilter;
            parent = parent.Parent;
        }
        await JSRepo.InvokeVoidAsync("open", $"https://yandex.ru/maps?text={resUri}&source=serp_navig", "_blank");
    }
}