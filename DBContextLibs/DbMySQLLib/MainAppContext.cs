////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbLayerLib;
using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Контекст доступа к MySQL БД
/// </summary>
public class MainAppContext(DbContextOptions<MainAppContext> options) : LayerContext(options)
{

}