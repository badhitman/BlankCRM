////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SharedLib;

/// <summary>
/// Валидация корректности имени
/// </summary>
public partial class NameValidAttribute : ValidationAttribute
{
    /// <summary>
    /// Разрешить пустую строку
    /// </summary>
    public bool AllowEmptyStrings { get; set; }

    static readonly Regex MyRegexName = new(@"^\w+.*\w$", RegexOptions.Compiled),
        MyRegexPrefixCheck = new(@"^[^\d_]", RegexOptions.Compiled),
        MyRegexPostfixCheck = new(@"[^_]$");

    /// <inheritdoc/>
    public override bool IsValid(object? value)
    {
        ErrorMessage = "Некорректное имя: первым и последним символом должна идти буква";

        if (value is null)
            return AllowEmptyStrings;

        if (value is string n)
        {
            if (AllowEmptyStrings && n == "")
                return true;

            return MyRegexName.IsMatch(n) &&
                MyRegexPrefixCheck.IsMatch(n) &&
                MyRegexPostfixCheck.IsMatch(n);
        }

        return false;
    }
}