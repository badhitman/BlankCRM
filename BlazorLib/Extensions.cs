////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using SharedLib;
using MudBlazor;

namespace BlazorLib;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
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

        string? phoneNum = principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
        string? givenName = principal.FindFirst(ClaimTypes.GivenName)?.Value;
        string? surName = principal.FindFirst(ClaimTypes.Surname)?.Value;
        string? userName = principal.FindFirst(ClaimTypes.Name)?.Value;
        string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
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
            UserName = userName,
            Roles = [.. roles],
            GivenName = givenName,
            Claims = [.. principal.Claims.Where(x => x.Type != ClaimTypes.Role).Select(x => new EntryAltModel() { Id = x.Type, Name = x.Value })],
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
}