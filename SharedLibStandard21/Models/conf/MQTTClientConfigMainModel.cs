////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MQTTClientConfigMainModel
/// </summary>
public partial class MQTTClientConfigMainModel : MQTTClientConfigModel
{
    /// <inheritdoc/>
    public new static MQTTClientConfigMainModel BuildEmpty()
    {
        return new MQTTClientConfigMainModel() { Scheme = "mqtt", Port = 1883 };
    }
}
 