﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace DbcLib;

/// <summary>
/// Промежуточный/общий слой контекста базы данных
/// </summary>
public partial class FeedsHaierProffRuContext(DbContextOptions<FeedsHaierProffRuContext> options) : FeedsHaierProffRuLayerContext(options)
{

}