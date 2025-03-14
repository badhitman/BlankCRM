////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SharedLib;

/// <inheritdoc/>
public static class Extensions
{
    /// <inheritdoc/>
    public static List<RootKLADRModelDB> KladrBuild(this Dictionary<KladrChainTypesEnum, Newtonsoft.Json.Linq.JObject[]> src)
    {
        List<RootKLADRModelDB> res = [];
        foreach (KeyValuePair<KladrChainTypesEnum, Newtonsoft.Json.Linq.JObject[]> node in src)
        {
            foreach (Newtonsoft.Json.Linq.JObject subNode in node.Value)
            {
                switch (node.Key)
                {
                    case KladrChainTypesEnum.StreetsInPopPoint or KladrChainTypesEnum.StreetsInCity or KladrChainTypesEnum.StreetsInRegion:
                        res.Add(subNode.ToObject<StreetMetaKLADRModel>()!);
                        break;
                    case KladrChainTypesEnum.HousesInStreet:
                        res.Add(subNode.ToObject<HouseKLADRModelDTO>()!);
                        break;
                    default:
                        res.Add(subNode.ToObject<ObjectMetaKLADRModel>()!);
                        break;
                }
            }
        }

        return res;
    }

    /// <summary>
    /// GetCustomTime
    /// </summary>
    public static DateTime GetCustomTime(this DateTime dateTime, string timeZone = "Europe/Moscow")
        => TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

    /// <summary>
    /// Дата + время
    /// </summary>
    public static string GetHumanDateTime(this DateTime dateTime, string timeZone = "Europe/Moscow")
    {
        DateTime _cdt = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
        return $"{_cdt.ToString("d", GlobalStaticConstants.RU)} {_cdt.ToString("t", GlobalStaticConstants.RU)}";
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
    /// Отправка запроса GET согласно указанному универсальному коду ресурса (URI) и возврат текста ответа в виде строки в асинхронной операции.
    /// </summary>
    public static async Task<TResponseModel<T>> GetStringAsync<T>(this HttpClient httpCli, [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken = default) where T : class
    {
        TResponseModel<T> res = new();
        try
        {
            string raw = await httpCli.GetStringAsync(requestUri, cancellationToken);
            res.Response = JsonConvert.DeserializeObject<T>(raw) ?? throw new Exception(raw);
        }
        catch (Exception ex)
        {
            res.Messages.InjectException(ex);
        }

        return res;
    }

    /// <summary>
    /// SubArray
    /// </summary>
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
}