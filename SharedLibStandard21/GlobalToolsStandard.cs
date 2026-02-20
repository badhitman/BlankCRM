////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Net;
using System;

namespace SharedLib;

/// <summary>
/// Глобальные утилиты
/// </summary>
public static partial class GlobalToolsStandard
{
    /// <inheritdoc/>
    public static readonly int WebChatTicketSessionDeadlineSeconds = 60 * 60 * 24 * 60;

    /// <inheritdoc/>
    public static string ToReadableString(this TimeSpan span)
    {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

        if (formatted.EndsWith(", ")) formatted = formatted[..^2];

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

        return formatted;
    }

    /// <summary>
    /// Создает новый объект System.DateTime, который имеет то же количество тактов,
    /// что и указанный System.DateTime, но обозначается как местное время, всеобщее
    /// координированное время (UTC) или ни то, ни другое, как указано значением System.DateTimeKind.
    /// </summary>
    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.SetKindUtc();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Создает новый объект System.DateTime, который имеет то же количество тактов,
    /// что и указанный System.DateTime, но обозначается как местное время, всеобщее
    /// координированное время (UTC) или ни то, ни другое, как указано указанным значением System.DateTimeKind.
    /// </summary>
    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    /// <summary>
    /// Является ли строка адресом IP
    /// </summary>
    public static bool IsIpAddress(string endPoint) => IPAddress.TryParse(endPoint.Split(':')[0], out _);

    /// <summary>
    /// CreateIPEndPoint
    /// </summary>
    public static IPEndPoint CreateIPEndPoint(string endPoint)
    {
        string[] ep = endPoint.Split(':');
        if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
        if (ep[0].Equals("localhost"))
            ep[0] = "127.0.0.1";
        if (!IPAddress.TryParse(ep[0], out IPAddress ip))
            throw new FormatException("Invalid ip-adress");

        if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out int port))
            throw new FormatException("Invalid port");

        return new IPEndPoint(ip, port);
    }

    /// <summary>
    /// Получить значение атрибута Description
    /// </summary>
    public static string DescriptionInfo(this Enum enumValue)
    {
        foreach (FieldInfo field in enumValue.GetType().GetFields())
        {
            DescriptionAttribute descriptionAttribute = field.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (descriptionAttribute != null && field.Name.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return descriptionAttribute.Description;
            }
        }

        return enumValue.ToString();
    }


    /// <inheritdoc/>
    public static bool IsEmpty(this string sender)
        => string.IsNullOrWhiteSpace(sender);


    /// <summary>
    /// Добавить информация об исключении
    /// </summary>
    public static void InjectException(this List<ResultMessage> sender, List<ValidationResult> validationResults)
        => sender.AddRange(validationResults.Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage)).Select(x => new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = x.ErrorMessage! }));

    /// <summary>
    /// Добавить информация об исключении
    /// </summary>
    public static void InjectException(this List<ResultMessage> sender, Exception ex)
    {
        sender.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = ex.Message });
        if (ex.StackTrace != null)
            sender.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = ex.StackTrace });
        int i = 0;
        while (ex.InnerException != null)
        {
            i++;
            sender.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.Message}" });
            if (ex.InnerException.StackTrace != null)
                sender.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.StackTrace}" });

            ex = ex.InnerException;
        }
    }

    /// <summary>
    /// Преобразовать размер файла в читаемый вид
    /// </summary>
    public static string SizeDataAsString(long SizeFile)
    {
        if (SizeFile < 1024)
            return SizeFile.ToString() + " bytes";
        else if (SizeFile < 1024 * 1024)
            return Math.Round((double)SizeFile / 1024, 2).ToString() + " KB";
        else if (SizeFile < 1024 * 1024 * 1024)
            return Math.Round((double)SizeFile / 1024 / 1024, 2).ToString() + " MB";
        else
            return Math.Round((double)SizeFile / 1024 / 1024 / 1024, 3).ToString() + " GB";
    }

    /// <summary>
    /// файл является изображением
    /// </summary>
    public static bool IsImageFile(string file_tag)
    {
        return file_tag
            .EndsWith("GIF", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("JPG", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("JPEG", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("PNG", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("BMP", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("BMP2", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("BMP3", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("ICO", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("HEIC", StringComparison.OrdinalIgnoreCase) ||
            file_tag.EndsWith("JBIG", StringComparison.OrdinalIgnoreCase);
    }
}