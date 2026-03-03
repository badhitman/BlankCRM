////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Декларативная модель
/// </summary>
public abstract class DeclarationAbstraction
{
    /// <summary>
    /// Название
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Описание
    /// </summary>
    public abstract string? About { get; }

    /// <summary>
    /// Разрешение запуска/вызова без параметров
    /// </summary>
    public virtual bool AllowCallWithoutParameters { get; } = false;

    static readonly Dictionary<string, CommandAsEntryModel> ParseCalculationsAsEntriesCache = [];

    /// <summary>
    /// Кэш деклараций
    /// </summary>
    static readonly Dictionary<string, DeclarationAbstraction> _calc_cache = [];
    /// <summary>
    /// Получить службу-обработчик
    /// </summary>
    /// <param name="type_name">имя калькулятора</param>
    /// <exception cref="Exception">Ошибка в имени калькулятора или требуемый калькулятор находится в NS отличном от основного</exception>
    public static DeclarationAbstraction? GetHandlerService(string type_name)
    {
        if (string.IsNullOrWhiteSpace(type_name))
            return null;

        DeclarationAbstraction? _c;
        lock (_calc_cache)
        {
            if (_calc_cache.TryGetValue(type_name, out DeclarationAbstraction? _vcc))
                return _vcc;

            Type? t = GlobalTools.GetType($"{typeof(DeclarationAbstraction).Namespace}.{type_name}");
            if (t is null)
                return null;

            _c = Activator.CreateInstance(t) as DeclarationAbstraction;
            if (_c is not null)
                _calc_cache.Add(type_name, _c);
        }

        return _c;
    }

    /// <summary>
    /// Парсить строку JSON в команду
    /// </summary>
    public static CommandAsEntryModel? ParseCommandsAsEntries(string json_data)
    {
        if (string.IsNullOrWhiteSpace(json_data))
            return null;

        lock (ParseCalculationsAsEntriesCache)
        {
            if (ParseCalculationsAsEntriesCache.TryGetValue(json_data, out CommandAsEntryModel? c))
                return c;

            Dictionary<string, string>? kvp = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_data);
            if (kvp is not null)
            {
                string[]? pd = JsonConvert.DeserializeObject<string[]>(kvp[nameof(MetadataExtensionsFormFieldsEnum.Parameter)]);
               
                CommandAsEntryModel _v = new()
                {
                    CommandName = kvp[nameof(MetadataExtensionsFormFieldsEnum.Descriptor)],
                    Options = pd ?? []
                };

                ParseCalculationsAsEntriesCache.Add(json_data, _v);
                return _v;
            }
        }

        return null;
    }
}