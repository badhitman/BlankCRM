////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace KladrService;

/// <summary>
/// КЛАДР 4.0
/// </summary>
public class KladrServiceImpl(
    IDbContextFactory<KladrContext> kladrDbFactory,
    ILogger<KladrServiceImpl> loggerRepo) : IKladrService
{
    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req)
    {
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
        if (req.ForTemporary)
        {
            return new()
            {
                AltnamesCount = await context.temp_AltnamesKLADR.CountAsync(),
                NamesCount = await context.temp_NamesMapsKLADR.CountAsync(),
                ObjectsCount = await context.temp_ObjectsKLADR.CountAsync(),
                SocrbasesCount = await context.temp_SocrbasesKLADR.CountAsync(),
                StreetsCount = await context.temp_StreetsKLADR.CountAsync(),
                DomaCount = await context.temp_HousesKLADR.CountAsync(),
            };
        }

        return new()
        {
            AltnamesCount = await context.AltnamesKLADR.CountAsync(),
            NamesCount = await context.NamesMapsKLADR.CountAsync(),
            ObjectsCount = await context.ObjectsKLADR.CountAsync(),
            SocrbasesCount = await context.SocrbasesKLADR.CountAsync(),
            StreetsCount = await context.StreetsKLADR.CountAsync(),
            DomaCount = await context.HousesKLADR.CountAsync(),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearTempKladr()
    {
        loggerRepo.LogInformation($"call > {nameof(ClearTempKladr)}");
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        await context.temp_StreetsKLADR.ExecuteDeleteAsync();
        await context.temp_SocrbasesKLADR.ExecuteDeleteAsync();
        await context.temp_ObjectsKLADR.ExecuteDeleteAsync();
        await context.temp_NamesMapsKLADR.ExecuteDeleteAsync();
        await context.temp_AltnamesKLADR.ExecuteDeleteAsync();
        await context.temp_HousesKLADR.ExecuteDeleteAsync();

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTempKladrModel req)
    {
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        if(req.StreetsPart is not null && req.StreetsPart.Length != 0)
         await context.temp_StreetsKLADR.AddRangeAsync(req.StreetsPart.Select(StreetKLADRModelDB.Build));

        if (req.SocrbasesPart is not null && req.SocrbasesPart.Length != 0)
            await context.temp_SocrbasesKLADR.AddRangeAsync(req.SocrbasesPart.Select(SocrbaseKLADRModelDB.Build));

        if (req.ObjectsPart is not null && req.ObjectsPart.Length != 0)
            await context.temp_ObjectsKLADR.AddRangeAsync(req.ObjectsPart.Select(ObjectKLADRModelDB.Build));

        if (req.NamesPart is not null && req.NamesPart.Length != 0)
            await context.temp_NamesMapsKLADR.AddRangeAsync(req.NamesPart.Select(NameMapKLADRModelDB.Build));

        if (req.AltnamesPart is not null && req.AltnamesPart.Length != 0)
            await context.temp_AltnamesKLADR.AddRangeAsync(req.AltnamesPart.Select(AltnameKLADRModelDB.Build));

        if (req.HousesPart is not null && req.HousesPart.Length != 0)
            await context.temp_HousesKLADR.AddRangeAsync(req.HousesPart.Select(HouseKLADRModelDB.Build));

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}