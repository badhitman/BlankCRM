////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib.CommerceML2;

/// <remarks/>
public enum ОбязательностьСвойствТип
{
    /// <remarks/>
    [Description("Для каталога")]
    ДляКаталога,

    /// <remarks/>
    [Description("Для документа")]
    ДляДокумента,

    /// <remarks/>
    [Description("Для предложений")]
    ДляПредложений,
}