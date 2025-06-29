////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Сопроводительное сообщение к результату выполнения операции сервером
/// </summary>
public class ResultMessage
{
    /// <summary>
    /// Тип сообщения (ошибка, инфо и т.п.)
    /// </summary>
    public MessagesTypesEnum TypeMessage { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is ResultMessage other)
            return other.TypeMessage == TypeMessage && other.Text == Text;

        return false;
    }

    /// <inheritdoc/>        
    public override string ToString() => $"({TypeMessage}) {Text}";
}