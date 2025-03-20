////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Kladr.main;

/// <summary>
/// HomeNodeKladrComponent
/// </summary>
public partial class HomeNodeKladrComponent : KladrNavBaseNodeComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required HouseKLADRModelDB ObjectKLADR { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required QueryNavKladrComponent Parent { get; set; }


    string? FullNameData;

    CodeKladrModel MetaData = default!;
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
        MetaData = CodeKladrModel.Build(ObjectKLADR.CODE);
        List<string> md = GetFullName(Parent);
        md.RemoveAt(md.Count - 1);
        FullNameData = string.Join(", ", md);

    }
}