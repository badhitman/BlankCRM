////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// KladrUploadComponent
/// </summary>
public partial class KladrUploadComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IClientHTTPRestService RemoteClient { get; set; } = default!;


    private string _inputFileId = Guid.NewGuid().ToString();
    private readonly List<IBrowserFile> loadedFiles = [];

    private readonly List<(KladrFileViewComponent ParentComponent, string FileName)> ViewsChilds = [];

    string? aboutStatus, aboutSubStatus;

    bool CanSend
    {
        get
        {
            return loadedFiles.All(x => KladrFileViewComponent.KladrFiles.Any(y => $"{y}.dbf".Equals(x.Name, StringComparison.OrdinalIgnoreCase))) &&
                KladrFileViewComponent.KladrFiles.All(x => loadedFiles.Any(y => y.Name.Equals($"{x}.dbf", StringComparison.OrdinalIgnoreCase)));
        }
    }

    string _value = "cp866";
    string SelectedEncoding
    {
        get => _value;
        set
        {
            _value = value;

            InvokeAsync(async () => await SetBusy());
            InvokeAsync(async () => await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.ParentComponent.SeedDemo(_value)))));
            InvokeAsync(async () => await SetBusy(false));
        }
    }


    int ChildsVotesBusyBalance = 0;
    /// <inheritdoc/>
    public void ChildBusyVote(int voteVal)
    {
        Interlocked.Add(ref ChildsVotesBusyBalance, voteVal);
        if (voteVal < 0 && ChildsVotesBusyBalance == 0)
        {
            if (loadedFiles.Count != 0)
            {
                aboutStatus = "Файлы подготовлены";
                aboutSubStatus = "можно отправлять данные в БД";
            }
        }

        StateHasChangedCall();
    }

    /// <inheritdoc/>
    public override bool IsBusyProgress
    {
        get => ChildsVotesBusyBalance != 0 || base.IsBusyProgress;
        set => base.IsBusyProgress = value;
    }

    /// <inheritdoc/>
    public void InitHandleAction((KladrFileViewComponent ParentComponent, string FileName) sender)
    {
        ViewsChilds.Add(sender);
        StateHasChangedCall();
    }

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        aboutStatus = "Выбранные файлы инициализируются";
        aboutSubStatus = "дождитесь окончания...";

        loadedFiles.Clear();
        ViewsChilds.Clear();

        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

    async Task TransferData()
    {
        if (loadedFiles.Count == 0)
            throw new Exception();

        aboutStatus = "Отправка данных на сервер";
        aboutSubStatus = "данные записываются в удалённую базу данных";

        await SetBusy();
        await RemoteClient.ClearTempKladrAsync();
        await Task.WhenAll(ViewsChilds.Select(x => Task.Run(async () => await x.ParentComponent.UploadData())));

        aboutStatus = "Данные отправлены на сервер";
        aboutSubStatus = "теперь вы можете обновить основную БД";

        loadedFiles.Clear();
        _inputFileId = Guid.NewGuid().ToString();
        await SetBusy(false);
    }
}