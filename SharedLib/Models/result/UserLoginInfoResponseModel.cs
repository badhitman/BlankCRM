﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Информация для входа и источник записи пользователя.
/// </summary>
public class UserLoginInfoResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Информация для входа и источник записи пользователя.
    /// </summary>
    public UserLoginInfoModel? UserLoginInfoData { get; set; }

    /// <inheritdoc/>
    public string? IdentityName { get; set; }
}