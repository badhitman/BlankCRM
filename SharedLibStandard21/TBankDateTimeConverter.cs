////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Converters;
using System;

namespace SharedLib;

/// <summary>
/// Преобразователь <see cref="DateTime"/> в/из строку в формате YYYY-MM-DDTHH24:MI:SS+GMT
/// </summary>
public class TBankDateTimeConverter : IsoDateTimeConverter
{
    /// <summary>
    /// Преобразователь &lt;see cref="DateTime"/&gt; в/из строку в формате YYYY-MM-DDTHH24:MI:SS+GMT
    /// </summary>
    public TBankDateTimeConverter()
    {
        DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";
    }
}