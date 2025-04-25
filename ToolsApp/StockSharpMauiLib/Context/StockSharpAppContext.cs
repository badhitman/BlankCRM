////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class StockSharpAppContext(DbContextOptions<StockSharpAppContext> options) : StockSharpAppLayerContext(options)
{

}