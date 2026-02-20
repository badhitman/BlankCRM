////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Контекст доступа к Postgres
/// </summary>
public class RealtimeContext(DbContextOptions<RealtimeContext> options) : RealtimeLayerContext(options)
{

}