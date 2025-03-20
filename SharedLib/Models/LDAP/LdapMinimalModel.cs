////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// LDAP объект - минимальный набор атрибутов
/// </summary>
public class LdapMinimalModel(string _DistinguishedName, string _SAMAccountName, string _CommonName)
{
    /// <summary>
    /// Полное имя объекта
    /// </summary>
    public string DistinguishedName { get; set; } = _DistinguishedName;

    /// <summary>
    /// SAM-Account-Name
    /// </summary>
    public string SAMAccountName { get; set; } = _SAMAccountName;

    /// <summary>
    /// CN
    /// </summary>
    public string CommonName { get; set; } = _CommonName;
}