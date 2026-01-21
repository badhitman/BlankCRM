////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Reports.mmm;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.test;

/// <summary>
/// MainRetailReportComponent
/// </summary>
public partial class MainRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public required MMMWrapperComponent Owner { get; set; }


    MainReportResponseModel? ReportData;

    DateRange? _dateRange;
    DateRange? DateRangeProp
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            InvokeAsync(ReloadServerData);
        }
    }

    #region перевод в компанию на расчетный счет
    /// <summary>
    /// перевод в компанию на расчетный счет
    /// </summary>
    readonly StorageMetadataModel TransferToCompanyBankAccountStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "TransferToCompanyBankAccount",
    };
    readonly StorageMetadataModel TransferToCompanyBankAccountFootNoteStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "TransferToCompanyBankAccountFootNote",
    };

    string? _transferToCompanyBankAccountFootNote;
    string? TransferToCompanyBankAccountFootNote
    {
        get => _transferToCompanyBankAccountFootNote;
        set
        {
            _transferToCompanyBankAccountFootNote = value;
            InvokeAsync(SaveTransferToCompanyBankAccountFootNoteParameter);
        }
    }

    async void SaveTransferToCompanyBankAccountFootNoteParameter()
    {
        _ = await StorageTransmissionRepo.SaveParameterAsync<string?>(TransferToCompanyBankAccountFootNote, TransferToCompanyBankAccountFootNoteStorageMetadata, true);
        StateHasChanged();
    }

    decimal _transferToCompanyBankAccount = 0;
    decimal TransferToCompanyBankAccount
    {
        get => _transferToCompanyBankAccount;
        set
        {
            _transferToCompanyBankAccount = value;
            InvokeAsync(SaveTransferToCompanyBankAccountParameter);
        }
    }

    async void SaveTransferToCompanyBankAccountParameter()
    {
        if (Owner is null)
        {
            SnackBarRepo.Error($"Owner [{nameof(MMMWrapperComponent)}]: IS NULL");
            throw new ArgumentNullException(nameof(Owner));
        }

        _ = await StorageTransmissionRepo.SaveParameterAsync<decimal?>(TransferToCompanyBankAccount, TransferToCompanyBankAccountStorageMetadata, true);
        StateHasChanged();
    }
    #endregion

    #region пеервод в компанию
    /// <summary>
    /// перевод в компанию бонусов
    /// </summary>
    readonly StorageMetadataModel TransferBonusesCompanyStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "TransferBonusesCompany",
    };
    readonly StorageMetadataModel TransferBonusesCompanyFootNoteStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "TransferBonusesCompanyFootNote",
    };

    string? _transferBonusesCompanyFootNote;
    string? TransferBonusesCompanyFootNote
    {
        get => _transferBonusesCompanyFootNote;
        set
        {
            _transferBonusesCompanyFootNote = value;
            InvokeAsync(SaveTransferBonusesCompanyFootNoteParameter);
        }
    }

    async void SaveTransferBonusesCompanyFootNoteParameter()
    {
        _ = await StorageTransmissionRepo.SaveParameterAsync<string?>(TransferBonusesCompanyFootNote, TransferBonusesCompanyFootNoteStorageMetadata, true);
        StateHasChanged();
    }

    decimal _transferBonusesCompany = 0;
    decimal TransferBonusesCompany
    {
        get => _transferBonusesCompany;
        set
        {
            _transferBonusesCompany = value;
            InvokeAsync(SaveTransferBonusesCompanyParameter);
        }
    }

    async void SaveTransferBonusesCompanyParameter()
    {
        if (Owner is null)
        {
            SnackBarRepo.Error($"Owner [{nameof(MMMWrapperComponent)}]: IS NULL");
            throw new ArgumentNullException(nameof(Owner));
        }

        _ = await StorageTransmissionRepo.SaveParameterAsync<decimal?>(TransferBonusesCompany, TransferBonusesCompanyStorageMetadata, true);
        StateHasChanged();
    }
    #endregion

    #region Оплата наличными
    /// <summary>
    /// Оплата наличными
    /// </summary>
    readonly StorageMetadataModel CashPaymentStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "CashPayment",
    };
    readonly StorageMetadataModel CashPaymentFootNoteStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "CashPaymentFootNote",
    };

    string? _cashPaymentFootNote;
    string? CashPaymentFootNote
    {
        get => _cashPaymentFootNote;
        set
        {
            _cashPaymentFootNote = value;
            InvokeAsync(SaveCashPaymentFootNoteParameter);
        }
    }

    async void SaveCashPaymentFootNoteParameter()
    {
        _ = await StorageTransmissionRepo.SaveParameterAsync<string?>(CashPaymentFootNote, CashPaymentFootNoteStorageMetadata, true);
        StateHasChanged();
    }

    decimal _cashPayment = 0;
    decimal CashPayment
    {
        get => _cashPayment;
        set
        {
            _cashPayment = value;
            InvokeAsync(SaveCashPaymentParameter);
        }
    }

    async void SaveCashPaymentParameter()
    {
        if (Owner is null)
        {
            SnackBarRepo.Error($"Owner [{nameof(MMMWrapperComponent)}]: IS NULL");
            throw new ArgumentNullException(nameof(Owner));
        }

        _ = await StorageTransmissionRepo.SaveParameterAsync<decimal?>(CashPayment, CashPaymentStorageMetadata, true);
        StateHasChanged();
    }
    #endregion

    #region Долг
    /// <summary>
    /// Долг
    /// </summary>
    readonly StorageMetadataModel DebtStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "Debt",
    };
    readonly StorageMetadataModel DebtFootNoteStorageMetadata = new()
    {
        ApplicationName = "MMM",
        PropertyName = "DebtFootNote",
    };

    string? _debtFootNote;
    string? DebtFootNote
    {
        get => _debtFootNote;
        set
        {
            _debtFootNote = value;
            InvokeAsync(SaveDebtFootNoteParameter);
        }
    }

    async void SaveDebtFootNoteParameter()
    {
        _ = await StorageTransmissionRepo.SaveParameterAsync<string?>(DebtFootNote, DebtFootNoteStorageMetadata, true);
        StateHasChanged();
    }

    decimal _debt = 0;
    decimal Debt
    {
        get => _debt;
        set
        {
            _debt = value;
            InvokeAsync(SaveDebtParameter);
        }
    }

    async void SaveDebtParameter()
    {
        if (Owner is null)
        {
            SnackBarRepo.Error($"Owner [{nameof(MMMWrapperComponent)}]: IS NULL");
            throw new ArgumentNullException(nameof(Owner));
        }

        _ = await StorageTransmissionRepo.SaveParameterAsync<decimal?>(Debt, DebtStorageMetadata, true);
        StateHasChanged();
    }
    #endregion

    decimal _bonusAmount = 0;
    HtmlGenerator.html5.areas.div wrapDiv = new();
    async Task SaveReport()
    {
        wrapDiv.ClearNestedDom();
        if (ReportData is null)
        {
            SnackBarRepo.Error("ReportData is null");
            return;
        }

        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.h4($"Всего заказов выполнено [{ReportData.DoneOrdersCount} шт.] на сумму [{ReportData.DoneOrdersSumAmount:C}] ({Math.Round(ReportData.DoneOrdersSumAmount / 120, 2)})"));
        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p($"Год: {Owner.SelectedYear}; [неделя:{Owner.SelectedWeek?.NumWeekOfYear}] ({Owner.SelectedWeek?.Start.ToShortDateString()} - {Owner.SelectedWeek?.End.ToShortDateString()})"));
        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p($"Дата формирования: {DateTime.Now.ToLongDateString()} {DateTime.Now:T}"));
        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p($""));
        HtmlGenerator.html5.tables.table my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };

        my_table.TBody.AddRow(["Оплачено \"На сайте\"", $"{ReportData.PaidOnSitePaymentsSumAmount:C} [x {ReportData.PaidOnSitePaymentsCount} шт.] ({Math.Round((ReportData.PaidOnSitePaymentsSumAmount / 120), 2)})"]);
        my_table.TBody.AddRow(["Другое", $"{ReportData.PaidNoSitePaymentsSumAmount + ReportData.ConversionsSumAmount:C} [x {ReportData.PaidNoSitePaymentsCount + ReportData.ConversionsCount} шт.] ({Math.Round((ReportData.PaidNoSitePaymentsSumAmount + ReportData.ConversionsSumAmount) / 120, 2)})"]);
        my_table.TBody.AddRow(["&nbsp;&nbsp;&nbsp;&nbsp;Перевод/конвертация (поступило)", $"{ReportData.ConversionsSumAmount:C} [x {ReportData.ConversionsCount} шт.] ({Math.Round(ReportData.ConversionsSumAmount / 120, 2)})"]);
        my_table.TBody.AddRow(["&nbsp;&nbsp;&nbsp;&nbsp;Платежи", $"{ReportData.PaidNoSitePaymentsSumAmount:C} [x {ReportData.PaidNoSitePaymentsCount} шт.] ({Math.Round(ReportData.PaidNoSitePaymentsSumAmount / 120, 2)})"]);
        my_table.TBody.AddRow(["Итого:", $"{Math.Round(ReportData.PaidOnSitePaymentsSumAmount + ReportData.PaidNoSitePaymentsSumAmount + ReportData.ConversionsSumAmount, 2):C} [x {ReportData.PaidOnSitePaymentsCount + ReportData.PaidNoSitePaymentsSumAmount + ReportData.ConversionsSumAmount} шт.] ({Math.Round((ReportData.PaidOnSitePaymentsSumAmount + ReportData.PaidNoSitePaymentsSumAmount + ReportData.ConversionsSumAmount) / 120, 2)})"]);
        wrapDiv.AddDomNode(my_table);
        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p(""));

        my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };
        my_table.THead
            .AddColumn("#")
            .AddColumn("Value")
            .AddColumn("Note");

        my_table.TBody.AddRow(["Перевод в компанию на расчетный счет", $"{TransferToCompanyBankAccount}", $"{TransferToCompanyBankAccountFootNote}"]);
        my_table.TBody.AddRow(["Перевод в компанию бонусов", $"{TransferBonusesCompany} ({TransferBonusesCompany * 42})", $"{TransferBonusesCompanyFootNote}"]);
        my_table.TBody.AddRow(["Оплата наличными", $"{CashPayment}", $"{CashPaymentFootNote}"]);
        my_table.TBody.AddRow(["Долг", $"{Debt}", $"{DebtFootNote}"]);
        wrapDiv.AddDomNode(my_table);
        wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p(""));

        if (_bonusAmount > 0)
        {
            my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };

            my_table.TBody.AddRow(["Офисные за онлайн контракты", $"{Math.Round((decimal)(_bonusAmount * (decimal)0.1) * (ReportData.PaidOnSitePaymentsSumAmount * (decimal)0.1) / 120, 2)}"]);
            my_table.TBody.AddRow(["Офисные при оплате на складе (+переводы/конвертации)", $"{Math.Round(((decimal)(_bonusAmount * (decimal)0.1) * ((((ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount))) * (decimal)0.1) / 120), 2)}"]);
            my_table.TBody.AddRow(["Итого:", $"{Math.Round(((decimal)(_bonusAmount * (decimal)0.1) * (ReportData.PaidOnSitePaymentsSumAmount * (decimal)0.1) / 120) + (((decimal)(_bonusAmount * (decimal)0.1) * ((((ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount))) * (decimal)0.1) / 120)), 2)}"]);
            wrapDiv.AddDomNode(my_table);
            wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p(""));
            wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.h4("К оплате:"));

            my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };
            my_table.TBody.AddRow(["Сумма заявок:", $"{Math.Round((ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount), 2)}"]);
            my_table.TBody.AddRow(["минус [офисные при оплате на складе]", $"{((decimal)(_bonusAmount * (decimal)0.1) * ((Math.Round(ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount) * (decimal)0.1) / 120), 2)}"]);
            my_table.TBody.AddRow(["минус [офисные за онлайн контракты]", $"{(Math.Round((decimal)(_bonusAmount * (decimal)0.1) * (ReportData.PaidOnSitePaymentsSumAmount * (decimal)0.1) / 120), 2)}"]);
            my_table.TBody.AddRow(["минус [перевод в компанию бонусов]", $"{TransferBonusesCompany * 42}"]);
            my_table.TBody.AddRow(["минус [перевод в компанию на расчетный счет]", $"{TransferToCompanyBankAccount}"]);
            my_table.TBody.AddRow(["плюс [долг]", $"{Debt}"]);
            my_table.TBody.AddRow(["минус [оплата наличными]", $"{CashPayment}"]);
            my_table.TBody.AddRow(["Итого:", $"{Math.Round(((ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount) - (((decimal)(_bonusAmount * (decimal)0.1) * ((((ReportData.DoneOrdersSumAmount - ReportData.PaidOnSitePaymentsSumAmount))) * (decimal)0.1) / 120))) - ((decimal)(_bonusAmount * (decimal)0.1) * (ReportData.PaidOnSitePaymentsSumAmount * (decimal)0.1) / 120) - (TransferBonusesCompany * 42) - TransferToCompanyBankAccount + Debt - CashPayment, 2)}"]);

            wrapDiv.AddDomNode(my_table);
        }

        string test_s = $"<style>table, th, td {{border: 1px solid black;border-collapse: collapse;}}</style>{wrapDiv.GetHTML()}";

        using MemoryStream ms = new();
        StreamWriter writer = new(ms);
        writer.Write(test_s);
        writer.Flush();
        ms.Position = 0;

        using DotNetStreamReference streamRef = new(stream: ms);
        await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", $"Отчёт (сводный) - {DateTime.Now}.html", streamRef);

    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await SetBusyAsync();

        if (Owner?.SelectedWeek is not null)
            _dateRange = new()
            {
                Start = Owner.SelectedWeek.Value.Start,
                End = Owner.SelectedWeek.Value.End,
            };

        await ReloadServerData();
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public async Task ReloadServerData()
    {
        if (Owner.SelectedWeek is null)
        {
            SnackBarRepo.Error($"Owner.SelectedWeek: IS NULL");
            throw new ArgumentNullException(nameof(Owner));
        }

        _dateRange = new()
        {
            Start = Owner.SelectedWeek.Value.Start,
            End = Owner.SelectedWeek.Value.End,
        };


        string _prefix = $"y:{Owner.SelectedYear};w:{Owner.SelectedWeek.Value.NumWeekOfYear}";

        DebtStorageMetadata.PrefixPropertyName = _prefix;
        DebtFootNoteStorageMetadata.PrefixPropertyName = _prefix;

        CashPaymentStorageMetadata.PrefixPropertyName = _prefix;
        CashPaymentFootNoteStorageMetadata.PrefixPropertyName = _prefix;

        TransferBonusesCompanyFootNoteStorageMetadata.PrefixPropertyName = _prefix;
        TransferBonusesCompanyStorageMetadata.PrefixPropertyName = _prefix;

        TransferToCompanyBankAccountFootNoteStorageMetadata.PrefixPropertyName = _prefix;
        TransferToCompanyBankAccountStorageMetadata.PrefixPropertyName = _prefix;

        await SetBusyAsync();

        TResponseModel<decimal> getDecimal = await StorageTransmissionRepo.ReadParameterAsync<decimal>(GlobalStaticCloudStorageMetadata.BonusAmountStorageMetadata);
        _bonusAmount = getDecimal.Response;

        getDecimal = await StorageTransmissionRepo.ReadParameterAsync<decimal>(DebtStorageMetadata);
        _debt = getDecimal.Response;

        TResponseModel<string?> getString = await StorageTransmissionRepo.ReadParameterAsync<string>(DebtFootNoteStorageMetadata);
        _debtFootNote = getString.Response;


        getDecimal = await StorageTransmissionRepo.ReadParameterAsync<decimal>(CashPaymentStorageMetadata);
        _cashPayment = getDecimal.Response;

        getString = await StorageTransmissionRepo.ReadParameterAsync<string>(CashPaymentFootNoteStorageMetadata);
        _cashPaymentFootNote = getString.Response;

        getDecimal = await StorageTransmissionRepo.ReadParameterAsync<decimal>(TransferBonusesCompanyStorageMetadata);
        _transferBonusesCompany = getDecimal.Response;

        getString = await StorageTransmissionRepo.ReadParameterAsync<string>(TransferBonusesCompanyFootNoteStorageMetadata);
        _transferBonusesCompanyFootNote = getString.Response;

        getDecimal = await StorageTransmissionRepo.ReadParameterAsync<decimal>(TransferToCompanyBankAccountStorageMetadata);
        _transferToCompanyBankAccount = getDecimal.Response;

        getString = await StorageTransmissionRepo.ReadParameterAsync<string>(TransferToCompanyBankAccountFootNoteStorageMetadata);
        _transferToCompanyBankAccountFootNote = getString.Response;

        MainReportRequestModel req = new()
        {
            NumWeekOfYear = Owner.SelectedWeek.Value.NumWeekOfYear,
            SelectedYear = Owner.SelectedYear,
        };

        ReportData = await RetailRepo.GetMainReportAsync(req);
        await SetBusyAsync(false);
    }
}