////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace SharedLib;

/// <summary>
/// This is the file header for a DBF. We do this special layout with everything packed so we can read straight from disk into the structure to populate it
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct DBFHeader
{
    /// <inheritdoc/>
    public byte version;
    /// <inheritdoc/>
    public byte updateYear;
    /// <inheritdoc/>
    public byte updateMonth;
    /// <inheritdoc/>
    public byte updateDay;
    /// <inheritdoc/>
    public int numRecords;
    /// <inheritdoc/>
    public short headerLen;
    /// <inheritdoc/>
    public short recordLen;
    /// <inheritdoc/>
    public short reserved1;
    /// <inheritdoc/>
    public byte incompleteTrans;
    /// <inheritdoc/>
    public byte encryptionFlag;
    /// <inheritdoc/>
    public int reserved2;
    /// <inheritdoc/>
    public long reserved3;
    /// <inheritdoc/>
    public byte MDX;
    /// <inheritdoc/>
    public byte language;
    /// <inheritdoc/>
    public short reserved4;
}