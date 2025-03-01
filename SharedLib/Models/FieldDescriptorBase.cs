////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FieldDescriptorBase
/// </summary>
public struct FieldDescriptorBase
{
    /// <inheritdoc/>
    public string FieldName { get; set; }

    /// <inheritdoc/>
    public char FieldType { get; set; }

    /// <inheritdoc/>
    public byte FieldLen { get; set; }

    /// <inheritdoc/>
    public static FieldDescriptorBase Build(FieldDescriptor x)
    {
        return new()
        {
            FieldLen = x.fieldLen,
            FieldName = x.fieldName,
            FieldType = x.fieldType
        };
    }
}