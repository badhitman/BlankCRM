////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Text field form UI
/// </summary>
public partial class TextFieldFormUIComponent : ComponentBase
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public FieldFormConstructorModelDB FieldObject { get; set; } = default!;

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public Action<FieldFormBaseLowConstructorModel, Type> StateHasChangedHandler { get; set; } = default!;

    /// <inheritdoc/>
    public bool IsMultiline
    {
        get => (bool?)FieldObject.GetMetadataValue(MetadataExtensionsFormFieldsEnum.IsMultiline, false) == true;
        private set
        {
            FieldObject.SetValueOfMetadata(MetadataExtensionsFormFieldsEnum.IsMultiline, value);
            StateHasChangedHandler(FieldObject, GetType());
        }
    }

    /// <summary>
    /// Параметр текстового поля формы
    /// </summary>
    public string? FieldParameter
    {
        get => FieldObject.GetMetadataValue(MetadataExtensionsFormFieldsEnum.Parameter, "")?.ToString();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                FieldObject.UnsetValueOfMetadata(MetadataExtensionsFormFieldsEnum.Parameter);
            else
                FieldObject.SetValueOfMetadata(MetadataExtensionsFormFieldsEnum.Parameter, value);
            StateHasChangedHandler(FieldObject, GetType());
        }
    }

    /// <inheritdoc/>
    public void Update(FieldFormBaseLowConstructorModel field)
    {
        FieldObject.Update(field);
        StateHasChanged();
    }
}