////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <inheritdoc/>
public partial class KladrRowObjectComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ObjectKLADRModelDB ObjectKLADR { get; set; }


    bool Expanded = false;

    CodeKladrModel MetaData = default!;

    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        MetaData = CodeKladrModel.Build(ObjectKLADR.CODE);
        return base.OnInitializedAsync();
    }

    void OnExpandedChanged(bool newVal)
    {
        Expanded = newVal;
    }
}