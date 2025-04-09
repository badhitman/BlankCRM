using Microsoft.EntityFrameworkCore;

namespace DbLayerLib;

/// <inheritdoc/>
public static class EntityHelpers
{
    /// <summary>
    /// Имя таблицы со схемой
    /// </summary>
    public static string GetTableNameWithScheme<T>(this DbContext context) where T : class
    {
        Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType = context.Model.FindEntityType(typeof(T))!;
        string? schema = entityType.GetDefaultSchema();
        return string.IsNullOrWhiteSpace(schema)
            ? $"\"{entityType.GetTableName()}\""
            : $"\"{schema}\".\"{entityType.GetTableName()}\"";
    }
}