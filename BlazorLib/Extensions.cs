////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using SharedLib;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace BlazorLib;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    static List<ResultMessage> MessagesHistory { get; set; } = [];

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

    /// <summary>
    /// GetHistoryMessages
    /// </summary>
    public static List<ResultMessage> GetHistoryMessages(this ISnackbar SnackbarRepo)
    {
        lock (MessagesHistory)
        {
            return [.. MessagesHistory.Select(x => x)];
        }
    }

    /// <inheritdoc/>
    public static void ShowMessagesResponse(this ISnackbar SnackbarRepo, IEnumerable<ResultMessage> messages)
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
                SnackbarRepo.Add(m.Text, _style, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                MessagesHistory.Add(m);
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
            MessagesHistory.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = message });
            SnackbarRepo.Add(message, Severity.Error, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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
                MessagesHistory.Add(new() { TypeMessage = MessagesTypesEnum.Error, Text = x.ErrorMessage ?? "-error-" });
                SnackbarRepo.Error(x.ErrorMessage ?? "-error-");
            });
        }
    }

    /// <inheritdoc/>
    public static void Info(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            MessagesHistory.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = message });
            SnackbarRepo.Add(message, Severity.Info, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static void Warn(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            MessagesHistory.Add(new() { TypeMessage = MessagesTypesEnum.Warning, Text = message });
            SnackbarRepo.Add(message, Severity.Warning, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            while (MessagesHistory.Count > 110)
                MessagesHistory.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    public static void Success(this ISnackbar SnackbarRepo, string message)
    {
        lock (MessagesHistory)
        {
            MessagesHistory.Add(new() { TypeMessage = MessagesTypesEnum.Success, Text = message });
            SnackbarRepo.Add(message, Severity.Success, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
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
}