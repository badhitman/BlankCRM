﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace DbcLib;

/// <inheritdoc/>
public abstract partial class KladrLayerContext : DbContext
{
    /// <inheritdoc/>
    public KladrLayerContext(DbContextOptions options)
        : base(options)
    {
        //#if DEBUG
        //        Database.EnsureCreated();
        //#else
        Database.Migrate();
        //#endif
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.EnableSensitiveDataLogging(true);
        options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }

    /// <summary>
    /// Очистка временных таблиц
    /// </summary>
    public abstract Task EmptyTemplateTables();

    /// <summary>
    /// Altnames содержит сведения о соответствии кодов старых и новых наименований (обозначений домов) в случаях переподчинения 
    /// и “сложного” переименования адресных объектов (когда коды записей со старым и новым наименованиями не совпадают).
    /// </summary>
    /// <remarks>
    /// Возможные варианты “сложного” переименования:
    /// улица разделилась на несколько новых улиц;
    /// несколько улиц объединились в одну новую улицу;
    /// населенный пункт стал улицей другого города(населенного пункта);
    /// улица населенного пункта стала улицей другого города(населенного пункта).
    /// В этих случаях производятся следующие действия:
    /// вводятся новые объекты в файлы Kladr.dbf, Street.dbf и Doma.dbf;
    /// старые объекты переводятся в разряд неактуальных;
    /// в файл Altnames вводятся записи, содержащие соответствие старых и новых кодов адресных объектов.
    /// </remarks>
    public DbSet<AltnameKLADRModelDB> AltnamesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<NameMapKLADRModelDB> NamesMapsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ObjectKLADRModelDB> ObjectsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<SocrbaseKLADRModelDB> SocrbasesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<StreetKLADRModelDB> StreetsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<HouseKLADRModelDB> HousesKLADR { get; set; } = default!;



    /// <inheritdoc/>
    public DbSet<AltnameTempKLADRModelDB> TempAltnamesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<NameMapTempKLADRModelDB> TempNamesMapsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<ObjectTempKLADRModelDB> TempObjectsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<SocrbaseTempKLADRModelDB> TempSocrbasesKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<StreetTempKLADRModelDB> TempStreetsKLADR { get; set; } = default!;

    /// <inheritdoc/>
    public DbSet<HouseTempKLADRModelDB> TempHousesKLADR { get; set; } = default!;
}