﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Represents the result of a sign-in operation.
/// </summary>
public class IdentityResultResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Returns a flag indication whether the sign-in was successful.
    /// </summary>
    /// <value>True if the sign-in was successful, otherwise false.</value>
    public bool? Succeeded { get; set; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in is locked out.
    /// </summary>
    /// <value>True if the user attempting to sign-in is locked out, otherwise false.</value>
    public bool? IsLockedOut { get; set; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in is not allowed to sign-in.
    /// </summary>
    /// <value>True if the user attempting to sign-in is not allowed to sign-in, otherwise false.</value>
    public bool? IsNotAllowed { get; set; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in requires two factor authentication.
    /// </summary>
    /// <value>True if the user attempting to sign-in requires two factor authentication, otherwise false.</value>
    public bool? RequiresTwoFactor { get; set; }
}