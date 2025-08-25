////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Базовая модель ответа/результата на запрос
/// </summary>
public class TResponseModel<T> : ResponseBaseModel
{
    /// <summary>
    /// Базовая модель ответа/результата на запрос
    /// </summary>
    public TResponseModel() { }

    /// <summary>
    /// Базовая модель ответа/результата на запрос
    /// </summary>
    public TResponseModel(IEnumerable<ResultMessage> messages) { Messages = [.. messages]; }

    /// <summary>
    /// Полезная нагрузка ответа
    /// </summary>
    public T? Response { get; set; }

    /// <inheritdoc/>
    public static TResponseModel<T> Build(ResponseBaseModel sender)
    {
        return new() { Messages = sender.Messages };
    }
}