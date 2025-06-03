﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace SharedLib;

/// <summary>
/// Глобальные утилиты
/// </summary>
public static partial class GlobalTools
{
    /// <summary>
    /// Рассчитать хеш строки
    /// </summary>
    /// <param name="inputString">Строка для расчёта hash</param>
    /// <returns>hash строки</returns>
    public static string GetHashString(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
            return "<is null or white space>";

        StringBuilder sb = new();
        foreach (byte b in SHA256.HashData(Encoding.UTF8.GetBytes(inputString)))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }


    static readonly Regex cn_rx = new(@"^cn=([^,]*.*?)(?=,[^,])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    /// <summary>
    /// Извлечь 'comon name'
    /// </summary>
    /// <param name="dn">distinguishedName</param>
    /// <returns>Значение атрибута CN</returns>
    public static string GetCnAttrFromLdap(string dn)
    {
        if (string.IsNullOrWhiteSpace(dn))
            return string.Empty;

        Match rx = cn_rx.Match(dn);

        if (!rx.Success)
            return string.Empty;

        return rx.Groups[1].Value;
    }

    /// <summary>
    /// Экранировать спецсимволы в значениях, вставляемых в фильтры-запросы
    /// </summary>
    /// <param name="value">Исходная строка значения, которе будет вставлено в правую часть проверки равенства фильтра</param>
    public static string LdapEscapeValue(string value)
    {
        value = value
            .Replace("\\", "\\5c")
            .Replace("(", "\\28")
            .Replace(")", "\\29")
            .Replace("&", "\\26")
            .Replace("|", "\\7c")
            .Replace("=", "\\3d")
            .Replace(">", "\\3e")
            .Replace("<", "\\3c")
            .Replace("~", "\\7e")
            .Replace("*", "\\2a")
            .Replace("/", "\\2f");
        return value;
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
        int i = dn_parsed_raw.IndexOf($"{ldap_route_segment_type_str}=", StringComparison.CurrentCultureIgnoreCase);
        if (i < 0)
            return;
        //
        int io = dn_parsed_raw.IndexOf(',');
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
            OrganizationalUnitsSections(ref inc, dn_parsed_raw, ldap_route_segment_type);
    }

    /// <summary>
    /// IsPhoneNumber
    /// </summary>
    public static bool IsPhoneNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return false;

        return PhoneNumValidateRegex().Match(number).Success;
    }

    /// <summary>
    /// RunCommandWithBash
    /// </summary>
    public static string RunCommandWithBash(string command, string fileName = "/bin/bash")
    {
        ProcessStartInfo psi = new()
        {
            FileName = fileName,
            Arguments = command,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process? process = Process.Start(psi);

        if (process is null)
            return SharedLib.GlobalStaticConstantsRoutes.Routes.NULL_CONTROLLER_NAME;

        process.WaitForExit();
        return process.StandardOutput.ReadToEnd();
    }

    /// <summary>
    /// Перемешать список элементов
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rng = new();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// GetFiles
    /// </summary>
    public static IEnumerable<string> GetFiles(string path)
    {
        Queue<string> queue = new();
        queue.Enqueue(path);
        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            try
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            string[]? files = null;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    yield return files[i];
                }
            }
        }
    }

    /// <summary>
    /// Добавить параметр к запросу. если он существует, то происходит обновление этого параметра
    /// </summary>
    public static string AppendQueryParameter(this Uri uri, string name, string val)
    {
        UriBuilder uriBuilder = new(uri);
        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
        if (query.AllKeys.Contains(name))
            query[name] = val;
        else
            query.Add(name, val);

        uriBuilder.Query = query.ToString();

        return uriBuilder.ToString();
    }

    /// <summary>
    /// HTML строку в обычную/нормальную (без тегов).
    /// например: для добавления в remarks
    /// </summary>
    public static string[] DescriptionHtmlToLinesRemark(string html_description)
    {
        if (string.IsNullOrWhiteSpace(html_description))
            return [];

        HtmlDocument doc = new();
        doc.LoadHtml(html_description
            .Replace("&nbsp;", " ")
            .Replace("  ", " ")
            .Replace("</p><p>", $"</p>{Environment.NewLine}<p>")
            .Replace("</br>", $"</br>{Environment.NewLine}")
            );
        return doc.DocumentNode.InnerText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// PascalCase to kebab-case
    /// </summary>
    public static string PascalToKebabCase(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        value = MyPascalToKebabCaseRegex().Replace(value, "-$1")
            .Trim()
            .ToLower();

        while (value.StartsWith('-'))
            value = value[1..];

        while (value.EndsWith('-'))
            value = value[..^1];

        while (value.Contains("--"))
            value = value.Replace("--", "-");

        return value;
    }

    /// <summary>
    /// Попытка де-сереализовать строку
    /// </summary>
    public static bool TryParseJson<T>(this string data, out T? result)
    {
        bool success = true;
        JsonSerializerSettings settings = new()
        {
            Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
            MissingMemberHandling = MissingMemberHandling.Error
        };
        result = JsonConvert.DeserializeObject<T>(data, settings);
        return success;
    }

    /// <summary>
    /// Валидация модели объекта
    /// </summary>
    public static ValidateReportModel ValidateObject(object object_for_validate)
    {
        List<ValidationResult> validationResults = [];
        return new ValidateReportModel(Validator.TryValidateObject(object_for_validate, new ValidationContext(object_for_validate), validationResults, true), validationResults);
    }

    /// <summary>
    /// Клон объекта (через сереализацию)
    /// </summary>
    public static T? CreateDeepCopy<T>(T? obj)
    {
        if (obj is null)
            return default;

        string json_raw = JsonConvert.SerializeObject(obj, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        return JsonConvert.DeserializeObject<T>(json_raw);

        //using MemoryStream ms = new();
        //XmlSerializer serializer = new(obj!.GetType());
        //serializer.Serialize(ms, obj);
        //ms.Seek(0, SeekOrigin.Begin);
        //return (T)serializer.Deserialize(ms)!;
    }


    #region
    /// <summary>
    /// Вычислить Хеш строки
    /// </summary>
    public static string CalculateHashString(string password)
    {
        byte[] bytes = new UTF8Encoding().GetBytes(password);
        byte[] hashBytes = System.Security.Cryptography.MD5.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Генерация пароля
    /// </summary>
    /// <param name="length">Длинна пароля</param>
    public static string CreatePassword(int length) => new PasswordGenerator(minimumLengthPassword: length, maximumLengthPassword: length).Generate();
    #endregion

    /// <summary>
    /// Транслит строки
    /// </summary>
    public static string GetTranslitString(string str)
    {
        string[] lat_up = ["A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya"];
        string[] lat_low = ["a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya"];
        string[] rus_up = ["А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"];
        string[] rus_low = ["а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"];
        for (int i = 0; i <= 32; i++)
        {
            str = str.Replace(rus_up[i], lat_up[i]);
            str = str.Replace(rus_low[i], lat_low[i]);
        }
        return str;
    }

    /// <summary>
    /// Транслит названия в латиницу CamelCase
    /// </summary>
    public static string TranslitToSystemName(string str)
    {
        str = Regex.Replace(str, @"[^\w\d_]+", " ").Trim();

        if (str.Length == 0)
            return str;

        string[] segments = str.Split(' ');

        if (segments.Length <= 1)
            str = $"{str[0..1].ToUpper()}{str[1..]}";
        else
            str = string.Join("", segments.Select(x => $"{x[0..1].ToUpper()}{x[1..]}"));

        return GetTranslitString(str);
    }

    /// <summary>
    /// Получить значение свойства анонимного типа (приведённого к object)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">объект анонимного типа</param>
    /// <param name="property_name">Имя свойства, которое требуется получить</param>
    /// <returns>Значение свойства (или null)</returns>
    public static T? GetPropertyValue<T>(this object obj, string property_name)
    {
        return (T?)obj.GetType().GetProperty(property_name)?.GetValue(obj, null);
    }

    /// <summary>
    /// Получить тип по его имени
    /// </summary>
    public static Type? GetType(string strFullyQualifiedName)
    {
        Type? type = Type.GetType(strFullyQualifiedName);
        if (type is not null)
            return type;
        foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(strFullyQualifiedName);
            if (type != null)
                return type;
        }
        return Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(t => t.Name == strFullyQualifiedName);
    }

    /// <summary>
    /// Является ли объект коллекцией элементов
    /// </summary>
    public static bool IsEnumerableType(Type type) => type != typeof(string) && type.GetInterfaces().Any(s => s.Name.StartsWith("IEnumerable"));

    /// <summary>
    /// Проверка валидности JSON строки к типу данных
    /// </summary>
    public static bool ValidateJson<T>(string json_src)
    {
        if (string.IsNullOrWhiteSpace(json_src))
            return false;

        try
        {
            _ = JsonConvert.DeserializeObject<T>(json_src);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Соответствия типов данных с расширениями файлов
    /// </summary>
    public static readonly Dictionary<string, string[]> ContentTypes = new()
    {
        {"text/html", new[]{"html", "htm", "shtml"}},
        {"text/css", new[]{"css"}},
        {"text/xml", new[]{"xml"}},
        {"image/gif", new[]{"gif"}},
        {"image/jpeg", new[]{"jpeg", "jpg"}},
        {"application/x-javascript", new[]{"js"}},
        {"application/atom+xml", new[]{"atom"}},
        {"application/rss+xml", new[]{"rss"}},
        {"text/mathml", new[]{"mml"}},
        {"text/plain", new[]{"txt"}},
        {"text/vnd.sun.j2me.app-descriptor", new[]{"jad"}},
        {"text/vnd.wap.wml", new[]{"wml"}},
        {"text/x-component", new[]{"htc"}},
        {"image/png", new[]{"png"}},
        {"image/tiff", new[]{"tif", "tiff"}},
        {"image/vnd.wap.wbmp", new[]{"wbmp"}},
        {"image/x-icon", new[]{"ico"}},
        {"image/x-jng", new[]{"jng"}},
        {"image/x-ms-bmp", new[]{"bmp"}},
        {"image/svg+xml", new[]{"svg"}},
        {"image/webp", new[]{"webp"}},
        {"application/java-archive", new[]{"jar", "war", "ear"}},
        {"application/mac-binhex40", new[]{"hqx"}},
        {"application/msword", new[]{"doc"}},
        {"application/pdf", new[]{"pdf"}},
        {"application/postscript", new[]{"ps", "eps", "ai"}},
        {"application/rtf", new[]{"rtf"}},
        {"application/vnd.ms-excel", new[]{"xls"}},
        {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new[]{"xlsx"}},
        {"application/vnd.ms-powerpoint", new[]{"ppt"}},
        {"application/vnd.wap.wmlc", new[]{"wmlc"}},
        {"application/vnd.google-earth.kml+xml", new[]{"kml"}},
        {"application/vnd.google-earth.kmz", new[]{"kmz"}},
        {"application/x-7z-compressed", new[]{"7z"}},
        {"application/x-cocoa", new[]{"cco"}},
        {"application/x-java-archive-diff", new[]{"jardiff"}},
        {"application/x-java-jnlp-file", new[]{"jnlp"}},
        {"application/x-makeself", new[]{"run"}},
        {"application/x-perl", new[]{"pl", "pm"}},
        {"application/x-pilot", new[]{"prc", "pdb"}},
        {"application/x-rar-compressed", new[]{"rar"}},
        {"application/x-redhat-package-manager", new[]{"rpm"}},
        {"application/x-sea", new[]{"sea"}},
        {"application/x-shockwave-flash", new[]{"swf"}},
        {"application/x-stuffit", new[]{"sit"}},
        {"application/x-tcl", new[]{"tcl", "tk"}},
        {"application/x-x509-ca-cert", new[]{"der", "pem", "crt"}},
        {"application/x-xpinstall", new[]{"xpi"}},
        {"application/xhtml+xml", new[]{"xhtml"}},
        {"application/zip", new[]{"zip"}},
        {"application/octet-stream", new[]{"bin", "exe", "dll","deb","dmg","eot","iso", "img","msi", "msp", "msm"}},
        {"audio/midi", new[]{"mid", "midi", "kar"}},
        {"audio/mpeg", new[]{"mp3"}},
        {"audio/ogg", new[]{"ogg"}},
        {"audio/x-realaudio", new[]{"ra"}},
        {"video/3gpp", new[]{"3gpp", "3gp"}},
        {"video/mpeg", new[]{"mpeg", "mpg"}},
        {"video/quicktime", new[]{"mov"}},
        {"video/x-flv", new[]{"flv"}},
        {"video/x-mng", new[]{"mng"}},
        {"video/x-ms-asf", new[]{"asx", "asf"}},
        {"video/x-ms-wmv", new[]{"wmv"}},
        {"video/x-msvideo", new[]{"avi"}},
        {"video/mp4", new[]{"m4v", "mp4"} }
    };

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex MyPascalToKebabCaseRegex();

    [GeneratedRegex(@"^(\+?[0-9]{11})$")]
    private static partial Regex PhoneNumValidateRegex();
}

/// <summary>
/// ValidateReportModel
/// </summary>
public record struct ValidateReportModel(bool IsValid, List<ValidationResult> ValidationResults)
{
    /// <inheritdoc/>
    public static implicit operator (bool IsValid, List<ValidationResult> ValidationResults)(ValidateReportModel value)
    {
        return (value.IsValid, value.ValidationResults);
    }

    /// <inheritdoc/>
    public static implicit operator ValidateReportModel((bool IsValid, List<ValidationResult> ValidationResults) value)
        => new(value.IsValid, value.ValidationResults);
}