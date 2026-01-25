////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

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
[System.Xml.Serialization.XmlIncludeAttribute(typeof(РеджектЗаказа))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(АкцептЗаказа))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(Заказ))]
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
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class ОКЕИ : КлассификаторТип {
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
public partial class ОКВЭД : КлассификаторТип {
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
public partial class Заказ : КоммерческийДокумент {
    
    private string длительностьОжиданияОтветаField;
    
    private ИдентификаторКонтрагента идСкладаField;
    
    private string адресСкладаField;
    
    private ИдентификаторКонтрагента идПолучателяТовараField;
    
    private System.DateTime датаВремяДоставкиField;
    
    private string длительностьОжиданияДоставкиField;
    
    private СтрокаЗаказа[] товарField;
    
    private System.Xml.XmlElement[] anyField;
    
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
    public ИдентификаторКонтрагента ИдСклада {
        get {
            return this.идСкладаField;
        }
        set {
            this.идСкладаField = value;
        }
    }
    
    /// <remarks/>
    public string АдресСклада {
        get {
            return this.адресСкладаField;
        }
        set {
            this.адресСкладаField = value;
        }
    }
    
    /// <remarks/>
    public ИдентификаторКонтрагента ИдПолучателяТовара {
        get {
            return this.идПолучателяТовараField;
        }
        set {
            this.идПолучателяТовараField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime ДатаВремяДоставки {
        get {
            return this.датаВремяДоставкиField;
        }
        set {
            this.датаВремяДоставкиField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="duration")]
    public string ДлительностьОжиданияДоставки {
        get {
            return this.длительностьОжиданияДоставкиField;
        }
        set {
            this.длительностьОжиданияДоставкиField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаЗаказа[] Товар {
        get {
            return this.товарField;
        }
        set {
            this.товарField = value;
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
[System.Xml.Serialization.XmlRootAttribute("Товар", Namespace="urn:moo-sodi.ru:commerceml_sodi", IsNullable=false)]
public partial class СтрокаЗаказа {
    
    private ИдентификаторТовара идТовараField;
    
    private decimal количествоField;
    
    private СтоимостьТип стоимостьField;
    
    private string примечаниеField;
    
    private System.Xml.XmlElement[] anyField;
    
    /// <remarks/>
    public ИдентификаторТовара ИдТовара {
        get {
            return this.идТовараField;
        }
        set {
            this.идТовараField = value;
        }
    }
    
    /// <remarks/>
    public decimal Количество {
        get {
            return this.количествоField;
        }
        set {
            this.количествоField = value;
        }
    }
    
    /// <remarks/>
    public СтоимостьТип Стоимость {
        get {
            return this.стоимостьField;
        }
        set {
            this.стоимостьField = value;
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
public partial class АкцептЗаказа : КоммерческийДокумент {
    
    private ИдентификаторДокумента номерИсходногоДокументаField;
    
    private System.Xml.XmlElement[] anyField;
    
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
public partial class РеджектЗаказа : КоммерческийДокумент {
    
    private ИдентификаторДокумента номерИсходногоДокументаField;
    
    private string длительностьОжиданияРеакцииField;
    
    private System.DateTime датаВремяДоставкиField;
    
    private string длительностьОжиданияДоставкиField;
    
    private СуммаТип превышениеЛимитаField;
    
    private СтрокаЗаказа[] товарField;
    
    private System.Xml.XmlElement[] anyField;
    
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
    public string ДлительностьОжиданияРеакции {
        get {
            return this.длительностьОжиданияРеакцииField;
        }
        set {
            this.длительностьОжиданияРеакцииField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime ДатаВремяДоставки {
        get {
            return this.датаВремяДоставкиField;
        }
        set {
            this.датаВремяДоставкиField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="duration")]
    public string ДлительностьОжиданияДоставки {
        get {
            return this.длительностьОжиданияДоставкиField;
        }
        set {
            this.длительностьОжиданияДоставкиField = value;
        }
    }
    
    /// <remarks/>
    public СуммаТип ПревышениеЛимита {
        get {
            return this.превышениеЛимитаField;
        }
        set {
            this.превышениеЛимитаField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаЗаказа[] Товар {
        get {
            return this.товарField;
        }
        set {
            this.товарField = value;
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
