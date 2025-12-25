////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Globalization;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.mmm;

/// <summary>
/// MMMWrapperComponent
/// </summary>
public partial class MMMWrapperComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    PeriodBaseModel? aboutPeriod;
    readonly List<int> Years = [];
    MMMReportsComponent? monthSelect_ref;
    readonly GregorianCalendar cal = new();
    readonly List<WeekMetadataModel> Weeklies = [];

    string GetCssForBtn(WeekMetadataModel _w)
    {
        string res = "me-1 mb-2 btn btn-";
        if (SelectedWeek.HasValue && SelectedWeek.Value.NumWeekOfYear == _w.NumWeekOfYear)
            res += "info";
        else
            res += "outline-secondary";

        if (_w.Start <= DateTime.Now && _w.End >= DateTime.Now)
            res += " border border-primary";

        return res;
    }

    int _selectedYear = DateTime.Now.Year;
    /// <summary>
    /// SelectedYear
    /// </summary>
    public int SelectedYear
    {
        get => _selectedYear;
        private set
        {
            _selectedYear = value;
            WeekliesUpdate();
            monthSelect_ref?.StateHasChangedCall();
        }
    }

    WeekMetadataModel? selectedWeek;
    /// <summary>
    /// SelectedWeek
    /// </summary>
    public WeekMetadataModel? SelectedWeek
    {
        get => selectedWeek;
        private set
        {
            selectedWeek = value;
            if (monthSelect_ref is not null)
                InvokeAsync(monthSelect_ref.Reload);
        }
    }

    void SelectWeek(WeekMetadataModel _w)
    {
        SelectedWeek = _w;
    }

    void WeekliesUpdate()
    {
        selectedWeek = null;
        Weeklies.Clear();
        DateTime dtSY = new(SelectedYear, 1, 1);
        int numWeekOfYear;
        numWeekOfYear = cal.GetWeekOfYear(dtSY, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Wednesday);
        while (numWeekOfYear < 1)
        {
            dtSY = dtSY.AddDays(1);
            numWeekOfYear = cal.GetWeekOfYear(dtSY, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Wednesday);
        }
        Weeklies.Add(new(numWeekOfYear, dtSY, EndOfDay(dtSY.AddDays(6))));
        dtSY = dtSY.AddDays(7);

        while (dtSY.Year == SelectedYear)
        {
            numWeekOfYear = cal.GetWeekOfYear(dtSY, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Wednesday);
            Weeklies.Add(new(numWeekOfYear, dtSY, EndOfDay(dtSY.AddDays(6))));
            dtSY = dtSY.AddDays(7);
        }
    }
    /// <summary>
    /// WeekMetadataModel
    /// </summary>
    public record struct WeekMetadataModel(int NumWeekOfYear, DateTime Start, DateTime End);

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        DateTime _cdt = DateTime.Now.Date;
        await SetBusyAsync();
        aboutPeriod = await RetailRepo.AboutPeriodAsync();
        if (aboutPeriod is not null)
        {
            if (aboutPeriod.Start is null || aboutPeriod.Start > _cdt)
                aboutPeriod.Start = _cdt;

            if (aboutPeriod.End is null || aboutPeriod.End < _cdt)
                aboutPeriod.End = _cdt;
        }
        int _y = aboutPeriod!.Start!.Value.Year;
        do
        {
            Years.Add(_y);
            _y++;
        }
        while (Years.Max() < aboutPeriod!.End!.Value.Year);
        WeekliesUpdate();
        int numWeekOfYear = cal.GetWeekOfYear(_cdt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Wednesday);
        DayOfWeek dayOfWeek = _cdt.DayOfWeek;
        while (dayOfWeek > DayOfWeek.Wednesday)
        {
            _cdt = _cdt.AddDays(-1);
            dayOfWeek = _cdt.DayOfWeek;
        }
        selectedWeek = new(numWeekOfYear, _cdt, EndOfDay(_cdt.AddDays(7)));
        await SetBusyAsync(false);
    }

    static DateTime EndOfDay(DateTime sender)
    {
        sender = sender.Date;
        sender = sender.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        return sender;
    }
}