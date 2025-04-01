////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace ToolsMauiLib;

/// <summary>
/// This is the file header for a DBF. We do this special layout with everything packed so we can read straight from disk into the structure to populate it
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct DBFHeader
{
    public byte version;
    public byte updateYear;
    public byte updateMonth;
    public byte updateDay;
    public int numRecords;
    public short headerLen;
    public short recordLen;
    public short reserved1;
    public byte incompleteTrans;
    public byte encryptionFlag;
    public int reserved2;
    public long reserved3;
    public byte MDX;
    public byte language;
    public short reserved4;
}