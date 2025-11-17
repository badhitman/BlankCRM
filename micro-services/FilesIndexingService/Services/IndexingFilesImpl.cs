////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using MongoDB.Bson;
using SharedLib;
using DbcLib;

namespace FileIndexingService;

/// <summary>
/// Индексирование файлов приложений IMongoDatabase mongoFs = mongoCli.GetDatabase("");
/// </summary>
public class IndexingFilesImpl(
    IDbContextFactory<FilesIndexingContext> fileIndexingDbFactory,
    IMongoDatabase mongoFs) : IFilesIndexing
{
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

    async Task<ResponseBaseModel> SpreadsheetDocumentHandle(StorageFileMiddleModel file_db, CancellationToken token = default)
    {
        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);

        if (await context.SheetsExcelIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("the file is already indexed");

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

        List<string> sharedStrings = [];
        SharedStringTablePart? sharedStringTablePart = workbookPart.SharedStringTablePart;

        if (sharedStringTablePart is not null)
        {
            foreach (var item in sharedStringTablePart.SharedStringTable.Elements<DocumentFormat.OpenXml.Spreadsheet.SharedStringItem>())
                sharedStrings.Add(item.InnerText);
        }

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        List<SheetExcelIndexFileModelDB> sheetsDb = [];
        foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
        {
            string partRelationshipId = workbookPart.GetIdOfPart(worksheetPart);
            DocumentFormat.OpenXml.Spreadsheet.Sheet correspondingSheet = sheets.Cast<DocumentFormat.OpenXml.Spreadsheet.Sheet>().First(s => s.Id == partRelationshipId);
            SheetExcelIndexFileModelDB _newSheetDb = new()
            {
                Title = correspondingSheet.Name?.Value ?? "-no name-",
                Cells = [],
                StoreFileId = file_db.Id,
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
                                StoreFileId = file_db.Id,
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

        if (sheetsDb.Count != 0)
        {
            await context.AddRangeAsync(sheetsDb, token);
            await context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    async Task<ResponseBaseModel> WordprocessingDocumentHandle(StorageFileMiddleModel file_db, CancellationToken token = default)
    {
        FilesIndexingContext context = await fileIndexingDbFactory.CreateDbContextAsync(token);

        if (await context.TablesWordIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token) || await context.ParagraphsWordIndexesFiles.AnyAsync(x => x.StoreFileId == file_db.Id, cancellationToken: token))
            return ResponseBaseModel.CreateInfo("the file is already indexed");

        using MemoryStream stream = new();
        GridFSBucket gridFS = new(mongoFs);
        await gridFS.DownloadToStreamAsync(new ObjectId(file_db.PointId), stream, cancellationToken: token);

        using WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(stream, false);
        if (wordprocessingDocument.MainDocumentPart?.Document.Body is null)
            return ResponseBaseModel.CreateError("word-processing document [MainDocumentPart?.Document.Body] is null");

        DocumentFormat.OpenXml.Wordprocessing.Document document = wordprocessingDocument.MainDocumentPart.Document;
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<ParagraphWordIndexFileModelDB> _paragraphs = [];
        List<TableWordIndexFileModelDB> _tablesDb = [];
        int _sortIndex = 0;
        foreach (DocumentFormat.OpenXml.OpenXmlElement element in document.Body.Elements())
        {
            if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph _paragraph)
            {
                DocumentFormat.OpenXml.HexBinaryValue? _paragraphId = _paragraph.ParagraphId;
                ParagraphWordIndexFileModelDB _par = new()
                {
                    StoreFileId = file_db.Id,
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
                    StoreFileId = file_db.Id,
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
                            StoreFileId = file_db.Id,
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

        if (_tablesDb.Count != 0)
            await context.AddRangeAsync(_tablesDb, token);

        if (_paragraphs.Count != 0)
            await context.AddRangeAsync(_paragraphs, token);

        if (_tablesDb.Count != 0 || _paragraphs.Count != 0)
        {
            await context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}