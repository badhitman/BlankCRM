////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SharedLib;

/// <inheritdoc/>
public static class Extensions
{

    /// <summary>
    /// Получить значение атрибута ConnectionMetadata
    /// </summary>
    public static (string BaseUrl, string GetStatementRequest, string? AccListRequest)? ConnectionMetadata(this Enum enumValue)
    {
        foreach (FieldInfo field in enumValue.GetType().GetFields())
        {
            ConnectionMetadataAttribute? descriptionAttribute = field.GetCustomAttributes<ConnectionMetadataAttribute>().FirstOrDefault();
            if (descriptionAttribute != null && field.Name.Equals(enumValue.ToString()))
                return (descriptionAttribute.BaseUrl, descriptionAttribute.GetStatementRequest, descriptionAttribute.AccListRequest);
        }

        return null;
    }

    /// <summary>
    /// Получить маршрут OU
    /// </summary>
    /// <param name="inc">Носитель сборки маршрута</param>
    /// <param name="dn_parsed_raw">Строка пути для парсинга</param>
    /// <param name="ldap_route_segment_type">Тип сегмента маршрута/пути DN</param>
    public static void OrganizationalUnitsSections(ref List<OuRouteSegmentModel> inc, string dn_parsed_raw, LdapRouteSegmentsTypesEnum ldap_route_segment_type = LdapRouteSegmentsTypesEnum.Ou)
    {
        string ldap_route_segment_type_str = ldap_route_segment_type.ToString().ToLower();
        dn_parsed_raw = dn_parsed_raw.Trim();
        int i = dn_parsed_raw.ToLower().IndexOf($"{ldap_route_segment_type_str}=");
        if (i < 0)
            return;
        //
        int io = dn_parsed_raw.IndexOf(",");
        string curr_ou = io < 0
        ? dn_parsed_raw.Trim()[3..]
        : dn_parsed_raw.Substring(i + 3, io - 3).Trim();

        inc.Add(new OuRouteSegmentModel()
        {
            Name = curr_ou,
            LdapRouteSegmentType = ldap_route_segment_type
        });

        if (dn_parsed_raw.Length <= curr_ou.Length + 4)
            return;

        dn_parsed_raw = dn_parsed_raw[(curr_ou.Length + 4)..];
        if (!dn_parsed_raw.StartsWith($"{ldap_route_segment_type_str}=", StringComparison.OrdinalIgnoreCase))
        {
            switch (ldap_route_segment_type)
            {
                case LdapRouteSegmentsTypesEnum.Ou:
                    OrganizationalUnitsSections(ref inc, dn_parsed_raw, LdapRouteSegmentsTypesEnum.Dc);
                    break;
                case LdapRouteSegmentsTypesEnum.Dc:
                    return;
            }
        }
        else
        {
            OrganizationalUnitsSections(ref inc, dn_parsed_raw, ldap_route_segment_type);
        }
    }

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
        return $"{_cdt.ToString("d", GlobalStaticConstants.RU)} {_cdt.ToString("T", GlobalStaticConstants.RU)}";
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