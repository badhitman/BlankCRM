////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
using System.Data;
using SharedLib;

namespace ToolsMauiLib;

public partial class ParseDBF
{
    public Encoding CurrentEncoding { get; set; } = Encoding.GetEncoding("cp866");

    BinaryReader? recReader;
    byte[]? buffer;
    ArrayList? Columns;

    string? year, month, day;
    long lDate, lTime;
    int fieldIndex, cur_num_row, dbfHeaderSize, FieldDescriptorHeaderSize;

    GCHandle handle;
    DBFHeader header;

    List<object[]>? DataList;
    DataTable? DataTableCache;

    MemoryStream? DbfFile;

    public async Task Init(Stream _dbfFile)
    {
        DataTableCache = new DataTable();

        DbfFile = new();
        await _dbfFile.CopyToAsync(DbfFile);
        DbfFile.Seek(0, SeekOrigin.Begin);

        dbfHeaderSize = Marshal.SizeOf<DBFHeader>();
        FieldDescriptorHeaderSize = Marshal.SizeOf<FieldDescriptor>();

        DataList = new List<object[]>(header.numRecords);

        buffer = new byte[dbfHeaderSize];
        await DbfFile.ReadExactlyAsync(buffer, 0, dbfHeaderSize);

        handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        header = Marshal.PtrToStructure<DBFHeader>(handle.AddrOfPinnedObject());
        handle.Free();

        Columns = [];
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
        buffer = new byte[header.recordLen];
        await DbfFile.ReadExactlyAsync(buffer, 0, header.recordLen);
        recReader = new BinaryReader(new MemoryStream(buffer));
        foreach (FieldDescriptor field in Columns)
        {
            //number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
            DataColumn col = new(field.fieldName, typeof(string));
            DataTableCache.Columns.Add(col);
        }
    }

    /// <summary>
    /// Не более 55
    /// </summary>
    /// <param name="limit_row"></param>
    /// <returns></returns>
    public async Task<(DataTable TableData, ArrayList Columns)> GetRandomRowsAsDataTable(int limit_row, bool del_row_inc = true)
    {
        if (DbfFile is null)
            throw new Exception("db file not set");

        if (DataTableCache is null)
            throw new Exception("data table not init");

        if (Columns is null)
            throw new Exception("columns table not created");

        DataTableCache.Rows.Clear();

        if (header.numRecords <= 0)
            return (StructureDB, Columns);

        if (limit_row <= 5)
            limit_row = 5;
        if (header.numRecords < limit_row)
            limit_row = header.numRecords;

        limit_row = Math.Min(55, limit_row);

        long old_file_position = DbfFile.Position;
        DbfFile.Seek(header.headerLen, SeekOrigin.Begin);
        Random rnd = new();
        rnd.Next(0, header.numRecords - 1);
        DataRow row;
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
            row = DataTableCache.NewRow();
            foreach (FieldDescriptor field in Columns)
            {
                switch (field.fieldType)
                {
                    case 'N':  // Number
                        string number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
                        if (IsNumber(number))
                        {
                            row[fieldIndex] = number;
                        }
                        else
                        {
                            row[fieldIndex] = "0";
                        }
                        break;
                    case 'C': // String
                        row[fieldIndex] = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));//row[fieldIndex] = CurrEnc.GetString(recReader.ReadBytes(field.fieldLen));
                        break;

                    case 'D': // Date (YYYYMMDD)
                        year = CurrentEncoding.GetString(recReader.ReadBytes(4));
                        month = CurrentEncoding.GetString(recReader.ReadBytes(2));
                        day = CurrentEncoding.GetString(recReader.ReadBytes(2));
                        row[fieldIndex] = DBNull.Value;
                        try
                        {
                            if (IsNumber(year) && IsNumber(month) && IsNumber(day))
                            {
                                if (int.Parse(year) > 1900)
                                {
                                    row[fieldIndex] = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day)).ToString();
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
                        row[fieldIndex] = JulianToDateTime(lDate).AddTicks(lTime).ToString();
                        break;

                    case 'L': // Boolean (Y/N)
                        if ('Y' == recReader.ReadByte())
                        {
                            row[fieldIndex] = "true";
                        }
                        else
                        {
                            row[fieldIndex] = "false";
                        }

                        break;

                    case 'F':
                        number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
                        if (IsNumber(number))
                        {
                            row[fieldIndex] = number;
                        }
                        else
                        {
                            row[fieldIndex] = "0.0";
                        }
                        break;
                }
                fieldIndex++;
            }
            recReader.Close();
            DataTableCache.Rows.Add(row);
        }
        DbfFile.Seek(old_file_position, SeekOrigin.Begin);
        return (DataTableCache, Columns);
    }

    public async Task Transit(bool inc_del)
    {
        if (DbfFile is null)
            throw new Exception("db file not set");

        if (DataTableCache is null)
            throw new Exception("data table not init");

        if (Columns is null)
            throw new Exception("columns table not created");

        DataList ??= [];
        DataTableCache.Rows.Clear();

        //string table_name = "table_" + new System.Text.RegularExpressions.Regex(@"\W").Replace(System.IO.Path.GetFileName(FileOutputName), "_");        
        DbfFile.Seek(header.headerLen, SeekOrigin.Begin);
        string[] s_row;
        int data_list_Count = 0, del_rows_count = 0;

        byte[] readed_data_tmp;
        for (int counter = 0; counter <= header.numRecords - 1; counter++)
        {
            cur_num_row = counter;
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
            s_row = new string[Columns.Count];
            foreach (FieldDescriptor field in Columns)
            {
                readed_data_tmp = recReader.ReadBytes(field.fieldLen);
                switch (field.fieldType)
                {
                    case 'N':
                        string number = CurrentEncoding.GetString(readed_data_tmp);
                        if (IsNumber(number))
                        {
                            s_row[fieldIndex] = number;
                        }
                        else
                        {
                            s_row[fieldIndex] = "0";
                        }
                        break;
                    case 'C':
                        //if (type_file == "my.sql")
                        //    s_row[fieldIndex] = "'" + g.MySQLEscape(CurrentEncoding.GetString(readed_data_tmp)) + "'";
                        //else if (type_file == "csv")
                        //    s_row[fieldIndex] = g.CSVEscape(CurrentEncoding.GetString(readed_data_tmp));
                        //else if (type_file == "xml")
                        //    s_row[fieldIndex] = CurrentEncoding.GetString(readed_data_tmp).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
                        //else
                        //    throw new NotImplementedException();
                        break;
                    case 'D': // Date (YYYYMMDD)
                        year = CurrentEncoding.GetString(readed_data_tmp.SubArray(0, 4));
                        month = CurrentEncoding.GetString(readed_data_tmp.SubArray(4, 2));
                        day = CurrentEncoding.GetString(readed_data_tmp.SubArray(6, 2));
                        s_row[fieldIndex] = "''";
                        try
                        {
                            if (IsNumber(year) && IsNumber(month) && IsNumber(day))
                            {
                                if ((int.Parse(year) > 1900))
                                {
                                    s_row[fieldIndex] = "'" + new DateTime(int.Parse(year), int.Parse(month), int.Parse(day)).ToString() + "'";
                                }
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
                        s_row[fieldIndex] = JulianToDateTime(lDate).AddTicks(lTime).ToString();
                        break;
                    case 'L': // Boolean (Y/N)
                        if (CurrentEncoding.GetString(readed_data_tmp).ToUpper() == "T")
                        {
                            s_row[fieldIndex] = "1";
                        }
                        else
                        {
                            s_row[fieldIndex] = "0";
                        }

                        break;
                    case 'F':
                        number = CurrentEncoding.GetString(readed_data_tmp);
                        if (IsNumber(number))
                        {
                            s_row[fieldIndex] = number;
                        }
                        else
                        {
                            s_row[fieldIndex] = "0.0";
                        }
                        break;
                }
                fieldIndex++;
            }
            recReader.Close();
            data_list_Count++;
            DataList.Add(s_row);
            if (data_list_Count > 5000)
            {
                data_list_Count = 0;
                int curr_count_data_lines = DataList.Count;

            }
        }

        //if (fsWrite != null)
        //    fsWrite.Close();
        //else
        //    my_stream.Close();
    }

    /*private void WriteXML(string[] s_arr)
    {
        string result_string = "<row ";
        int field_index = -1;
        int s_arr_Length = s_arr.Length - 1;
        foreach (FieldDescriptor field in fields)
        {
            field_index++;
            result_string += field.fieldName + "=\"" + s_arr[field_index] + "\"\t";
            if (field_index == s_arr_Length)
                result_string = result_string.TrimEnd() + "/>";
        }
        //bw.Write(g.StringToByte("\t\t" + result_string + "\n"));
    }
    private string NamesFieldsSQL(bool ForCreateTable, string separator = ", ", string quote = "`")
    {
        string returned_data = "";
        foreach (FieldDescriptor field in fields)
        {
            string type_data = quote + field.fieldName + quote;
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
        returned_data = returned_data.Trim();
        returned_data = returned_data.Substring(0, returned_data.Length - 1);
        return returned_data;
    }

    void Write(string[] strings, string separator = ", ", string left_blok = "(", string right_blok = "),")
    {
        //string result_string = left_blok;
        //foreach (string s in strings)
        //    result_string += s + separator;
        //result_string = result_string.Trim();
        //result_string = result_string.Substring(0, result_string.Length - 1);
        //result_string += right_blok;
        //result_string = result_string.Trim();
        //if (fsWrite != null)
        //    fsWrite.WriteLine(result_string);
        //else
        //    bw.Write(g.StringToByte(result_string + "\n"));
    }*/

    public DataTable StructureDB
    {
        get
        {
            DataTable empty_dt = new();
            if (DataTableCache is null)
                return empty_dt;

            foreach (DataColumn col in DataTableCache.Columns)
                empty_dt.Columns.Add(new DataColumn(col.ColumnName, col.DataType));

            return empty_dt;
        }
    }

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