////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// Сопроводительное сообщение к результату выполнения операции сервером
/// </summary>
public class ResultMessage : IEquatable<ResultMessage>
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
    public bool Equals(ResultMessage other)
    {
        return other is not null &&
               TypeMessage == other.TypeMessage &&
               Text == other.Text;
    }

    /// <inheritdoc/>  
    public override int GetHashCode()
    {
        return HashCode.Combine(TypeMessage, Text);
    }

    /// <inheritdoc/>        
    public override string ToString() => $"({TypeMessage}) {Text}";

    /// <inheritdoc/>  
    public static bool operator ==(ResultMessage left, ResultMessage right)
    {
        if(left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <inheritdoc/>  
    public static bool operator !=(ResultMessage left, ResultMessage right)
    {
        if (left is null && right is null)
            return false;
        if (left is null || right is null)
            return true;

        return !left.Equals(right);
    }
}