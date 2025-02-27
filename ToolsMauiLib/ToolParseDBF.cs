////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
using System.Data;
using SharedLib;

namespace ToolsMauiLib;

public partial class ToolParseDBF
{
    public Encoding CurrentEncoding { get; set; } = Encoding.Default;

    BinaryReader recReader, br;
    byte[] buffer;

    string? number, year, month, day;
    long lDate, lTime;
    int fieldIndex, cur_num_row;

    ArrayList fields;
    List<object[]>? data_list;

    GCHandle handle;
    DBFHeader header;

    DataTable dt;
    DataRow? row;


    private readonly bool _dataBaseReady = false;
    public bool DataBaseReady => _dataBaseReady && br?.BaseStream.CanRead == true;

    public long Length_File
    {
        get
        {
            if (!DataBaseReady || br is null)
                return -1;

            return ((FileStream)br.BaseStream).Length;
        }
    }
    public int CountRows
    {
        get
        {
            if (!DataBaseReady)
                return -1;

            return header.numRecords;
        }
    }
    public bool CanReadNextRow
    {
        get
        {
            if (!DataBaseReady)
                return false;

            return cur_num_row < CountRows;
        }
    }

    public ToolParseDBF(string dbfFile)
    {
        if (!File.Exists(dbfFile))
            throw new FileNotFoundException($"Файл dbf не найден: {dbfFile}", dbfFile);

        dt = new DataTable();
        br = new BinaryReader(File.OpenRead(dbfFile));
        buffer = br.ReadBytes(Marshal.SizeOf<DBFHeader>());

        handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        header = Marshal.PtrToStructure<DBFHeader>(handle.AddrOfPinnedObject());
        handle.Free();

        fields = [];
        while (13 != br.PeekChar())
        {
            buffer = br.ReadBytes(Marshal.SizeOf<FieldDescriptor>());
            handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            fields.Add(Marshal.PtrToStructure<FieldDescriptor>(handle.AddrOfPinnedObject()));
            handle.Free();
        }

        ((FileStream)br.BaseStream).Seek(header.headerLen + 1, SeekOrigin.Begin);
        buffer = br.ReadBytes(header.recordLen);
        recReader = new BinaryReader(new MemoryStream(buffer));
        foreach (FieldDescriptor field in fields)
        {
            number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
            DataColumn col = new(field.fieldName, typeof(string));
            dt.Columns.Add(col);
        }
        data_list = new List<object[]>(CountRows);
        _dataBaseReady = true;
    }

    public void SaveAs(string FileOutputName, string type_file, bool inc_del, Action<int, string> UpdateStatus)
    {
        string table_name = "table_" + new System.Text.RegularExpressions.Regex(@"\W").Replace(System.IO.Path.GetFileName(FileOutputName), "_");
        type_file = type_file.ToLower();
        if (type_file != "my.sql" && type_file != "csv" && type_file != "xml")
        {
            //System.Windows.MessageBox.Show(OwnerWin, "Не известный тип сохраняемого файла", "Ошибка");
            return;
        }
        //my_stream = new FileStream(FileOutputName, FileMode.Create, FileAccess.Write);
        //if (type_file == "csv")
        //    fsWrite = new StreamWriter(my_stream, Encoding.UTF8, 1024 * 64);
        //bw = new BinaryWriter(my_stream);
        //switch (type_file)
        //{
        //    case "my.sql":
        //        bw.Write(g.StringToByte(
        //        "-- DBF - conversion  into XML, SQL, CSV, ..." + "\n" +
        //        "-- " + g.preficsBildProgramm + "\n" +
        //        "-- https://sourceforge.net/projects/dbf-to-mysql-csv-xml/" + "\n" +
        //        "-- Datetime create dump: " + DateTime.Now.ToString("dd MM yyyy | HH:mm:ss") + "\n" +
        //        "/*!50503 SET NAMES utf8mb4 */;" + "\n" +
        //        "-- --------------------------------------------------------" + "\n" +
        //        "-- " + "\n" +
        //        "-- `" + table_name + "`\n" +
        //        "-- " + "\n" +
        //        "" + "\n" +
        //        "CREATE TABLE IF NOT EXISTS `" + table_name + "` (" + "\n" +
        //        fields_as_string_for_create + "\n" +
        //        ") ENGINE=MyISAM DEFAULT CHARSET=utf8;" + "\n" + "\n" +
        //        "-- " + "\n" +
        //        "-- Dump data table `" + table_name + "` (" + CountRows + " Records)" + "\n" +
        //        "-- " + "\n" + "\n"));
        //        break;
        //    case "csv":
        //        fsWrite.WriteLine(NamesFieldsSQL(false, ";", ""));
        //        break;
        //    case "xml":
        //        bw.Write(g.StringToByte("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\t<" + new System.Text.RegularExpressions.Regex(@"\W").Replace(System.IO.Path.GetFileNameWithoutExtension(FileOutputName), "_") + ">\n"));
        //        break;
        //}
        
        //my_stream.Close();
        ((FileStream)br.BaseStream).Seek(header.headerLen, SeekOrigin.Begin);
        string[] s_row;
        int data_list_Count = 0, del_rows_count = 0;

        byte[] readed_data_tmp;
        for (int counter = 0; counter <= CountRows - 1; counter++)
        {
            cur_num_row = counter ;
            buffer = br.ReadBytes(header.recordLen);
            if (buffer.Length == 0)
            {
                FlushData(type_file, table_name);
                //fsWrite.Close();
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
            s_row = new string[fields.Count];
            foreach (FieldDescriptor field in fields)
            {
                readed_data_tmp = recReader.ReadBytes(field.fieldLen);
                switch (field.fieldType)
                {
                    case 'N':
                        number = CurrentEncoding.GetString(readed_data_tmp);
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
            data_list.Add(s_row);
            if (data_list_Count > 5000)
            {
                data_list_Count = 0;
                FlushData(type_file, table_name);
                int curr_count_data_lines = data_list.Count;
                //OwnerWin.Dispatcher.Invoke(UpdateStatus, new object[] { counter, string.Format(g.dict["MessParseCountRows"].ToString() + (del_rows_count > 0 ? g.dict["MessParseCountDeletedRows"].ToString() : "") + " " + g.dict["MessParseCounSizeOtputFile"].ToString(), counter, g.SizeFileAsString(bw.BaseStream.Length), del_rows_count) });
            }
        }
        FlushData(type_file, table_name);

        //if (fsWrite != null)
        //    fsWrite.Close();
        //else
        //    my_stream.Close();
    }

    private void FlushData(string type_file, string table_name)
    {
        if (data_list.Count == 0)
            return;

        //if (type_file == "my.sql")
        //    bw.Write(g.StringToByte("INSERT INTO `" + table_name + "` (" + fields_as_string_for_insert + ") VALUES\n"));
        //int data_list_count = data_list.Count;
        //foreach (string[] s in data_list)
        //{
        //    data_list_count--;
        //    if (type_file == "my.sql")
        //        Write(s, ", ", "(", ")" + (data_list_count == 0 ? ";" : ","));
        //    else if (type_file == "csv")
        //        Write(s, ";", "", "");
        //    else
        //        WriteXML(s);
        //}
        data_list.Clear();
    }

    /// <summary>
    /// Не более 55
    /// </summary>
    /// <param name="limit_row"></param>
    /// <returns></returns>
    public DataTable GetRandomRowsAsDataTable(int limit_row, bool del_row_inc = true)
    {
        if (!DataBaseReady)
            return new DataTable();

        if (CountRows <= 0)
            return StructureDB;

        if (limit_row <= 5)
            limit_row = 5;
        if (CountRows < limit_row)
            limit_row = CountRows;

        limit_row = Math.Min(55, limit_row);

        dt.Rows.Clear();
        long old_file_position = ((FileStream)br.BaseStream).Position;
        ((FileStream)br.BaseStream).Seek(header.headerLen, SeekOrigin.Begin);
        Random rnd = new();
        rnd.Next(0, CountRows - 1);
        for (int counter = 0; counter <= limit_row; counter++)
        {
            long random_position_row = (rnd.Next(0, CountRows - 1) * header.recordLen);
            br.BaseStream.Position = (header.headerLen + random_position_row);
            buffer = br.ReadBytes(header.recordLen);
            recReader = new BinaryReader(new MemoryStream(buffer));
            if (recReader.ReadChar() == '*')
            {
                if (!del_row_inc)
                    continue;
            }
            fieldIndex = 0;
            row = dt.NewRow();
            foreach (FieldDescriptor field in fields)
            {
                switch (field.fieldType)
                {
                    case 'N':  // Number
                        number = CurrentEncoding.GetString(recReader.ReadBytes(field.fieldLen));
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
            dt.Rows.Add(row);
        }
        ((FileStream)br.BaseStream).Seek(old_file_position, SeekOrigin.Begin);
        return dt;
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
            if (!DataBaseReady || dt is null)
                return empty_dt;

            foreach (DataColumn col in dt.Columns)
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