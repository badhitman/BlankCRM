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
            FieldName = x.fieldName,
            FieldType = x.fieldType,
            FieldLen = x.fieldLen,
        };
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return $"{FieldName} /{FieldType} ({FieldLen})";
    }
}