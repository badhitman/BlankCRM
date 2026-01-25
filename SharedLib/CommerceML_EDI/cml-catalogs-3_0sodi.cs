/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ИННРФ))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GLN))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(КонтрагентИД))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("ИдОтправителя", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public abstract partial class ИдентификаторКонтрагента {
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class СтрокаТовараКРаботе {
    
    private ИдентификаторТовара идТовараКлиентаField;
    
    private ИдентификаторТовара[] идТовараПоставщикаField;
    
    private string наименованиеField;
    
    private decimal коэффициентПересчетаField;
    
    private bool коэффициентПересчетаFieldSpecified;
    
    private string примечаниеField;
    
    private System.Xml.XmlElement[] anyField;
    
    /// <remarks/>
    public ИдентификаторТовара ИдТовараКлиента {
        get {
            return this.идТовараКлиентаField;
        }
        set {
            this.идТовараКлиентаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовараПоставщика")]
    public ИдентификаторТовара[] ИдТовараПоставщика {
        get {
            return this.идТовараПоставщикаField;
        }
        set {
            this.идТовараПоставщикаField = value;
        }
    }
    
    /// <remarks/>
    public string Наименование {
        get {
            return this.наименованиеField;
        }
        set {
            this.наименованиеField = value;
        }
    }
    
    /// <remarks/>
    public decimal КоэффициентПересчета {
        get {
            return this.коэффициентПересчетаField;
        }
        set {
            this.коэффициентПересчетаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool КоэффициентПересчетаSpecified {
        get {
            return this.коэффициентПересчетаFieldSpecified;
        }
        set {
            this.коэффициентПересчетаFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string Примечание {
        get {
            return this.примечаниеField;
        }
        set {
            this.примечаниеField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.XmlElement[] Any {
        get {
            return this.anyField;
        }
        set {
            this.anyField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GTIN))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ТоварИД))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("ИдТовара", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public abstract partial class ИдентификаторТовара {
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class GTIN : ИдентификаторТовара {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class ТоварИД : ИдентификаторТовара {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class СтрокаКаталога {
    
    private ИдентификаторТовара[] идТовараПоставщикаField;
    
    private ИдентификаторТовара[] идТовараКлиентаField;
    
    private ИдентификаторТовара[] штриховойКодField;
    
    private ОКЕИ оКЕИField;
    
    private ОКП оКПField;
    
    private ИСО3166 иСО3166Field;
    
    private ОКВЭД оКВЭДField;
    
    private КлассификаторТип[] классификаторТовараField;
    
    private string наименованиеField;
    
    private string торговаяМаркаField;
    
    private string производительField;
    
    private string описаниеField;
    
    private decimal весНеттоField;
    
    private bool весНеттоFieldSpecified;
    
    private decimal весБруттоField;
    
    private bool весБруттоFieldSpecified;
    
    private decimal высотаСлояТовараField;
    
    private bool высотаСлояТовараFieldSpecified;
    
    private decimal высотаТовараField;
    
    private bool высотаТовараFieldSpecified;
    
    private decimal ширинаТовараField;
    
    private bool ширинаТовараFieldSpecified;
    
    private decimal глубинаТовараField;
    
    private bool глубинаТовараFieldSpecified;
    
    private decimal объемТовараField;
    
    private bool объемТовараFieldSpecified;
    
    private decimal минКоличествоДляЗаказаField;
    
    private bool минКоличествоДляЗаказаFieldSpecified;
    
    private decimal количествоВСлоеНаЕвропалетеField;
    
    private bool количествоВСлоеНаЕвропалетеFieldSpecified;
    
    private string срокХраненияField;
    
    private string температураХраненияField;
    
    private decimal кратностьField;
    
    private bool кратностьFieldSpecified;
    
    private decimal количесвоЕдиницОбъектаВерхнегоУровняField;
    
    private bool количесвоЕдиницОбъектаВерхнегоУровняFieldSpecified;
    
    private string примечаниеField;
    
    private System.Xml.XmlElement[] anyField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовараПоставщика")]
    public ИдентификаторТовара[] ИдТовараПоставщика {
        get {
            return this.идТовараПоставщикаField;
        }
        set {
            this.идТовараПоставщикаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовараКлиента")]
    public ИдентификаторТовара[] ИдТовараКлиента {
        get {
            return this.идТовараКлиентаField;
        }
        set {
            this.идТовараКлиентаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ШтриховойКод")]
    public ИдентификаторТовара[] ШтриховойКод {
        get {
            return this.штриховойКодField;
        }
        set {
            this.штриховойКодField = value;
        }
    }
    
    /// <remarks/>
    public ОКЕИ ОКЕИ {
        get {
            return this.оКЕИField;
        }
        set {
            this.оКЕИField = value;
        }
    }
    
    /// <remarks/>
    public ОКП ОКП {
        get {
            return this.оКПField;
        }
        set {
            this.оКПField = value;
        }
    }
    
    /// <remarks/>
    public ИСО3166 ИСО3166 {
        get {
            return this.иСО3166Field;
        }
        set {
            this.иСО3166Field = value;
        }
    }
    
    /// <remarks/>
    public ОКВЭД ОКВЭД {
        get {
            return this.оКВЭДField;
        }
        set {
            this.оКВЭДField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("КлассификаторТовара")]
    public КлассификаторТип[] КлассификаторТовара {
        get {
            return this.классификаторТовараField;
        }
        set {
            this.классификаторТовараField = value;
        }
    }
    
    /// <remarks/>
    public string Наименование {
        get {
            return this.наименованиеField;
        }
        set {
            this.наименованиеField = value;
        }
    }
    
    /// <remarks/>
    public string ТорговаяМарка {
        get {
            return this.торговаяМаркаField;
        }
        set {
            this.торговаяМаркаField = value;
        }
    }
    
    /// <remarks/>
    public string Производитель {
        get {
            return this.производительField;
        }
        set {
            this.производительField = value;
        }
    }
    
    /// <remarks/>
    public string Описание {
        get {
            return this.описаниеField;
        }
        set {
            this.описаниеField = value;
        }
    }
    
    /// <remarks/>
    public decimal ВесНетто {
        get {
            return this.весНеттоField;
        }
        set {
            this.весНеттоField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ВесНеттоSpecified {
        get {
            return this.весНеттоFieldSpecified;
        }
        set {
            this.весНеттоFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ВесБрутто {
        get {
            return this.весБруттоField;
        }
        set {
            this.весБруттоField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ВесБруттоSpecified {
        get {
            return this.весБруттоFieldSpecified;
        }
        set {
            this.весБруттоFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ВысотаСлояТовара {
        get {
            return this.высотаСлояТовараField;
        }
        set {
            this.высотаСлояТовараField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ВысотаСлояТовараSpecified {
        get {
            return this.высотаСлояТовараFieldSpecified;
        }
        set {
            this.высотаСлояТовараFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ВысотаТовара {
        get {
            return this.высотаТовараField;
        }
        set {
            this.высотаТовараField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ВысотаТовараSpecified {
        get {
            return this.высотаТовараFieldSpecified;
        }
        set {
            this.высотаТовараFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ШиринаТовара {
        get {
            return this.ширинаТовараField;
        }
        set {
            this.ширинаТовараField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ШиринаТовараSpecified {
        get {
            return this.ширинаТовараFieldSpecified;
        }
        set {
            this.ширинаТовараFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ГлубинаТовара {
        get {
            return this.глубинаТовараField;
        }
        set {
            this.глубинаТовараField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ГлубинаТовараSpecified {
        get {
            return this.глубинаТовараFieldSpecified;
        }
        set {
            this.глубинаТовараFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal ОбъемТовара {
        get {
            return this.объемТовараField;
        }
        set {
            this.объемТовараField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ОбъемТовараSpecified {
        get {
            return this.объемТовараFieldSpecified;
        }
        set {
            this.объемТовараFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal МинКоличествоДляЗаказа {
        get {
            return this.минКоличествоДляЗаказаField;
        }
        set {
            this.минКоличествоДляЗаказаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool МинКоличествоДляЗаказаSpecified {
        get {
            return this.минКоличествоДляЗаказаFieldSpecified;
        }
        set {
            this.минКоличествоДляЗаказаFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal КоличествоВСлоеНаЕвропалете {
        get {
            return this.количествоВСлоеНаЕвропалетеField;
        }
        set {
            this.количествоВСлоеНаЕвропалетеField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool КоличествоВСлоеНаЕвропалетеSpecified {
        get {
            return this.количествоВСлоеНаЕвропалетеFieldSpecified;
        }
        set {
            this.количествоВСлоеНаЕвропалетеFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="duration")]
    public string СрокХранения {
        get {
            return this.срокХраненияField;
        }
        set {
            this.срокХраненияField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
    public string ТемператураХранения {
        get {
            return this.температураХраненияField;
        }
        set {
            this.температураХраненияField = value;
        }
    }
    
    /// <remarks/>
    public decimal Кратность {
        get {
            return this.кратностьField;
        }
        set {
            this.кратностьField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool КратностьSpecified {
        get {
            return this.кратностьFieldSpecified;
        }
        set {
            this.кратностьFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public decimal КоличесвоЕдиницОбъектаВерхнегоУровня {
        get {
            return this.количесвоЕдиницОбъектаВерхнегоУровняField;
        }
        set {
            this.количесвоЕдиницОбъектаВерхнегоУровняField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool КоличесвоЕдиницОбъектаВерхнегоУровняSpecified {
        get {
            return this.количесвоЕдиницОбъектаВерхнегоУровняFieldSpecified;
        }
        set {
            this.количесвоЕдиницОбъектаВерхнегоУровняFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string Примечание {
        get {
            return this.примечаниеField;
        }
        set {
            this.примечаниеField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.XmlElement[] Any {
        get {
            return this.anyField;
        }
        set {
            this.anyField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ОКЕИ : КлассификаторТип {
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ИСО3166))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКВЭД))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКП))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКЕИ))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public abstract partial class КлассификаторТип {
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ОКП : КлассификаторТип {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ИСО3166 : КлассификаторТип {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ОКВЭД : КлассификаторТип {
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ТоварКРаботе))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(КаталогТоваров))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ЗапросКаталога))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public abstract partial class КоммерческийДокумент {
    
    private ИдентификаторКонтрагента идОтправителяField;
    
    private ИдентификаторКонтрагента идПолучателяField;
    
    private ИдентификаторДокумента номерДокументаField;
    
    private System.DateTime моментСозданияField;
    
    private string примечаниеField;
    
    private System.Xml.XmlAttribute[] anyAttrField;
    
    /// <remarks/>
    public ИдентификаторКонтрагента ИдОтправителя {
        get {
            return this.идОтправителяField;
        }
        set {
            this.идОтправителяField = value;
        }
    }
    
    /// <remarks/>
    public ИдентификаторКонтрагента ИдПолучателя {
        get {
            return this.идПолучателяField;
        }
        set {
            this.идПолучателяField = value;
        }
    }
    
    /// <remarks/>
    public ИдентификаторДокумента НомерДокумента {
        get {
            return this.номерДокументаField;
        }
        set {
            this.номерДокументаField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime МоментСоздания {
        get {
            return this.моментСозданияField;
        }
        set {
            this.моментСозданияField = value;
        }
    }
    
    /// <remarks/>
    public string Примечание {
        get {
            return this.примечаниеField;
        }
        set {
            this.примечаниеField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAnyAttributeAttribute()]
    public System.Xml.XmlAttribute[] AnyAttr {
        get {
            return this.anyAttrField;
        }
        set {
            this.anyAttrField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ДокументИД))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("НомерДокумента", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public abstract partial class ИдентификаторДокумента {
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute(DataType="normalizedString")]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class ДокументИД : ИдентификаторДокумента {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class ИННРФ : ИдентификаторКонтрагента {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class GLN : ИдентификаторКонтрагента {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
public partial class КонтрагентИД : ИдентификаторКонтрагента {
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("Стоимость", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class СтоимостьТип {
    
    private СуммаТип суммаField;
    
    private СтоимостьНалогТип[] налогField;
    
    /// <remarks/>
    public СуммаТип Сумма {
        get {
            return this.суммаField;
        }
        set {
            this.суммаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Налог")]
    public СтоимостьНалогТип[] Налог {
        get {
            return this.налогField;
        }
        set {
            this.налогField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("Сумма", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class СуммаТип {
    
    private string валютаField;
    
    private decimal valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string Валюта {
        get {
            return this.валютаField;
        }
        set {
            this.валютаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public decimal Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute("Налог", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class СтоимостьНалогТип {
    
    private ТипНалога типНалогаField;
    
    private decimal величинаСтавкиНалогаField;
    
    private СуммаТип суммаField;
    
    private bool включеноВСтоимостьField;
    
    /// <remarks/>
    public ТипНалога ТипНалога {
        get {
            return this.типНалогаField;
        }
        set {
            this.типНалогаField = value;
        }
    }
    
    /// <remarks/>
    public decimal ВеличинаСтавкиНалога {
        get {
            return this.величинаСтавкиНалогаField;
        }
        set {
            this.величинаСтавкиНалогаField = value;
        }
    }
    
    /// <remarks/>
    public СуммаТип Сумма {
        get {
            return this.суммаField;
        }
        set {
            this.суммаField = value;
        }
    }
    
    /// <remarks/>
    public bool ВключеноВСтоимость {
        get {
            return this.включеноВСтоимостьField;
        }
        set {
            this.включеноВСтоимостьField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public enum ТипНалога {
    
    /// <remarks/>
    НДС,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ЗапросКаталога : КоммерческийДокумент {
    
    private string длительностьОжиданияОтветаField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="duration")]
    public string ДлительностьОжиданияОтвета {
        get {
            return this.длительностьОжиданияОтветаField;
        }
        set {
            this.длительностьОжиданияОтветаField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class КаталогТоваров : КоммерческийДокумент {
    
    private ИдентификаторДокумента номерИсходногоДокументаField;
    
    private string длительностьОжиданияОтветаField;
    
    private bool этоПолныйКаталогField;
    
    private bool этоПолныйКаталогFieldSpecified;
    
    private СтрокаКаталога[] товарField;
    
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента {
        get {
            return this.номерИсходногоДокументаField;
        }
        set {
            this.номерИсходногоДокументаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="duration")]
    public string ДлительностьОжиданияОтвета {
        get {
            return this.длительностьОжиданияОтветаField;
        }
        set {
            this.длительностьОжиданияОтветаField = value;
        }
    }
    
    /// <remarks/>
    public bool ЭтоПолныйКаталог {
        get {
            return this.этоПолныйКаталогField;
        }
        set {
            this.этоПолныйКаталогField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ЭтоПолныйКаталогSpecified {
        get {
            return this.этоПолныйКаталогFieldSpecified;
        }
        set {
            this.этоПолныйКаталогFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаКаталога[] Товар {
        get {
            return this.товарField;
        }
        set {
            this.товарField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.9037.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ТоварКРаботе : КоммерческийДокумент {
    
    private ИдентификаторДокумента номерИсходногоДокументаField;
    
    private СтрокаТовараКРаботе[] товарField;
    
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента {
        get {
            return this.номерИсходногоДокументаField;
        }
        set {
            this.номерИсходногоДокументаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаТовараКРаботе[] Товар {
        get {
            return this.товарField;
        }
        set {
            this.товарField = value;
        }
    }
}
