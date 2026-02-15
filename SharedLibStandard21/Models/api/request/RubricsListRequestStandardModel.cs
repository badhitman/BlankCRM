////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RubricsListRequestModel
/// </summary>
public class RubricsListRequestStandardModel : TProjectedRequestStandardModel<int>
{
    /// <summary>
    /// Имя контекста для разделения различных селекторов независимо друг от друга
    /// </summary>
    /// <remarks>
    /// Рубрики HelpDesk имеют значение контекста NULL. А подсистема адресов (Регионы/Города) используют этот эту же службу с указанием на имя контекста: <see cref="GlobalStaticConstantsRoutes.Routes.ADDRESS_CONTROLLER_NAME"/> 
    /// </remarks>
    public string? ContextName { get; set; }

    /// <summary>
    /// Имя-префикс
    /// </summary>
    /// <remarks>
    /// Для организации внутри одного контекста разных наборов рубрик
    /// </remarks>
    public string? PrefixName { get; set; }
}