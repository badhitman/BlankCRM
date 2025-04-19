////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Базовая модель ответа/результата на запрос
/// </summary>
public class TResponseMQModel<T> : TResponseMQBaseModel<T>
{
    /// <summary>
    /// Базовая модель ответа/результата на запрос
    /// </summary>
    public TResponseMQModel()  { }

    /// <summary>
    /// Базовая модель ответа/результата на запрос
    /// </summary>
    public TResponseMQModel(IEnumerable<ResultMessage> messages) { Messages = [.. messages]; }

    /// <inheritdoc/>
    public static TResponseModel<T> Build(ResponseBaseModel sender)
    {
        return new() { Messages = sender.Messages };
    }
}