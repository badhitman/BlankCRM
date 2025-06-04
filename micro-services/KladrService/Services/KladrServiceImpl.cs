﻿////////////////////////////////////////////////
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
IHttpClientFactory HttpClientFactory,
ILogger<KladrServiceImpl> loggerRepo) : IKladrService
{
    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladrAsync(GetMetadataKladrRequestModel req, CancellationToken token = default)
    {
        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.RabbitMqManagement.ToString());

        TResponseModel<List<RabbitMqManagementResponseModel>> resMq = default!;
        MetadataKladrModel res = default!;
        IEnumerable<RabbitMqManagementResponseModel>? q = default!;

        await Task.WhenAll([Task.Run(async () =>
        {
        resMq = await client.GetStringAsync<List<RabbitMqManagementResponseModel>>($"api/queues", cancellationToken : token);
         q = resMq.Response?
            .Where(x => x.name?.Equals(GlobalStaticConstantsTransmission.TransmissionQueues.UploadPartTempKladrReceive) == true);
        }, token), Task.Run(async () =>
        {

        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        res = req.ForTemporary
            ? new() {
            AltnamesCount = await context.TempAltnamesKLADR.CountAsync(),
            NamesCount = await context.TempNamesMapsKLADR.CountAsync(),
            ObjectsCount = await context.TempObjectsKLADR.CountAsync(),
            SocrbasesCount = await context.TempSocrbasesKLADR.CountAsync(),
            StreetsCount = await context.TempStreetsKLADR.CountAsync(),
            DomaCount = await context.TempHousesKLADR.CountAsync()}
            : new() {
            AltnamesCount = await context.AltnamesKLADR.CountAsync(),
            NamesCount = await context.NamesMapsKLADR.CountAsync(),
            ObjectsCount = await context.ObjectsKLADR.CountAsync(),
            SocrbasesCount = await context.SocrbasesKLADR.CountAsync(),
            StreetsCount = await context.StreetsKLADR.CountAsync(),
            DomaCount = await context.HousesKLADR.CountAsync()};
        }, token)]);

        res.RabbitMqManagement = q?.FirstOrDefault(x => x.name?.Equals(GlobalStaticConstantsTransmission.TransmissionQueues.UploadPartTempKladrReceive) == true);
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearTempKladrAsync(CancellationToken token = default)
    {
        loggerRepo.LogInformation($"call > {nameof(ClearTempKladrAsync)}");
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);
        await context.EmptyTemplateTablesAsync();
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken token = default)
    {
        string tableName = req.TableName[..req.TableName.IndexOf('.')];
        bool valid = Enum.TryParse(tableName, true, out KladrFilesEnum currentKladrElement);
        if (!valid)
            return ResponseBaseModel.CreateError($"Имя таблицы `{req.TableName}` не валидное. Разрешённые имена: {string.Join(", ", Enum.GetNames<KladrFilesEnum>().Select(x => $"{x}.dbf"))}"); ;

        req.RowsData.RemoveAll(x => x.All(y => y is null || string.IsNullOrWhiteSpace(y.ToString())));
        if (req.RowsData.Count == 0)
            return ResponseBaseModel.CreateWarning("Строк данных нет");

        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);

        switch (currentKladrElement)
        {
            case KladrFilesEnum.ALTNAMES:
                TResponseModel<List<AltnameKLADRModel>> altnamesPart = AltnameKLADRModel.Build(req.Columns, req.RowsData);

                if (!altnamesPart.Success())
                    return altnamesPart;

                if (altnamesPart.Response is null || altnamesPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("altnamesPart.Response is null || altnamesPart.Response.Length == 0");

                await context.TempAltnamesKLADR.AddRangeAsync(altnamesPart.Response.Select(AltnameKLADRModelDB.Build), token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.ALTNAMES}` - {await context.SaveChangesAsync(token)}");
            case KladrFilesEnum.DOMA:
                TResponseModel<List<HouseKLADRModel>> housesPart = HouseKLADRModel.Build(req.Columns, req.RowsData);

                if (!housesPart.Success())
                    return housesPart;

                if (housesPart.Response is null || housesPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("housesPart.Response is null || housesPart.Response.Length == 0");

                await context.TempHousesKLADR.AddRangeAsync(housesPart.Response.Select(HouseKLADRModelDB.Build), cancellationToken: token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.DOMA}` - {await context.SaveChangesAsync(token)}");
            case KladrFilesEnum.NAMEMAP:
                TResponseModel<List<NameMapKLADRModel>> namesPart = NameMapKLADRModel.Build(req.Columns, req.RowsData);

                if (!namesPart.Success())
                    return namesPart;

                if (namesPart.Response is null || namesPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("namesPart.Response is null || namesPart.Response.Length == 0");

                await context.TempNamesMapsKLADR.AddRangeAsync(namesPart.Response.Select(NameMapKLADRModelDB.Build), cancellationToken: token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.NAMEMAP}` - {await context.SaveChangesAsync(token)}");
            case KladrFilesEnum.SOCRBASE:
                TResponseModel<List<SocrbaseKLADRModel>> socrbasesPart = SocrbaseKLADRModel.Build(req.Columns, req.RowsData);

                if (!socrbasesPart.Success())
                    return socrbasesPart;

                if (socrbasesPart.Response is null || socrbasesPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("socrbasesPart.Response is null || socrbasesPart.Response.Length == 0");

                await context.TempSocrbasesKLADR.AddRangeAsync(socrbasesPart.Response.Select(SocrbaseKLADRModelDB.Build), token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.SOCRBASE}` - {await context.SaveChangesAsync(token)}");
            case KladrFilesEnum.KLADR:
                TResponseModel<List<ObjectKLADRModel>> objectsPart = ObjectKLADRModel.Build(req.Columns, req.RowsData);

                if (!objectsPart.Success())
                    return objectsPart;

                if (objectsPart.Response is null || objectsPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("objectsPart.Response is null || objectsPart.Response.Length == 0");

                await context.TempObjectsKLADR.AddRangeAsync(objectsPart.Response.Select(ObjectKLADRModelDB.Build), token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.KLADR}` - {await context.SaveChangesAsync(token)}");
            case KladrFilesEnum.STREET:
                TResponseModel<List<RootKLADRModel>> streetsPart = RootKLADRModel.Build(req.Columns, req.RowsData);

                if (!streetsPart.Success())
                    return streetsPart;

                if (streetsPart.Response is null || streetsPart.Response.Count == 0)
                    return ResponseBaseModel.CreateError("streetsPart.Response is null || streetsPart.Response.Length == 0");

                await context.TempStreetsKLADR.AddRangeAsync(streetsPart.Response.Select(StreetKLADRModelDB.Build), token);
                return ResponseBaseModel.CreateSuccess($"Добавлено `{KladrFilesEnum.STREET}` - {await context.SaveChangesAsync(token)}");
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FlushTempKladrAsync(CancellationToken token = default)
    {
        loggerRepo.LogInformation($"call > {nameof(FlushTempKladrAsync)}");
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync(token);

        try
        {
            await context.FlushTempKladr(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }
        catch (Exception ex)
        {
            return ResponseBaseModel.CreateError(ex);
        }
    }
}