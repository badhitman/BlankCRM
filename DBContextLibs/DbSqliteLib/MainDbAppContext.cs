﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbLayerLib;
using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Контекст доступа к SQLite БД
/// </summary>
public class MainDbAppContext(DbContextOptions<MainDbAppContext> options) : LayerContext(options)
{

}