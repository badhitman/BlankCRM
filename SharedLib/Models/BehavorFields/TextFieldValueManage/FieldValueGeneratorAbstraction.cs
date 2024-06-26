﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Генератор значений для выбора клиентом в специализированном контроле
/// </summary>
public abstract class FieldValueGeneratorAbstraction : DeclarationAbstraction
{
    /// <summary>
    /// модель запроса (для примера/схемы пользователю)
    /// </summary>
    public object RequestModel = new();

    /// <summary>
    /// Элементы, полученные от генератора
    /// </summary>
    public abstract TResponseModel<string[]> GetListElements(FieldFormConstructorModelDB field, SessionOfDocumentDataModelDB session_Questionnaire, TabJoinDocumentSchemeConstructorModelDB? page_join_form = null, uint row_num = 0);
}