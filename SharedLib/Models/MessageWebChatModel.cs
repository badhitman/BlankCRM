////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MessageWebChatModel
/// </summary>
public record MessageWebChatModel(
       string Name,
       string Initials,
       string Text,
       DateTime Time)
{
}