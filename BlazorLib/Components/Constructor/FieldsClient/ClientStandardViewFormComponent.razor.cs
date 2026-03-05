////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.FieldsClient;

/// <summary>
/// Client standard view form
/// </summary>
public partial class ClientStandardViewFormComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public string? Title { get; set; }


    /// <summary>
    /// Номер строки таблицы данных (0 - если форма обычная, а не не таблица/многострочная)
    /// </summary>
    [Parameter]
    public uint RowNum { get; set; } = 0;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public FormConstructorModelDB Form { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public SessionOfDocumentDataModelDB? SessionOfDocumentData { get; set; }


    /// <summary>
    /// Доступ к перечню полей формы. Каждое поле формы добавляет себя к этому перечню при инициализации (в <c>OnInitialized()</c>) базового <cref name="FieldBaseClientComponent">компонента</cref>
    /// </summary>
    protected List<FieldComponentBaseModel?> FieldsReferring = [];

    /// <inheritdoc/>
    protected IEnumerable<EntryNestedModel> Directories = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await Update();
    }

    /// <inheritdoc/>
    public async Task Update(FormConstructorModelDB? form = null)
    {
        if (form is not null)
            Form.Reload(form);

        if (Form.FieldsDirectoriesLinks is not null && Form.FieldsDirectoriesLinks.Count != 0)
        {
            await SetBusyAsync();

            Directories = await ConstructorRepo.ReadDirectoriesAsync([.. Form.FieldsDirectoriesLinks.Select(x => x.DirectoryId).Distinct()]);
            await SetBusyAsync(false);
        }
    }
}