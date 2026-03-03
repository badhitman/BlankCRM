////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SharedLib;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BlazorLib;

/// <summary>
/// Extensions
/// </summary>
public static class ExtensionsBlazorLib
{
    /// <summary>
    /// Convert
    /// </summary>
    public static DirectionsEnum Convert(this SortDirection mudSort)
    {
        return mudSort switch
        {
            SortDirection.Ascending => DirectionsEnum.Up,
            SortDirection.Descending => DirectionsEnum.Down,
            SortDirection.None => DirectionsEnum.None,
            _ => DirectionsEnum.None,
        };
    }

    static List<MessageViewModel> MessagesHistory { get; set; } = [];
    static List<ToastViewClientModel> ToastsHistory { get; set; } = [];

    /// <summary>
    /// Получить данные по текущему пользователю
    /// </summary>
    public static UserInfoMainModel? ReadCurrentUserInfo(this ClaimsPrincipal principal)
    {
        string? userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return null;

        string?
            phoneNum = principal.FindFirst(ClaimTypes.MobilePhone)?.Value,
            givenName = principal.FindFirst(ClaimTypes.GivenName)?.Value,
            surName = principal.FindFirst(ClaimTypes.Surname)?.Value,
            patronymic = principal.FindFirst(nameof(IdentityDetailsModel.Patronymic))?.Value,
            userName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            email = principal.FindFirst(ClaimTypes.Email)?.Value;

        string[] roles = principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

        long? telegram_id = null;
        string? telegramIdAsString = principal.FindFirst(GlobalStaticConstants.TelegramIdClaimName)?.Value;
        if (!string.IsNullOrWhiteSpace(telegramIdAsString) && long.TryParse(telegramIdAsString, out long tgId))
            telegram_id = tgId;

        return new()
        {
            PhoneNumber = phoneNum,
            UserId = userId,
            Email = email,
            TelegramId = telegram_id,
            Surname = surName,
            Patronymic = patronymic,
            UserName = userName,
            Roles = [.. roles],
            GivenName = givenName,
            Claims = [.. principal.Claims.Where(x => x.Type != ClaimTypes.Role).Select(x => new EntryAltStandardModel() { Id = x.Type, Name = x.Value })],
        };
    }

    /// <inheritdoc/>
    public static List<ToastViewClientModel> GetHistoryToasts(this ISnackbar SnackBarRepo)
    {
        lock (MessagesHistory)
        {
            return [.. ToastsHistory.Select(x => x)];
        }
    }

    /// <inheritdoc/>
    public static void SaveToast(this ISnackbar sB, ToastShowClientModel tst)
    {
        lock (ToastsHistory)
        {
            ToastsHistory.Add(new() { DateTimeRecord = DateTime.UtcNow, HeadTitle = tst.HeadTitle, MessageText = tst.MessageText, TypeMessage = tst.TypeMessage });
            while (ToastsHistory.Count > 1000)
                ToastsHistory.RemoveAt(0);
        }
    }

    /// <summary>
    /// GetHistoryMessages
    /// </summary>
    public static List<MessageViewModel> GetHistoryMessages(this ISnackbar sB)
    {
        lock (MessagesHistory)
        {
            return [.. MessagesHistory.Select(x => x)];
        }
    }

    /// <inheritdoc/>
    public static void ShowMessagesResponse(this ISnackbar SnackBarRepo, IEnumerable<ResultMessage> messages)
    {
        if (!messages.Any())
            return;

        Severity _style;
        lock (MessagesHistory)
        {
            foreach (ResultMessage m in messages)
            {
                _style = m.TypeMessage switch
                {
                    MessagesTypesEnum.Success => Severity.Success,
                    MessagesTypesEnum.Info => Severity.Info,
                    MessagesTypesEnum.Warning => Severity.Warning,
                    MessagesTypesEnum.Error => Severity.Error,
                    _ => Severity.Normal
                };
                SnackBarRepo.Add(m.Text, _style, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

                int _ix = MessagesHistory.Count - 1;
                if (_ix == -1 || !MessagesHistory[_ix].Message.Equals(m))
                    MessagesHistory.Add(new(m, [DateTime.UtcNow]));
                else
                    MessagesHistory[_ix].Points.Add(DateTime.UtcNow);

                while (MessagesHistory.Count > 110)
                    MessagesHistory.RemoveAt(0);
            }
        }
    }

    /// <inheritdoc/>
    public static void Error(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            SnackbarRepo.Add(message, Severity.Error, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            int _ix = MessagesHistory.Count - 1;
            if (_ix == -1 || MessagesHistory[_ix].Message.TypeMessage != MessagesTypesEnum.Error && MessagesHistory[_ix].Message.Text != message)
                MessagesHistory.Add(new(new() { TypeMessage = MessagesTypesEnum.Error, Text = message }, [DateTime.UtcNow]));
            else
                MessagesHistory[_ix].Points.Add(DateTime.UtcNow);

            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static void Error(this ISnackbar SnackbarRepo, List<ValidationResult> ValidationResults)
    {
        lock (MessagesHistory)
        {
            ValidationResults.ForEach(x =>
            {
                SnackbarRepo.Error(x.ErrorMessage ?? "-error-");

                int _ix = MessagesHistory.Count - 1;
                if (_ix == -1 || MessagesHistory[_ix].Message.TypeMessage != MessagesTypesEnum.Error || MessagesHistory[_ix].Message.Text != x.ErrorMessage)
                    MessagesHistory.Add(new(new() { TypeMessage = MessagesTypesEnum.Error, Text = x.ErrorMessage ?? x.ToString() }, [DateTime.UtcNow]));
                else
                    MessagesHistory[_ix].Points.Add(DateTime.UtcNow);
            });
        }
    }

    /// <inheritdoc/>
    public static void Info(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            SnackbarRepo.Add(message, Severity.Info, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            int _ix = MessagesHistory.Count - 1;
            if (_ix == -1 || MessagesHistory[_ix].Message.TypeMessage != MessagesTypesEnum.Info || MessagesHistory[_ix].Message.Text != message)
                MessagesHistory.Add(new(new() { Text = message, TypeMessage = MessagesTypesEnum.Info }, [DateTime.UtcNow]));
            else
                MessagesHistory[_ix].Points.Add(DateTime.UtcNow);

            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static void Warn(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            SnackbarRepo.Add(message, Severity.Warning, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            int _ix = MessagesHistory.Count - 1;
            if (_ix == -1 || MessagesHistory[_ix].Message.TypeMessage != MessagesTypesEnum.Warning || MessagesHistory[_ix].Message.Text != message)
                MessagesHistory.Add(new(new() { TypeMessage = MessagesTypesEnum.Warning, Text = message }, [DateTime.UtcNow]));
            else
                MessagesHistory[_ix].Points.Add(DateTime.UtcNow);

            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static void Success(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            SnackbarRepo.Add(message, Severity.Success, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

            int _ix = MessagesHistory.Count - 1;
            if (_ix == -1 || MessagesHistory[_ix].Message.TypeMessage != MessagesTypesEnum.Success || MessagesHistory[_ix].Message.Text != message)
                MessagesHistory.Add(new(new() { TypeMessage = MessagesTypesEnum.Success, Text = message }, [DateTime.UtcNow]));
            else
                MessagesHistory[_ix].Points.Add(DateTime.UtcNow);

            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static DirectionsEnum GetVerticalDirection(this SortDirection sort_direction)
    {
        return sort_direction switch
        {
            SortDirection.Descending => DirectionsEnum.Down,
            SortDirection.Ascending => DirectionsEnum.Up,
            _ => DirectionsEnum.Up
        };
    }

    /// <inheritdoc/>
    public static string GetTranslate(this IStringLocalizer<Resources> loc, string src, StringLocalizeAreaEnum? area = null)
    {
        src = loc[src];

        if (area is not null && src.EndsWith($".{area}"))
            src = src[..src.IndexOf($".{area}")];

        return src.Trim();
    }

    /// <summary>
    /// ReloadPage
    /// </summary>
    public static void ReloadPage(this NavigationManager manager)
    {
        manager.NavigateTo(manager.Uri, true);
    }

    /// <summary>
    /// TryGetQueryString
    /// </summary>
    public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T? value)
    {
        Uri uri = navManager.ToAbsoluteUri(navManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues valueFromQueryString))
        {
            if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = (T)(object)valueAsInt;
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)valueFromQueryString.ToString();
                return true;
            }

            if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = (T)(object)valueAsDecimal;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// MessageViewModel
    /// </summary>
    public class MessageViewModel(ResultMessage msg, List<DateTime> points)
    {
        /// <summary>
        /// Message
        /// </summary>
        public ResultMessage Message { get; set; } = msg;

        /// <summary>
        /// Points
        /// </summary>
        public List<DateTime> Points { get; set; } = points;
    }

    /// <inheritdoc/>
    public class ToastViewClientModel : ToastShowClientModel
    {
        /// <inheritdoc/>
        public DateTime DateTimeRecord { get; set; }
    }

    /// <summary>
    /// Кэш команд
    /// </summary>
    static readonly Dictionary<string, CommandEntryModel[]> _commands_cache = [];

    /// <summary>
    /// Все программные калькуляции
    /// </summary>
    public static CommandEntryModel[] CommandsAsEntries<T>()
    {
        Type _current_type = typeof(T);
        string? type_name = _current_type.FullName;
        if (string.IsNullOrWhiteSpace(type_name))
            throw new ArgumentException($"Тип данных [{_current_type}] без имени?", nameof(_current_type.FullName));

        lock (_commands_cache)
        {
            if (_commands_cache.TryGetValue(type_name, out CommandEntryModel[]? _vcc))
                return _vcc;

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetExportedTypes())
                .Where(p => _current_type.IsAssignableFrom(p) && _current_type != p && !p.IsAbstract && !p.IsInterface);

            CommandEntryModel[] res = [.. types.Select(x =>
            {
                if (Activator.CreateInstance(x) is not T obj)
                    throw new Exception("error 919F8FF2-B902-4112-8680-67352F369F0C");

                if (obj is not DeclarationAbstraction _set)
                    throw new Exception("error EF8D4F4A-F578-44C6-B78C-BA7685662938");

                return new CommandEntryModel() { Id = x.Name, Name = _set.Name, Description = _set.About, AllowCallWithoutParameters = _set.AllowCallWithoutParameters };
            })];

            _commands_cache.Add(type_name, res);
            return res;
        }
    }

}