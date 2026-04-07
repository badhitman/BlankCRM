////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// NoteSimpleComponent
/// </summary>
public partial class NoteSimpleComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    /// <summary>
    /// Label
    /// </summary>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <summary>
    /// Hint
    /// </summary>
    [Parameter, EditorRequired]
    public required string Hint { get; set; }
    /// <summary>
    /// ColorTheme
    /// </summary>
    [Parameter]
    public BootstrapColorsStylesEnum ColorTheme { get; set; } = BootstrapColorsStylesEnum.Secondary;

    /// <summary>
    /// Имя приложения, которое обращается к службе облачного хранения параметров
    /// </summary>
    [Parameter, EditorRequired]
    public required string ApplicationName { get; set; }

    /// <summary>
    /// Имя параметра
    /// </summary>
    [Parameter, EditorRequired]
    public required string PropertyName { get; set; }

    /// <summary>
    /// Префикс имени (опционально)
    /// </summary>
    [Parameter]
    public string? PrefixPropertyName { get; set; }

    /// <summary>
    /// Связанный PK строки базы данных (опционально)
    /// </summary>
    [Parameter]
    public int? OwnerPrimaryKey { get; set; }


    StorageMetadataModel KeyStorage => new()
    {
        ApplicationName = ApplicationName,
        PropertyName = PropertyName,
        OwnerPrimaryKey = OwnerPrimaryKey,
        PrefixPropertyName = PrefixPropertyName,
    };

    string domId = default!;

    string? initValue;
    string? editValue;

    async Task SaveText()
    {
        await SetBusyAsync();
        TResponseModel<int> rest = await StorageRepo.SaveParameterAsync(editValue, KeyStorage, false);
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        initValue = editValue;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        domId = $"{OwnerPrimaryKey}/{nameof(NoteSimpleComponent)}{ApplicationName}{PropertyName}{PrefixPropertyName}";
        await SetBusyAsync();
        TResponseModel<string?> rest = await StorageRepo.ReadParameterAsync<string?>(KeyStorage);

        initValue = rest.Response;
        editValue = initValue;

        await SetBusyAsync(false);
    }
}