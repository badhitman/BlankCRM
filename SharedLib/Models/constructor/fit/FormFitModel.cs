﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Форма
/// </summary>
public class FormFitModel : BaseFormFitModel
{
    /// <summary>
    /// Имя связи формы с табом
    /// </summary>
    public string? JoinName { get; set; }

    /// <summary>
    /// Табличная часть
    /// </summary>
    public bool IsTable { get; set; }

    /// <summary>
    /// Заголовок формы
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Простые поля
    /// </summary>
    public FieldFitModel[]? SimpleFields { get; set; }

    /// <summary>
    /// Поля типа: справочник/список/перечисление
    /// </summary>
    public FieldAkaDirectoryFitModel[]? FieldsAtDirectories { get; set; }

    /// <summary>
    /// Все поля формы (отсортированные)
    /// </summary>
    public List<BaseRequiredFormFitModel> AllFields
    {
        get
        {
            List<BaseRequiredFormFitModel> res = [];

            if (SimpleFields is not null && SimpleFields.Length != 0)
                res.AddRange(SimpleFields);

            if (FieldsAtDirectories is not null && FieldsAtDirectories.Length != 0)
                res.AddRange(FieldsAtDirectories);

            return [.. res.OrderBy(x => x.SortIndex)];
        }
    }
}