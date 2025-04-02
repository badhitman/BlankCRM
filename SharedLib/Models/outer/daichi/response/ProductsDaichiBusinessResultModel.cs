////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// ProductsDaichiBusinessResultModel
/// </summary>
public class ProductsDaichiBusinessResultModel
{
    /// <inheritdoc/>
    public JObject? Result { get; set; }

    /// <inheritdoc/>
    public double Time { get; set; }

    /// <inheritdoc/>
    Dictionary<string, JToken?>? GetJTokens
    {
        get
        {
            if (Result is null)
                return null;

            Dictionary<string, JToken?> dict_data = [];
            foreach (KeyValuePair<string, JToken?> kvp in Result)
            {
                JValue? jv = kvp.Value as JValue;
                if (!dict_data.ContainsKey(kvp.Key))
                    dict_data.Add(kvp.Key, jv);
            }

            return dict_data;
        }
    }

    /// <inheritdoc/>
    public List<GoodsDaichiModel>? GetProducts
    {
        get
        {
            Dictionary<string, JToken?>? tokens = GetJTokens;

            if (tokens is null)
                return null;

            List<GoodsDaichiModel> res = [];
            return res;
        }
    }
}