﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IKladrNavigationService
/// </summary>
public interface IKladrNavigationService
{
    /// <summary>
    /// Получить objects
    /// </summary>
    public Task<List<UniversalBaseModel>> ObjectsList(KladrsListRequestModel req);
}