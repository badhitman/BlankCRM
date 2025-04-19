////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// Базовая модель ответа/результата на запрос
/// </summary>
public class TResponseMQBaseModel<T> : ResponseBaseModel
{
    /// <summary>
    /// Получен запрос
    /// </summary>
    public DateTime StartedServer { get; set; }

    /// <summary>
    /// Запрос обработан
    /// </summary>
    public DateTime FinalizedServer { get; set; }

    /// <summary>
    /// Duration
    /// </summary>
    public TimeSpan Duration() => FinalizedServer - StartedServer;

    /// <summary>
    /// Полезная нагрузка ответа
    /// </summary>
    public virtual T? Response { get; set; }
}