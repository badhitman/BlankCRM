////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Kladr;

/// <summary>
/// KladrServerSideLoadComponent
/// </summary>
public partial class KladrServerSideLoadComponent
{
    [Inject]
    IKladrParseService Parser { get; set; } = default!;

    string[] _validateList = ["ALTNAMES.DBF", "DOMA.DBF", "FLAT.DBF", "KLADR.DBF", "NAMEMAP.DBF", "SOCRBASE.DBF", "STREET.DBF"];
    bool IsValidList => allFiles is not null && allCropFiles is not null && allFiles.All(x => _validateList.Contains(Path.GetFileName(x))) && _validateList.All(x => allCropFiles.Contains(x));
    string? dirData;
    string[]? allFiles, allCropFiles;
    List<string> uploadedFiles = [];
    bool _isLoading;
    int totalRecordsParts;
    string? currentFilePart;
    // string currentEncoding = "cp866";

    void ScanDirectory()
    {
        if (string.IsNullOrWhiteSpace(dirData) || !Directory.Exists(dirData))
            return;

        allFiles = Directory.GetFiles(dirData);
        allCropFiles = [.. allFiles.Select(x => Path.GetFileName(x))];
    }

    async Task Load()
    {
        _isLoading = true;
        uploadedFiles.Clear();
        // Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        StateHasChanged();

        Parser.PartUploadNotify += PartUploadNotifyHandle;
        foreach (string _fn in allFiles!)
        {
            currentFilePart = _fn;
            using MemoryStream ms = new(File.ReadAllBytes(_fn));
            totalRecordsParts = await Parser.Open(ms, Path.GetFileName(_fn));
            while (Parser.CurrentNumRecord < totalRecordsParts)
            {
                await Parser.UploadData(true, 10);
                StateHasChanged();
            }
        }

        Parser.PartUploadNotify -= PartUploadNotifyHandle;
        _isLoading = false;
        totalRecordsParts = 0;
        StateHasChanged();
    }

    private void PartUploadNotifyHandle(int recordNum)
    {
        InvokeAsync(StateHasChanged);
    }
}