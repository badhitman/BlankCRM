﻿using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbLayerLib;

public partial class LayerContext
{
    /// <summary>
    /// Projects
    /// </summary>
    public DbSet<ManageManufactureModelDB> Manufactures { get; set; }
}