﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Имена HTTP клиентов для IHttpClientFactory (например: IHttpClientFactory.CreateClient(HttpClientsNamesEnum.Insight.ToString()))
/// </summary>
public enum HttpClientsNamesEnum
{
    /// <summary>
    /// Default
    /// </summary>
    Default = 10,

    /// <summary>
    /// Kladr
    /// </summary>
    Kladr = 20,

    /// <summary>
    /// Wappi
    /// </summary>
    Wappi = 30,

    /// <summary>
    /// RabbitMq rest/api Management
    /// </summary>
    RabbitMqManagement = 40,
}