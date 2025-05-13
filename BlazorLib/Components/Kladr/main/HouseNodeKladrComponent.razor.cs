////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Kladr.main;

/// <summary>
/// HouseNodeKladrComponent
/// </summary>
public partial class HouseNodeKladrComponent : KladrNavBaseNodeComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required HouseKLADRModelDB ObjectKLADR { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required QueryNavKladrComponent Parent { get; set; }


    string? FullNameData;

    CodeKladrModel? _md;
    CodeKladrModel MetaData
    {
        get
        {
            _md ??= CodeKladrModel.Build(ObjectKLADR.CODE);
            return _md;
        }
    }

    string[]? _names;
    string[] Names
    {
        get
        {
            _names ??= ObjectKLADR.NAME.Split(",");
            return _names;
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        List<KladrBaseElementModel> md = GetNamesScheme(Parent);
        md.RemoveAt(md.Count - 1);
        FullNameData = string.Join(", ", md.Select(x => x.ToString()));
    }
}