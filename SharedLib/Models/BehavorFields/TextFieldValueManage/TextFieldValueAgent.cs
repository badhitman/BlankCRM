﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Агент обработки поведения текстового поля
/// </summary>
public abstract class TextFieldValueAgent : DeclarationAbstraction
{
    /// <summary>
    /// Автоматическая установка значения (если значение NULL)
    /// </summary>
    public abstract string? DefaultValueIfNull(FieldFormConstructorModelDB field, SessionOfDocumentDataModelDB session_Questionnaire, int page_join_form_id);
}