////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace ToolsMauiLib;

// This is the field descriptor structure. There will be one of these for each column in the table.
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct FieldDescriptor
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
    public string fieldName;
    public char fieldType;
    public int address;
    public byte fieldLen;
    public byte count;
    public short reserved1;
    public byte workArea;
    public short reserved2;
    public byte flag;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public byte[] reserved3;
    public byte indexFlag;
}