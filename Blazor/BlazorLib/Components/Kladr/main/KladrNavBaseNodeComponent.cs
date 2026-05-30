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


    List<KladrBaseElementModel>? _fullName;
    /// <inheritdoc/>
    public List<KladrBaseElementModel> GetNamesScheme(QueryNavKladrComponent Owner)
    {
        if (_fullName is not null)
            return _fullName;

        _fullName = [];
        _fullName.Add(KladrBaseElementModel.Build(Payload));

        string? codeLikeFilter = Owner.CodeLikeFilter;
        QueryNavKladrComponent? parent = Owner.Parent;
        while (parent is not null && !string.IsNullOrWhiteSpace(codeLikeFilter))
        {
            KladrResponseModel _el = parent.PartData.First(z => z.Code == codeLikeFilter);
            _fullName.Insert(0, KladrBaseElementModel.Build(_el));
            codeLikeFilter = parent.CodeLikeFilter;
            parent = parent.Parent;
        }
        return _fullName;
    }
}