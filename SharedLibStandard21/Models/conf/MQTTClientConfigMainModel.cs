////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MQTTClientConfigMainModel
/// </summary>
public partial class MQTTClientConfigMainModel : RealtimeMQTTClientConfigModel
{
    /// <inheritdoc/>
    public new static MQTTClientConfigMainModel BuildEmpty()
    {
        return new MQTTClientConfigMainModel() { Scheme = "mqtt", Port = 1883 };
    }
}
 