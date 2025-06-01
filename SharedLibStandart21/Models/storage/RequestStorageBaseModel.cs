////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.IO;

namespace SharedLib;

/// <summary>
/// RequestStorageCloudParameterModel
/// </summary>

public class RequestStorageBaseModel
{
    /// <summary>
    /// Имя приложения, которое обращается к службе облачного хранения параметров
    /// </summary>
    public  string ApplicationName { get; set; }


    /// <summary>
    /// Имя
    /// </summary>
    public  string PropertyName { get; set; }

    /// <summary>
    /// Normalize
    /// </summary>
    public virtual void Normalize()
    {
        ApplicationName = ApplicationName.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
}