////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using SharedLib;
using DbcLib;

namespace FileIndexingService;

/// <summary>
/// Индексирование файлов приложений
/// </summary>
public class IndexingFilesImpl(
    IDbContextFactory<FilesIndexingContext> fileIndexingDbFactory,
    IOptions<MongoConfigModel> mongoConf,
    IStorageTransmission storageRepo) : IFilesIndexing
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default)
    {
        if (req.RequestBody is not null)
            req.RequestBody = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.RequestBody));

        if (req.ResponseBody is not null)
            req.ResponseBody = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.ResponseBody));

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceReceiverRecord> traceReceiverRecords = mongoFs.GetCollection<TraceReceiverRecord>(nameof(TraceReceiverRecord));

        CreateIndexOptions indexOptions = new();
        IndexKeysDefinition<TraceReceiverRecord> indexKeys = Builders<TraceReceiverRecord>.IndexKeys.Ascending(x => x.ReceiverName);
        CreateIndexModel<TraceReceiverRecord> indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceReceiverRecord>.IndexKeys.Ascending(x => x.UTCTimestampInitReceive);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        await traceReceiverRecords.InsertOneAsync(req, cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default)
    {
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceReceiverRecord> traceReceiverRecords = mongoFs.GetCollection<TraceReceiverRecord>(nameof(TraceReceiverRecord));

        IQueryable<TraceReceiverRecord> query = traceReceiverRecords
            .AsQueryable()
            .Where(x => req.Payload == null || req.Payload.ReceiversNames == null || req.Payload.ReceiversNames.Contains(x.ReceiverName));

        Task<int> totalTask = query.CountAsync(cancellationToken: token);
        Task<List<TraceReceiverRecord>> itemsTask = query.Skip(0).Take(10).ToListAsync(cancellationToken: token);
        await Task.WhenAll(totalTask, itemsTask);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = totalTask.Result,
            Response = itemsTask.Result,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default)
    {
        return Path.GetExtension(req.FileName) switch
        {
            ".xlsx" => await SpreadsheetDocumentHandle(req, token),
            ".docx" => await WordprocessingDocumentHandle(req, token),
            _ => ResponseBaseModel.CreateInfo("file format not support"),
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>> SpreadsheetDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        TAuthRequestStandardModel<RequestFileReadModel> fileReq = new()
        {
            SenderActionUserId = req.SenderActionUserId,
            Payload = new()
            {
                FileId = req.Payload
            }
        };
        TResponseModel<FileContentModel>? fileData = await storageRepo.ReadFileAsync(fileReq, token);
        TResponseModel<SpreadsheetDocumentIndexingFileResponseModel> res = new()
        {
            Messages = fileData.Messages
        };

        if (!res.Success())
            return res;

        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);
        res.Response = new()
        {
            Sheets = await context.SheetsExcelIndexesFiles
            .Where(x => x.StoreFileId == req.Payload)
            .Include(x => x.Cells)
            .ToListAsync(cancellationToken: token)
        };

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>> WordprocessingDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        TAuthRequestStandardModel<RequestFileReadModel> fileReq = new()
        {
            SenderActionUserId = req.SenderActionUserId,
            Payload = new()
            {
                FileId = req.Payload
            }
        };
        TResponseModel<FileContentModel>? fileData = await storageRepo.ReadFileAsync(fileReq, token);


        TResponseModel<WordprocessingDocumentIndexingFileResponseModel> res = new()
        {
            Messages = fileData.Messages
        };

        if (!res.Success())
            return res;

        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);
        res.Response = new()
        {
            Paragraphs = await context.ParagraphsWordIndexesFiles.Where(x => x.StoreFileId == req.Payload).ToListAsync(cancellationToken: token),
            Tables = await context.TablesWordIndexesFiles.Where(x => x.StoreFileId == req.Payload).Include(x => x.Data).ToListAsync(cancellationToken: token)
        };
        return res;
    }

    async Task<ResponseBaseModel> SpreadsheetDocumentHandle(StorageFileMiddleModel file_db, CancellationToken token = default)
    {
        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);

        if (await context.SheetsExcelIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("the file is already indexed");

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase(mongoConf.Value.FilesSystemName);
        using MemoryStream stream = new();
        GridFSBucket gridFS = new(mongoFs);
        await gridFS.DownloadToStreamAsync(new ObjectId(file_db.PointId), stream, cancellationToken: token);
        using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, false);
        if (spreadsheetDocument.WorkbookPart is null)
            return ResponseBaseModel.CreateError("spreadsheetDocument.WorkbookPart is null");

        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

        DocumentFormat.OpenXml.Spreadsheet.Sheets? sheets = workbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();

        if (sheets is null)
            return ResponseBaseModel.CreateError("Workbook.Sheets is null");

        List<SheetExcelIndexFileModelDB> sheetsDb = ExcelRead(file_db.Id, workbookPart, sheets);

        if (sheetsDb.Count != 0)
        {
            await context.AddRangeAsync(sheetsDb, token);
            await context.SaveChangesAsync(token);
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    static List<SheetExcelIndexFileModelDB> ExcelRead(int fileId, WorkbookPart workbookPart, DocumentFormat.OpenXml.Spreadsheet.Sheets sheets)
    {
        List<string> sharedStrings = [];
        SharedStringTablePart? sharedStringTablePart = workbookPart.SharedStringTablePart;

        if (sharedStringTablePart is not null)
        {
            foreach (var item in sharedStringTablePart.SharedStringTable.Elements<DocumentFormat.OpenXml.Spreadsheet.SharedStringItem>())
                sharedStrings.Add(item.InnerText);
        }

        List<SheetExcelIndexFileModelDB> sheetsDb = [];
        foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
        {
            string partRelationshipId = workbookPart.GetIdOfPart(worksheetPart);
            DocumentFormat.OpenXml.Spreadsheet.Sheet correspondingSheet = sheets.Cast<DocumentFormat.OpenXml.Spreadsheet.Sheet>().First(s => s.Id == partRelationshipId);
            SheetExcelIndexFileModelDB _newSheetDb = new()
            {
                Title = correspondingSheet.Name?.Value ?? "-no name-",
                Cells = [],
                StoreFileId = fileId,
                SortIndex = sheetsDb.Count,
            };

            foreach (DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData in worksheetPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>())
            {
                uint _rowNum = 0;
                foreach (DocumentFormat.OpenXml.Spreadsheet.Row _row in sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
                {
                    uint _colNum = 0;
                    foreach (DocumentFormat.OpenXml.Spreadsheet.Cell _cell in _row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                    {
                        string? _cellVal = _cell.CellValue?.Text;
                        DocumentFormat.OpenXml.EnumValue<DocumentFormat.OpenXml.Spreadsheet.CellValues>? _cellValType = _cell.DataType;

                        if (!string.IsNullOrWhiteSpace(_cellVal))
                        {
                            if (_cellValType is not null && _cellValType == "s" && int.TryParse(_cellVal, out int _indexStr) && sharedStrings.Count > _indexStr)
                                _cellVal = sharedStrings[_indexStr];

                            _newSheetDb.Cells.Add(new CellTableExcelIndexFileModelDB()
                            {
                                SheetExcelFile = _newSheetDb,
                                StoreFileId = fileId,
                                RowNum = _rowNum,
                                ColNum = _colNum,
                                Data = _cellVal,
                            });
                        }
                        _colNum++;
                    }
                    _rowNum++;
                }
            }
            sheetsDb.Add(_newSheetDb);
        }
        return sheetsDb;
    }

    async Task<ResponseBaseModel> WordprocessingDocumentHandle(StorageFileMiddleModel file_db, CancellationToken token = default)
    {
        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);

        if (await context.TablesWordIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token) || await context.ParagraphsWordIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("the file is already indexed");

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase(mongoConf.Value.FilesSystemName);
        using MemoryStream stream = new();
        GridFSBucket gridFS = new(mongoFs);
        await gridFS.DownloadToStreamAsync(new ObjectId(file_db.PointId), stream, cancellationToken: token);

        using WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(stream, false);
        if (wordprocessingDocument.MainDocumentPart?.Document.Body is null)
            return ResponseBaseModel.CreateError("word-processing document [MainDocumentPart?.Document.Body] is null");

        (List<ParagraphWordIndexFileModelDB> _paragraphs, List<TableWordIndexFileModelDB> _tablesDb) = WordRead(file_db.Id, wordprocessingDocument.MainDocumentPart.Document.Body);

        if (_tablesDb.Count != 0)
            await context.AddRangeAsync(_tablesDb, token);

        if (_paragraphs.Count != 0)
            await context.AddRangeAsync(_paragraphs, token);

        if (_tablesDb.Count != 0 || _paragraphs.Count != 0)
            await context.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    static (List<ParagraphWordIndexFileModelDB> paragraphs, List<TableWordIndexFileModelDB> tables) WordRead(int fileId, DocumentFormat.OpenXml.Wordprocessing.Body bodyDocument)
    {
        List<ParagraphWordIndexFileModelDB> _paragraphs = [];
        List<TableWordIndexFileModelDB> _tablesDb = [];
        int _sortIndex = 0;
        foreach (DocumentFormat.OpenXml.OpenXmlElement element in bodyDocument.Elements())
        {
            if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph _paragraph)
            {
                DocumentFormat.OpenXml.HexBinaryValue? _paragraphId = _paragraph.ParagraphId;
                ParagraphWordIndexFileModelDB _par = new()
                {
                    StoreFileId = fileId,
                    SortIndex = _sortIndex,
                    Data = _paragraph.InnerText,
                    ParagraphId = _paragraphId?.Value,
                };
                _paragraphs.Add(_par);
            }
            else if (element is DocumentFormat.OpenXml.Wordprocessing.Table _table)
            {
                List<CellTableWordIndexFileModelDB> _cellTablesDb = [];

                TableWordIndexFileModelDB _tableDb = new()
                {
                    StoreFileId = fileId,
                    SortIndex = _sortIndex,
                    Data = [],
                };
                uint _rowsSort = 0;
                foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow _row in _table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                {
                    uint _colsSort = 0;
                    foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell _cell in _row.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                    {
                        CellTableWordIndexFileModelDB _colDataDb = new()
                        {
                            ColNum = _colsSort,
                            RowNum = _rowsSort,
                            Data = _cell.InnerText,
                            StoreFileId = fileId,
                            TableWordFile = _tableDb,
                        };
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph[] paragraphs = [.. _cell.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()];
                        if (paragraphs.Length == 1)
                            _colDataDb.ParagraphId = paragraphs[0].ParagraphId;

                        _cellTablesDb.Add(_colDataDb);
                        _colsSort++;
                    }
                    _rowsSort++;
                }
                _tableDb.Data = _cellTablesDb;
                _tablesDb.Add(_tableDb);
            }
            _sortIndex++;
        }
        return (_paragraphs, _tablesDb);
    }
}