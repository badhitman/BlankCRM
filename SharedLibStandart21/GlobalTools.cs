////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using System.Net;
using System.ComponentModel;
using System.Reflection;

namespace SharedLib;

/// <summary>
/// Глобальные утилиты
/// </summary>
public static partial class GlobalToolsStandard
{
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
    /// CreateIPEndPoint
    /// </summary>
    public static IPEndPoint CreateIPEndPoint(string endPoint)
    {
        string[] ep = endPoint.Split(':');
        if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
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
            DescriptionAttribute? descriptionAttribute = field.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (descriptionAttribute != null && field.Name.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return descriptionAttribute.Description;
            }
        }

        return enumValue.ToString();
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


    /// <inheritdoc/>
    public static bool IsEmpty(this string sender)
        => string.IsNullOrWhiteSpace(sender);


    /// <summary>
    /// Добавить информация об исключении
    /// </summary>
    public static void InjectException(this List<ResultMessage> sender, List<ValidationResult> validationResults)
        => sender.AddRange(validationResults.Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage)).Select(x => new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = x.ErrorMessage! }));

    /// <summary>
    /// Добавить информация об исключении
    /// </summary>
    public static void InjectException(this List<ResultMessage> sender, Exception ex)
    {
        sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = ex.Message });
        if (ex.StackTrace != null)
            sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = ex.StackTrace });
        int i = 0;
        while (ex.InnerException != null)
        {
            i++;
            sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.Message}" });
            if (ex.InnerException.StackTrace != null)
                sender.Add(new ResultMessage() { TypeMessage = ResultTypesEnum.Error, Text = $"InnerException -> {i}/ {ex.InnerException.StackTrace}" });

            ex = ex.InnerException;
        }
    }
}
