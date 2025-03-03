////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections;
using System.Text;
using SharedLib;

namespace ToolsMauiLib;

public partial class ParseDBF(IClientHTTPRestService RemoteClient)
{
    public delegate void PartUploadHandler(int recordNum);
    public event PartUploadHandler? PartUploadNotify;

    public Encoding CurrentEncoding { get; set; } = Encoding.GetEncoding("cp866");

    byte[]? buffer;
    readonly ArrayList Columns = [];

    string? year, month, day;
    long lDate, lTime;
    int fieldIndex, dbfHeaderSize, FieldDescriptorHeaderSize;

    GCHandle handle;
    DBFHeader header;

    readonly List<object[]> DataList = [];

    MemoryStream? DbfFile;

    public async Task<int> Open(MemoryStream _dbfFile)
    {
        DbfFile = _dbfFile;
        DbfFile.Seek(0, SeekOrigin.Begin);

        dbfHeaderSize = Marshal.SizeOf<DBFHeader>();
        FieldDescriptorHeaderSize = Marshal.SizeOf<FieldDescriptor>();

        DataList.Clear();
        Columns.Clear();

        buffer = new byte[dbfHeaderSize];
        await DbfFile.ReadExactlyAsync(buffer, 0, dbfHeaderSize);

        handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        header = Marshal.PtrToStructure<DBFHeader>(handle.AddrOfPinnedObject());
        handle.Free();

        buffer = new byte[1];
        await DbfFile.ReadExactlyAsync(buffer, 0, 1);

        while (buffer.Length == 1 && 13 != buffer[0])
        {
            DbfFile.Seek(DbfFile.Position - 1, SeekOrigin.Begin);
            buffer = new byte[FieldDescriptorHeaderSize];
            await DbfFile.ReadExactlyAsync(buffer, 0, FieldDescriptorHeaderSize);

            handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Columns.Add(Marshal.PtrToStructure<FieldDescriptor>(handle.AddrOfPinnedObject()));
            handle.Free();
            buffer = new byte[1];
            await DbfFile.ReadExactlyAsync(buffer, 0, 1);
        }

        DbfFile.Seek(header.headerLen + 1, SeekOrigin.Begin);
        /*buffer = new byte[header.recordLen];
       await DbfFile.ReadExactlyAsync(buffer, 0, header.recordLen);
      recReader = new BinaryReader(new MemoryStream(buffer));
       foreach (FieldDescriptor field in Columns)
       {
           //number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
           //DataColumn col = new(field.fieldName, typeof(string));
           //DataTableCache.Columns.Add(col);
       }*/
        return header.numRecords;
    }

    /// <summary>
    /// Не более 55
    /// </summary>
    /// <param name="limit_row"></param>
    /// <returns></returns>
    public async Task<(List<object[]> TableData, FieldDescriptorBase[] Columns)> GetRandomRowsAsDataTable(int limit_row, bool del_row_inc = true)
    {
        if (DbfFile is null)
            throw new Exception("db file not set");

        DataList.Clear();
        FieldDescriptor[] _fields;
        if (header.numRecords <= 0)
        {
            _fields = [.. Columns.ToArray().Cast<FieldDescriptor>()];
            return (DataList, _fields.Select(FieldDescriptorBase.Build).ToArray());
        }

        if (limit_row <= 5)
            limit_row = 5;
        if (header.numRecords < limit_row)
            limit_row = header.numRecords;

        limit_row = Math.Min(55, limit_row);

        long old_file_position = DbfFile.Position;
        DbfFile.Seek(header.headerLen, SeekOrigin.Begin);
        Random rnd = new();
        rnd.Next(0, header.numRecords - 1);
        object[] s_row;
        BinaryReader recReader;
        for (int counter = 0; counter <= limit_row; counter++)
        {
            long random_position_row = rnd.Next(0, header.numRecords - 1) * header.recordLen;
            DbfFile.Seek(header.headerLen + random_position_row, SeekOrigin.Begin);
            buffer = new byte[header.recordLen];
            await DbfFile.ReadExactlyAsync(buffer, 0, header.recordLen);
            recReader = new BinaryReader(new MemoryStream(buffer));
            if (recReader.ReadChar() == '*')
            {
                if (!del_row_inc)
                    continue;
            }
            fieldIndex = 0;
            s_row = new object[Columns.Count];
            foreach (FieldDescriptor field in Columns)
            {
                switch (field.fieldType)
                {
                    case 'N':  // Number
                        string number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
                        if (IsNumber(number))
                            s_row[fieldIndex] = number;
                        else
                            s_row[fieldIndex] = "0";
                        break;
                    case 'C': // String
                        s_row[fieldIndex] = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));//s_row[fieldIndex] = CurrEnc.GetString(recReader.ReadBytes(field.fieldLen));
                        break;

                    case 'D': // Date (YYYYMMDD)
                        year = CurrentEncoding.GetString(recReader.ReadBytes(4));
                        month = CurrentEncoding.GetString(recReader.ReadBytes(2));
                        day = CurrentEncoding.GetString(recReader.ReadBytes(2));
                        s_row[fieldIndex] = DBNull.Value;
                        try
                        {
                            if (IsNumber(year) && IsNumber(month) && IsNumber(day))
                            {
                                if (int.Parse(year) > 1900)
                                {
                                    s_row[fieldIndex] = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                                }
                            }
                        }
                        catch
                        { }

                        break;

                    case 'T': // Timestamp, 8 bytes - two integers, first for date, second for time
                        // Date is the number of days since 01/01/4713 BC (Julian Days)
                        // Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L (Milliseconds since midnight)
                        lDate = recReader.ReadInt32();
                        lTime = recReader.ReadInt32() * 10000L;
                        s_row[fieldIndex] = JulianToDateTime(lDate).AddTicks(lTime);
                        break;

                    case 'L': // Boolean (Y/N)
                        if ('Y' == recReader.ReadByte())
                            s_row[fieldIndex] = "true";
                        else
                            s_row[fieldIndex] = "false";

                        break;

                    case 'F':
                        number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
                        if (IsNumber(number))
                            s_row[fieldIndex] = number;
                        else
                            s_row[fieldIndex] = "0.0";
                        break;
                }
                fieldIndex++;
            }
            recReader.Close();
            DataList.Add(s_row);
        }
        DbfFile.Seek(old_file_position, SeekOrigin.Begin);
        _fields = [.. Columns.ToArray().Cast<FieldDescriptor>()];

        return (GlobalTools.CreateDeepCopy(DataList)!, _fields.Select(FieldDescriptorBase.Build).ToArray());
    }

    public async Task UploadData(bool inc_del)
    {
        if (DbfFile is null)
            throw new Exception("db file not set");

        if (DataList is null)
            throw new Exception("data table not init");

        if (Columns is null)
            throw new Exception("columns table not created");

        DataList.Clear();

        //string table_name = "table_" + new System.Text.RegularExpressions.Regex(@"\W").Replace(System.IO.Path.GetFileName(FileOutputName), "_");        
        DbfFile.Seek(header.headerLen, SeekOrigin.Begin);
        object[] s_row;
        int data_list_Count = 0, del_rows_count = 0;
        BinaryReader recReader;
        byte[] readed_data_tmp;
        for (int counter = 0; counter <= header.numRecords - 1; counter++)
        {
            buffer = new byte[header.recordLen];
            await DbfFile.ReadExactlyAsync(buffer, 0, header.recordLen);
            if (buffer.Length == 0)
            {
                return;
            }

            recReader = new BinaryReader(new MemoryStream(buffer));
            if (recReader.ReadChar() == '*')
            {
                del_rows_count++;
                if (!inc_del)
                    continue;
            }
            fieldIndex = 0;
            s_row = new object[Columns.Count];
            foreach (FieldDescriptor field in Columns)
            {
                readed_data_tmp = recReader.ReadBytes(field.fieldLen);
                switch (field.fieldType)
                {
                    case 'N':
                        string number = CurrentEncoding.GetString(readed_data_tmp);

                        if (IsNumber(number))
                        {
                            if (field.count > 0)
                                s_row[fieldIndex] = double.Parse(number);
                            else
                            {
                                number = number
                                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                                s_row[fieldIndex] = int.Parse(number);
                            }
                        }
                        else
                        {
                            s_row[fieldIndex] = 0;
                        }
                        break;
                    case 'F':
                        number = CurrentEncoding.GetString(readed_data_tmp);
                        if (IsNumber(number))
                        {
                            number = number
                                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                            s_row[fieldIndex] = decimal.Parse(number);
                        }
                        else
                            s_row[fieldIndex] = 0.0;

                        break;
                    case 'C':
                        s_row[fieldIndex] = CurrentEncoding.GetString(readed_data_tmp).Trim();
                        break;
                    case 'D': // Date (YYYYMMDD)
                        year = CurrentEncoding.GetString(readed_data_tmp.SubArray(0, 4));
                        month = CurrentEncoding.GetString(readed_data_tmp.SubArray(4, 2));
                        day = CurrentEncoding.GetString(readed_data_tmp.SubArray(6, 2));
                        // s_row[fieldIndex] = ' ';
                        try
                        {
                            if (IsNumber(year) && IsNumber(month) && IsNumber(day))
                            {
                                if (int.Parse(year) > 1900)
                                    s_row[fieldIndex] = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                            }
                        }
                        catch
                        { }
                        break;
                    case 'T': // Timestamp, 8 bytes - two integers, first for date, second for time
                        // Date is the number of days since 01/01/4713 BC (Julian Days)
                        // Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L (Milliseconds since midnight)
                        lDate = BitConverter.ToInt32(readed_data_tmp, 0);
                        lTime = BitConverter.ToInt32(readed_data_tmp, 4);
                        s_row[fieldIndex] = JulianToDateTime(lDate).AddTicks(lTime);
                        break;
                    case 'L': // Boolean (Y/N)
                        s_row[fieldIndex] =
                            CurrentEncoding.GetString(readed_data_tmp).StartsWith("Y", StringComparison.OrdinalIgnoreCase) ||
                            CurrentEncoding.GetString(readed_data_tmp).StartsWith("T", StringComparison.OrdinalIgnoreCase);
                        break;
                }
                fieldIndex++;
            }
            recReader.Close();
            data_list_Count++;
            DataList.Add(s_row);
            if (data_list_Count >= 1000)
            {
                data_list_Count = 0;
                _ = await RemoteClient.UploadPartTempKladr(new()
                {
                    Columns = [.. Columns.Cast<FieldDescriptor>().Select(x => new FieldDescriptorBase() { FieldLen = x.fieldLen, FieldName = x.fieldName, FieldType = x.fieldType })],
                    RowsData = DataList
                });
                DataList.Clear();
                if (PartUploadNotify is not null)
                    PartUploadNotify(counter);
            }
        }
        if (DataList.Count != 0)
        {
            _ = await RemoteClient.UploadPartTempKladr(new()
            {
                Columns = [.. Columns.Cast<FieldDescriptor>().Select(x => new FieldDescriptorBase() { FieldLen = x.fieldLen, FieldName = x.fieldName, FieldType = x.fieldType })],
                RowsData = DataList
            });
            DataList.Clear();
            if (PartUploadNotify is not null)
                PartUploadNotify(header.numRecords);
        }
    }

    /*
    private string NamesFieldsSQL(bool ForCreateTable, string separator = ", ", string quote = "`")
    {
        foreach (FieldDescriptor field in fields)
        {
            if (ForCreateTable)
            {
                switch (field.fieldType)
                {
                    case 'N':
                        if (field.count > 0)
                        {
                            type_data += " DECIMAL(" + field.fieldLen + "," + field.count + ")";
                        }
                        else
                        {
                            type_data += " INT(" + field.fieldLen + ")";
                        }
                        break;
                    case 'C':
                        type_data += " VARCHAR(" + field.fieldLen + ")";
                        break;
                    case 'T':
                        //col = new DataColumn(field.fieldName, typeof(string));
                        type_data += " TIME";
                        break;
                    case 'D':
                        type_data += " DATE";
                        break;
                    case 'L':
                        type_data += " BOOLEAN";
                        break;
                    case 'F':
                        type_data += " DOUBLE(" + field.fieldLen + "," + field.count + ")";
                        break;
                }
                returned_data += type_data + " NOT NULL,\n";
            }
            else
                returned_data += type_data + separator;

        }
        
    }

    */

    static bool IsNumber(string numberString)
    {
        char[] numbers = numberString.ToCharArray();
        int number_count = 0, point_count = 0, space_count = 0;

        foreach (char number in numbers)
        {
            if (number >= 48 && number <= 57)
                number_count += 1;
            else if (number == 46)
                point_count += 1;
            else if (number == 32)
                space_count += 1;
            else
                return false;
        }

        return (number_count > 0 && point_count < 2);
    }

    static DateTime JulianToDateTime(long lJDN)
    {
        double p = Convert.ToDouble(lJDN);
        double s1 = p + 68569;
        double n = Math.Floor(4 * s1 / 146097);
        double s2 = s1 - Math.Floor((146097 * n + 3) / 4);
        double i = Math.Floor(4000 * (s2 + 1) / 1461001);
        double s3 = s2 - Math.Floor(1461 * i / 4) + 31;
        double q = Math.Floor(80 * s3 / 2447);
        double d = s3 - Math.Floor(2447 * q / 80);
        double s4 = Math.Floor(q / 11);
        double m = q + 2 - 12 * s4;
        double j = 100 * (n - 49) + i + s4;
        return new DateTime(Convert.ToInt32(j), Convert.ToInt32(m), Convert.ToInt32(d));
    }
}