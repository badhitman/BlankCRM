////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using HtmlGenerator.html5.textual;
using HtmlGenerator.html5.tables;
using HtmlGenerator.html5.areas;
using DocumentFormat.OpenXml;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService(
    IIdentityTransmission identityRepo,
    IDbContextFactory<CommerceContext> commerceDbFactory,
    IWebTransmission webTransmissionRepo,
    IHelpdeskTransmission HelpDeskRepo,
    IRubricsService RubricsRepo,
    IStorageTransmission FilesRepo,
    IHistoryIndexing indexingRepo,
    ITelegramTransmission tgRepo,
    ILogger<CommerceImplementService> loggerRepo,
    WebConfigModel _webConf,
    IParametersStorageTransmission StorageTransmissionRepo) : ICommerceService
{
    #region payment-document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdateOrCreateAsync(TAuthRequestStandardModel<PaymentDocumentBaseModel> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.Payload is null || req.Payload.Amount <= 0)
        {
            res.AddError("Сумма платежа должна быть больше нуля");
            return res;
        }
        if (req.Payload.OrderId < 1)
        {
            res.AddError("Не указан документ-заказ");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DateTime dtu = DateTime.UtcNow;

        PaymentDocumentModelDb? payment_db = null;
        if (!string.IsNullOrWhiteSpace(req.Payload.ExternalDocumentId))
        {
            payment_db = await context
               .PaymentsB2B
               .FirstOrDefaultAsync(x => x.ExternalDocumentId == req.Payload.ExternalDocumentId, cancellationToken: token);

            req.Payload.Id = req.Payload.Id > 0 ? req.Payload.Id : payment_db?.Id ?? 0;
        }

        if (req.Payload.Id < 1)
        {
            payment_db = new()
            {
                Name = req.Payload.Name,
                Amount = req.Payload.Amount,
                OrderId = req.Payload.OrderId,
                ExternalDocumentId = req.Payload.ExternalDocumentId,
            };

            await context.PaymentsB2B.AddAsync(payment_db, token);
            await context.SaveChangesAsync(token);

            res.AddSuccess("Платёж добавлен");
            res.Response = req.Payload.Id;
            return res;
        }

        res.Response = await context.PaymentsB2B
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Name, req.Payload.Name)
            .SetProperty(p => p.Amount, req.Payload.Amount), cancellationToken: token);

        await context.OrdersB2B
               .Where(x => x.Id == req.Payload.OrderId)
               .ExecuteUpdateAsync(set => set.SetProperty(p => p.LastUpdatedAtUTC, dtu), cancellationToken: token);


        if (!string.IsNullOrWhiteSpace(req.Payload.ExternalDocumentId) && payment_db?.ExternalDocumentId != req.Payload.ExternalDocumentId)
            res.Response = await context.PaymentsB2B
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.ExternalDocumentId, req.Payload.ExternalDocumentId), cancellationToken: token);

        res.AddSuccess($"Обновление `{GetType().Name}` выполнено");
        return res;
    }
    #endregion

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default)
    {

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(req.SenderActionUserId))
            return new() { Messages = [new() { Text = "string.IsNullOrEmpty(req.SenderActionUserId)", TypeMessage = MessagesTypesEnum.Error }] };

        TResponseModel<UserInfoModel[]> rest = default!;
        TResponseModel<OrderDocumentModelDB[]> orderData = default!;
        List<Task> _taskList = [
            Task.Run(async () => { rest = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId]); }, token),
            Task.Run(async () => { orderData = await OrdersReadAsync(new(){ Payload = [req.Payload], SenderActionUserId = req.SenderActionUserId }); }, token)];

        await Task.WhenAll(_taskList);

        if (!rest.Success() || rest.Response is null || rest.Response.Length != 1)
            return new() { Messages = rest.Messages };

        TResponseModel<FileAttachModel> res = new();
        if (!orderData.Success() || orderData.Response is null || orderData.Response.Length != 1)
        {
            res.AddRangeMessages(orderData.Messages);
            return res;
        }

        OrderDocumentModelDB orderDb = orderData.Response[0];
        UserInfoModel actor = rest.Response[0];
        bool allowed = actor.IsAdmin || orderDb.AuthorIdentityUserId == actor.UserId || actor.UserId == GlobalStaticConstantsRoles.Roles.System;
        if (!allowed && orderDb.HelpDeskId.HasValue && orderDb.HelpDeskId.Value > 0)
        {
            TResponseModel<IssueHelpDeskModelDB[]> issueData = await HelpDeskRepo.IssuesReadAsync(new TAuthRequestStandardModel<IssuesReadRequestModel>()
            {
                SenderActionUserId = req.SenderActionUserId,
                Payload = new()
                {
                    IssuesIds = [orderDb.HelpDeskId.Value],
                    IncludeSubscribersOnly = true,
                }
            }, token);
            if (!issueData.Success() || issueData.Response is null || issueData.Response.Length != 1)
            {
                res.AddRangeMessages(issueData.Messages);
                return res;
            }
            IssueHelpDeskModelDB issueDb = issueData.Response[0];
            if (actor.UserId != issueDb.AuthorIdentityUserId && actor.UserId != issueDb.ExecutorIdentityUserId && !issueDb.Subscribers!.Any(s => s.UserId == actor.UserId))
            {
                res.AddError("У вас не доступа к этому документу");
                return res;
            }
        }
        else if (!allowed)
        {
            res.AddError("У вас не доступа к этому документу");
            return res;
        }

        string docName = $"Заказ {orderDb.Name} от {orderDb.CreatedAtUTC.GetHumanDateTime()}";

        try
        {
            res.Response = new()
            {
                Data = SaveOrderAsExcel(orderDb),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("xlsx")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.xlsx",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            div wrapDiv = new();
            wrapDiv.AddDomNode(new p(docName));

            orderDb.OfficesTabs!.ForEach(aNode =>
            {
                div addressDiv = new();
                addressDiv.AddDomNode(new p($"Адрес: `{aNode.Office?.Name}`"));

                table my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };
                my_table.THead.AddColumn("Наименование").AddColumn("Цена").AddColumn("Кол-во").AddColumn("Сумма");

                aNode.Rows?.ForEach(dr =>
                {
                    my_table.TBody.AddRow([dr.Offer!.GetName(), dr.Offer.Price.ToString(), dr.Quantity.ToString(), dr.Amount.ToString()]);
                });
                addressDiv.AddDomNode(my_table);
                addressDiv.AddDomNode(new p($"Итого: {aNode.Rows!.Sum(x => x.Amount)}") { css_style = "float: right;" });
                wrapDiv.AddDomNode(addressDiv);
            });

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string test_s = $"<style>table, th, td {{border: 1px solid black;border-collapse: collapse;}}</style>{wrapDiv.GetHTML()}";

            using MemoryStream ms = new();
            StreamWriter writer = new(ms);
            writer.Write(test_s);
            writer.Flush();
            ms.Position = 0;

            res.Response = new()
            {
                Data = ms.ToArray(),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetExcelAsync(CancellationToken token = default)
    {
        string docName = $"Прайс на {DateTime.Now.GetHumanDateTime()}";
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        List<OfferModelDB> offersAll = await context.Offers
            .Include(x => x.Nomenclature)
            .Include(x => x.Registers)
            .ToListAsync(cancellationToken: token);

        if (offersAll.Count == 0)
        {
            loggerRepo.LogWarning($"Пустой прайс: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        int[] rubricsIds = [.. offersAll.SelectMany(x => x.Registers!).Select(x => x.WarehouseId).Distinct()];
        if (rubricsIds.Length == 0)
            return new()
            {
                Data = Encoding.UTF8.GetBytes("Warehouse not any"),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };

        TResponseModel<List<RubricStandardModel>> rubricsDb = await RubricsRepo.RubricsGetAsync(rubricsIds, token);
        List<IGrouping<NomenclatureModelDB?, OfferModelDB>> gof = offersAll.GroupBy(x => x.Nomenclature).Where(x => x.Any(y => y.Registers!.Any(z => z.Quantity > 0))).ToList();
        try
        {
            return new()
            {
                Data = ExportPrice(gof, rubricsDb.Response),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("xlsx")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.xlsx",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            return new()
            {
                Data = Encoding.UTF8.GetBytes(ex.Message),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }
    }

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetJsonAsync(CancellationToken token = default)
    {
        string docName = $"Прайс на {DateTime.Now.GetHumanDateTime()}";
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        List<OfferModelDB> offersAll = await context.Offers
            .Include(x => x.Nomenclature)
            .Include(x => x.Registers)
            .ToListAsync(cancellationToken: token);

        if (offersAll.Count == 0)
        {
            loggerRepo.LogWarning($"Пустой прайс: {docName}");
            return new()
            {
                Data = Encoding.UTF8.GetBytes("Пустой прайс"),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }

        int[] rubricsIds = [.. offersAll.SelectMany(x => x.Registers!).Select(x => x.WarehouseId).Distinct()];
        TResponseModel<List<RubricStandardModel>> rubricsDb = await RubricsRepo.RubricsGetAsync(rubricsIds, token);

        List<NomenclatureScopeModel> _catalogData = [];
        foreach (IGrouping<NomenclatureModelDB?, OfferModelDB> _gn in offersAll.GroupBy(x => x.Nomenclature))
        {
            NomenclatureScopeModel _nom = new()
            {
                BaseUnit = _gn.Key!.BaseUnit.ToString(),
                IsDisabled = _gn.Key.IsDisabled,
                Description = _gn.Key.Description,
                Name = _gn.Key.Name,
                Offers = [.. _gn.Select(x => new OfferScopeModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    IsDisabled = x.IsDisabled,
                    Multiplicity = x.Multiplicity,
                    OfferUnit = x.OfferUnit.ToString(),
                    Price = x.Price,
                    QuantitiesTemplate = x.QuantitiesTemplate,
                    ShortName = x.ShortName,
                    Weight = x.Weight,
                    Registers = [.. x.Registers!.Select(y=> new OfferAvailabilityScopeModel()
                    {
                        Warehouse = rubricsDb.Response!.First(r=>r.Id == y.WarehouseId).Name!,
                         Quantity = y.Quantity,
                    })]
                })]
            };
            _catalogData.Add(_nom);
        }

        string inputString = JsonConvert.SerializeObject(_catalogData, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);

        try
        {
            return new()
            {
                Data = Encoding.UTF8.GetBytes(inputString),
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("json")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.json",
            };
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка создания Excel документа: {docName}");
            return new()
            {
                Data = [],
                ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
                Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
            };
        }
    }

    #region static
    static byte[] SaveOrderAsExcel(OrderDocumentModelDB orderDb)
    {
        string docName = $"Заказ {orderDb.Name} от {orderDb.CreatedAtUTC.GetHumanDateTime()}";
        using MemoryStream XLSStream = new();
        using SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(XLSStream, SpreadsheetDocumentType.Workbook);

        WorkbookPart wBookPart = spreadsheetDoc.AddWorkbookPart();
        wBookPart.Workbook = new Workbook();
        uint sheetId = 1;
        //WorkbookPart workbookPart = spreadsheetDoc.WorkbookPart ?? spreadsheetDoc.AddWorkbookPart();

        WorkbookStylesPart wbsp = wBookPart.AddNewPart<WorkbookStylesPart>();

        wbsp.Stylesheet = GenerateExcelStyleSheet();
        wbsp.Stylesheet.Save();

        wBookPart.Workbook.Sheets = new Sheets();

        Sheets sheets = wBookPart.Workbook.GetFirstChild<Sheets>() ?? wBookPart.Workbook.AppendChild(new Sheets());

        foreach (var table in orderDb.OfficesTabs!)
        {
            WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
            Sheet sheet = new() { Id = wBookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = table.Office?.Name };
            sheets.Append(sheet);

            SheetData sheetData = new();
            wSheetPart.Worksheet = new Worksheet(sheetData);

            Columns lstColumns = wSheetPart.Worksheet.GetFirstChild<Columns>()!;
            bool needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }

            lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 100, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });

            if (needToInsertColumns)
                wSheetPart.Worksheet.InsertAt(lstColumns, 0);

            Row topRow = new() { RowIndex = 2 };
            InsertExcelCell(topRow, 1, $"Адрес доставки: {table.Office?.AddressUserComment}", CellValues.String, 0);
            sheetData!.Append(topRow);

            Row headerRow = new() { RowIndex = 4 };
            InsertExcelCell(headerRow, 1, "Наименование", CellValues.String, 1);
            InsertExcelCell(headerRow, 2, "Цена", CellValues.String, 1);
            InsertExcelCell(headerRow, 3, "Кол-во", CellValues.String, 1);
            InsertExcelCell(headerRow, 4, "Сумма", CellValues.String, 1);
            sheetData.AppendChild(headerRow);

            uint row_index = 5;
            foreach (RowOfOrderDocumentModelDB dr in table.Rows!)
            {
                Row row = new() { RowIndex = row_index++ };
                InsertExcelCell(row, 1, dr.Offer!.GetName(), CellValues.String, 0);
                InsertExcelCell(row, 2, dr.Offer.Price.ToString(), CellValues.String, 0);
                InsertExcelCell(row, 3, dr.Quantity.ToString(), CellValues.String, 0);
                InsertExcelCell(row, 4, dr.Amount.ToString(), CellValues.String, 0);
                sheetData.Append(row);
            }
            Row btRow = new() { RowIndex = row_index++ };
            InsertExcelCell(btRow, 1, "", CellValues.String, 0);
            InsertExcelCell(btRow, 2, "", CellValues.String, 0);
            InsertExcelCell(btRow, 3, "Итого:", CellValues.String, 0);
            InsertExcelCell(btRow, 4, table.Rows!.Sum(x => x.Amount).ToString(), CellValues.String, 0);
            sheetData.Append(btRow);
            sheetId++;
        }

        wBookPart.Workbook.Save();
        spreadsheetDoc.Save();

        XLSStream.Position = 0;
        return XLSStream.ToArray();
    }

    static byte[] ExportPrice(List<IGrouping<NomenclatureModelDB?, OfferModelDB>> sourceTable, List<RubricStandardModel>? rubricsDb)
    {
        using MemoryStream XLSStream = new();
        using SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(XLSStream, SpreadsheetDocumentType.Workbook);

        WorkbookPart wBookPart = spreadsheetDoc.AddWorkbookPart();
        wBookPart.Workbook = new Workbook();
        uint sheetId = 1;

        WorkbookStylesPart wbsp = wBookPart.AddNewPart<WorkbookStylesPart>();

        wbsp.Stylesheet = GenerateExcelStyleSheet();
        wbsp.Stylesheet.Save();

        wBookPart.Workbook.Sheets = new Sheets();

        Sheets sheets = wBookPart.Workbook.GetFirstChild<Sheets>() ?? wBookPart.Workbook.AppendChild(new Sheets());

        foreach (IGrouping<NomenclatureModelDB?, OfferModelDB> table in sourceTable)
        {
            WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
            Sheet sheet = new() { Id = wBookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = table.Key?.Name };
            sheets.Append(sheet);

            SheetData sheetData = new();
            wSheetPart.Worksheet = new Worksheet(sheetData);

            Columns lstColumns = wSheetPart.Worksheet.GetFirstChild<Columns>()!;
            bool needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }

            lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 100, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 8, CustomWidth = true, BestFit = true, });
            lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });

            if (needToInsertColumns)
                wSheetPart.Worksheet.InsertAt(lstColumns, 0);

            Row topRow = new() { RowIndex = 2 };
            InsertExcelCell(topRow, 1, $"Дата формирования: {DateTime.Now.GetHumanDateTime()}", CellValues.String, 0);
            sheetData!.Append(topRow);

            Row headerRow = new() { RowIndex = 4 };
            InsertExcelCell(headerRow, 1, "Наименование", CellValues.String, 1);
            InsertExcelCell(headerRow, 2, "Цена", CellValues.String, 1);
            InsertExcelCell(headerRow, 3, "Ед.изм.", CellValues.String, 1);
            InsertExcelCell(headerRow, 4, "Остаток/Склад", CellValues.String, 1);
            sheetData.AppendChild(headerRow);

            uint row_index = 5;
            foreach (OfferModelDB dr in table)
            {
                foreach (IGrouping<int, OfferAvailabilityModelDB> nodeG in dr.Registers!.GroupBy(x => x.WarehouseId))
                {
                    Row row = new() { RowIndex = row_index++ };
                    sheetData.AppendChild(row);
                    InsertExcelCell(row, 1, dr!.GetName(), CellValues.String, 0);
                    InsertExcelCell(row, 2, dr.Price.ToString(), CellValues.String, 0);
                    InsertExcelCell(row, 3, dr.OfferUnit.DescriptionInfo(), CellValues.String, 0);
                    InsertExcelCell(row, 4, $"{nodeG.Sum(x => x.Quantity)} /{rubricsDb?.FirstOrDefault(r => r.Id == nodeG.Key)?.Name}", CellValues.String, 0);
                }
                ;
            }
            Row btRow = new() { RowIndex = row_index++ };
            InsertExcelCell(btRow, 1, "", CellValues.String, 0);
            InsertExcelCell(btRow, 2, "", CellValues.String, 0);
            InsertExcelCell(btRow, 3, "Итого:", CellValues.String, 0);
            InsertExcelCell(btRow, 4, table!.Sum(x => x.Registers!.Sum(y => y.Quantity)).ToString(), CellValues.String, 0);
            sheetData.Append(btRow);
            sheetId++;
        }

        wBookPart.Workbook.Save();
        spreadsheetDoc.Save();
        XLSStream.Position = 0;

        return XLSStream.ToArray();
    }

    static Stylesheet GenerateExcelStyleSheet()
    {
        return new Stylesheet(
            new Fonts(
                new Font(                                                               // Стиль под номером 0 - Шрифт по умолчанию.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Стиль под номером 1 - Жирный шрифт Times New Roman.
                    new Bold(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 2 - Обычный шрифт Times New Roman.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 3 - Шрифт Times New Roman размером 14.
                    new FontSize() { Val = 14 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
        new FontName() { Val = "Times New Roman" })
        ),
        new Fills(
                new Fill(                                                           // Стиль под номером 0 - Заполнение ячейки по умолчанию.
        new PatternFill() { PatternType = PatternValues.None }),
                new Fill(                                                           // Стиль под номером 1 - Заполнение ячейки серым цветом
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFAAAAAA" } }
        )
                    { PatternType = PatternValues.Solid }),
        new Fill(                                                           // Стиль под номером 2 - Заполнение ячейки красным.
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFEFEFEF" } }
                    )
                    { PatternType = PatternValues.Solid })
            )
            ,
            new Borders(
                new Border(                                                         // Стиль под номером 0 - Грани.
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder(),
                    new BottomBorder(),
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 1 - Грани
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 2 - Грани.
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            ),
            new CellFormats(
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Стиль под номером 0 - The default cell style.  (по умолчанию)
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 1, FillId = 2, BorderId = 1, ApplyFont = true },       // Стиль под номером 1 - Bold 
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true },       // Стиль под номером 2 - REgular
                new CellFormat() { FontId = 3, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 },       // Стиль под номером 3 - Times Roman
                new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Стиль под номером 4 - Yellow Fill
                new CellFormat(                                                                   // Стиль под номером 5 - Alignment
                    new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                )
                { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },      // Стиль под номером 6 - Border
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 }       // Стиль под номером 7 - Задает числовой формат полю.
            )
        );
    }

    static void InsertExcelCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
    {
        Cell? refCell = null;
        Cell newCell = new() { CellReference = cell_num.ToString() + ":" + row.RowIndex?.ToString(), StyleIndex = styleIndex };
        row.InsertBefore(newCell, refCell);

        newCell.CellValue = new CellValue(val);
        newCell.DataType = new EnumValue<CellValues>(type);
    }
    #endregion
}