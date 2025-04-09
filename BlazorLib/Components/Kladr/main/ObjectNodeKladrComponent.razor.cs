////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Kladr.main;

/// <summary>
/// ObjectNodeKladrComponent.razor
/// </summary>
public partial class ObjectNodeKladrComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ObjectKLADRModelDB ObjectKLADR { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required QueryNavKladrComponent Parent { get; set; }


    bool Expanded = false;
    CodeKladrModel? MetaData;

    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        MetaData = CodeKladrModel.Build(ObjectKLADR.CODE);
        return base.OnInitializedAsync();
    }

    void OnExpandedChanged(bool newVal) => Expanded = newVal;
}