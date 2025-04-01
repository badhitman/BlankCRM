////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace ToolsMauiApp.Components.Layout;

/// <summary>
/// NavMenu
/// </summary>
public partial class NavMenu
{
#if DEBUG
    bool IsDebug = true;
#else
    bool IsDebug = false;
#endif
}