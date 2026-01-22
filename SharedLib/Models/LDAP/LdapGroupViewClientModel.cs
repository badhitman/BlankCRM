////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// LdapGroupViewClientModel
/// </summary>
public class LdapGroupViewClientModel : EntryDictStandardModel
{
    /// <summary>
    /// Group
    /// </summary>
    public LdapGroupViewModel? Group { get; set; }
}