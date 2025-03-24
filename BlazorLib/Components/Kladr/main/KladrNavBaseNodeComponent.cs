////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Kladr.main;

/// <summary>
/// KladrNavBaseNodeComponent
/// </summary>
public class KladrNavBaseNodeComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required KladrResponseModel Payload { get; set; }

    List<string>? _fullName;
    /// <inheritdoc/>
    public List<string> GetFullName(QueryNavKladrComponent Owner)
    {
        if (_fullName != null)
            return _fullName;

        _fullName = [];

        _fullName.Add($"{Payload.Socr} {Payload.Name}");
        QueryNavKladrComponent? parent = Owner.Parent;
        string? codeLikeFilter = Owner.CodeLikeFilter;
        while (parent is not null && !string.IsNullOrWhiteSpace(codeLikeFilter))
        {
            KladrResponseModel _el = parent.PartData.First(z => z.Code == codeLikeFilter);
            _fullName.Insert(0, $"{_el.Socr} {_el.Name}");
            codeLikeFilter = parent.CodeLikeFilter;
            parent = parent.Parent;
        }
        return _fullName;
    }
}