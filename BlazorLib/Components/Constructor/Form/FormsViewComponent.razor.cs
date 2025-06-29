﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Form;

/// <summary>
/// Forms view
/// </summary>
public partial class FormsViewComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDialogService DialogServiceRepo { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Родительская страница форм
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    /// <summary>
    /// имя типа данных: формы
    /// </summary>
    const string type_name_form_of_tab = nameof(FormConstructorModelDB);

    /// <summary>
    /// Таблица
    /// </summary>
    protected MudTable<FormConstructorModelDB>? table;

    /// <summary>
    /// Строка поиска
    /// </summary>
    protected string? searchString = null;

    TPaginationResponseModel<FormConstructorModelDB>? rest_data;

    /// <summary>
    /// Открыть форму
    /// </summary>
    protected async Task OpenForm(FormConstructorModelDB form)
    {
        await SetBusyAsync();
        TResponseModel<FormConstructorModelDB> rest = await ConstructorRepo.GetFormAsync(form.Id);
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackBarRepo.Error($"Ошибка BDED4783-A604-4347-A344-1B66064CDDE8 Action: {rest.Message()}");
            return;
        }
        StateHasChanged();
        DialogParameters<EditFormDialogComponent> parameters = new()
        {
            { x => x.Form, rest.Response },
            { x => x.ParentFormsPage, ParentFormsPage }
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<EditFormDialogComponent>($"Редактирование формы #{rest.Response?.Id}", parameters, options);

        if (table is not null)
            await table.ReloadServerData();
    }

    ///// <inheritdoc/>
    //protected override async Task OnInitializedAsync()
    //{
    //    await RestJson();
    //}


    TableState? _table_state;
    async Task RestJson()
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("Проект не выбран.");

        await SetBusyAsync();

        rest_data = await ConstructorRepo.SelectFormsAsync(new() { Request = SimplePaginationRequestModel.Build(searchString, _table_state?.PageSize ?? 10, _table_state?.Page ?? 0), ProjectId = ParentFormsPage.MainProject.Id });
        IsBusyProgress = false;
    }

    /// <summary>
    /// Открыть диалог создания формы
    /// </summary>
    protected async Task OpenDialogCreateForm()
    {
        if (ParentFormsPage.MainProject is null)
        {
            SnackBarRepo.Error("Не выбран основной/текущий проект");
            return;
        }

        DialogParameters<EditFormDialogComponent> parameters = new()
        {
            { x => x.Form, FormConstructorModelDB.BuildEmpty(ParentFormsPage.MainProject.Id) },
            { x => x.ParentFormsPage, ParentFormsPage }
        };
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference result = await DialogServiceRepo.ShowAsync<EditFormDialogComponent>("Создание новой формы", parameters, options);

        if (table is not null)
            await table.ReloadServerData();
    }

    /// <summary>
    /// Загрузка данных форм
    /// </summary>
    protected async Task<TableData<FormConstructorModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        bool _init = rest_data is null;
        if (_init)
            await RestJson();

        _table_state = state;

        if (!_init)
            await RestJson();

        return new TableData<FormConstructorModelDB>() { TotalItems = rest_data!.TotalRowsCount, Items = rest_data.Response };
    }

    /// <inheritdoc/>
    protected private async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}