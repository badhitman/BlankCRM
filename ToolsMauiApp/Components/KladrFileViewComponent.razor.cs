////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToolsMauiLib;
using System.Text;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <inheritdoc/>
public partial class KladrFileViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ParseDBF Parser { get; set; } = default!;


    /// <summary>
    /// OwnerComponent
    /// </summary>
    [Parameter, EditorRequired]
    public required IBrowserFile FileViewElement { get; set; }

    /// <summary>
    /// InitHandle
    /// </summary>
    [Parameter, EditorRequired]
    public required KladrUploadComponent OwnerComponent { get; set; }


    /// <inheritdoc/>
    public static readonly string[] KladrFiles = [.. Enum.GetNames<KladrFilesEnum>()];


    int NumRecordsTotal, numRecordProgress;

    DateTime? startedProgress = null;
    DateTime? endedProgress = null;
    string _stateInfo = "";

    (List<object[]> TableData, FieldDescriptorBase[] Columns)? DemoTable;

    MemoryStream ms = default!;
    string currentEncoding = "cp866";

    double CurrentPercentage
    {
        get
        {
            double _nrt = NumRecordsTotal;
            double percentageVal = NumRecordsTotal == 0 ? 0 : (numRecordProgress / (_nrt / 100));

            return percentageVal > 100 ? 100 : percentageVal;
        }
    }

    /// <inheritdoc/>
    public async Task SeedDemo(string enc = "cp866")
    {
        currentEncoding = enc;
        Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);

        if (!IsBusyProgress)
            await SetBusy();

        DemoTable = await Parser.GetRandomRowsAsDataTable(5);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!KladrFiles.Any(x => $"{x}.dbf".Equals(FileViewElement.Name, StringComparison.OrdinalIgnoreCase)))
            return;

        OwnerComponent.ChildBusyVote(1);
        OwnerComponent.InitHandleAction((this, FileViewElement.Name));

        await SetBusy();
        ms = new();
        await FileViewElement.OpenReadStream(long.MaxValue).CopyToAsync(ms);
        NumRecordsTotal = await Parser.Open(ms, FileViewElement.Name);
        await SeedDemo();
        OwnerComponent.ChildBusyVote(-1);
    }

    /// <inheritdoc/>
    public async Task UploadData()
    {
        startedProgress = DateTime.Now;
        OwnerComponent.ChildBusyVote(1);
        numRecordProgress = 1;
        await SetBusy();
        Parser.CurrentEncoding = Encoding.GetEncoding(currentEncoding);
        Parser.PartUploadNotify += ParserPartUploadNotify;
        await Parser.UploadData(true);
        Parser.PartUploadNotify -= ParserPartUploadNotify;
        endedProgress = DateTime.Now;
        await SetBusy(false);
        OwnerComponent.ChildBusyVote(-1);
    }

    private void ParserPartUploadNotify(int recordNum)
    {
        numRecordProgress = recordNum;

        if (startedProgress is null)
            _stateInfo = string.Empty;
        else if (endedProgress is not null)
            _stateInfo = $"Завершено за: {endedProgress - startedProgress}";
        else
        {
            TimeSpan currentDuration = DateTime.Now - startedProgress.Value;
            double percentageVal = CurrentPercentage;
            if (percentageVal == 0)
                _stateInfo = "Начинаем ...";
            else if (percentageVal >= 100)
                _stateInfo = $"Выполнено за {currentDuration:hh\\:mm\\:ss}";
            else
            {
                TimeSpan calcFinal = TimeSpan.FromMilliseconds(currentDuration.TotalMilliseconds / percentageVal * 100);
                _stateInfo = $" время прошло {currentDuration:hh\\:mm\\:ss} из {calcFinal:hh\\:mm\\:ss} - осталось {calcFinal - currentDuration:hh\\:mm\\:ss}";
                _stateInfo = $" время прошло {currentDuration:hh\\:mm\\:ss} из {calcFinal:hh\\:mm\\:ss} - осталось {calcFinal - currentDuration:hh\\:mm\\:ss}";
            }
        }
        InvokeAsync(StateHasChanged);
    }
}