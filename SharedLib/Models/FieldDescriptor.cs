////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace SharedLib;

/// <summary>
/// This is the field descriptor structure. There will be one of these for each column in the table.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct FieldDescriptor
{
    /// <inheritdoc/>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
    public string fieldName;

    /// <inheritdoc/>
    public char fieldType;

    /// <inheritdoc/>
    public int address;

    /// <inheritdoc/>
    public byte fieldLen;

    /// <inheritdoc/>
    public byte count;

    /// <inheritdoc/>
    public short reserved1;

    /// <inheritdoc/>
    public byte workArea;

    /// <inheritdoc/>
    public short reserved2;

    /// <inheritdoc/>
    public byte flag;

    /// <inheritdoc/>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public required byte[] reserved3;

    /// <inheritdoc/>
    public byte indexFlag;
}