////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.Users;

/// <summary>
/// ClaimRowComponent
/// </summary>
public partial class ClaimRowComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Claim
    /// </summary>
    [Parameter, EditorRequired]
    public required ClaimBaseModel Claim { get; set; }

    /// <summary>
    /// ClaimArea
    /// </summary>
    [Parameter, EditorRequired]
    public ClaimAreasEnum ClaimArea { get; set; }

    /// <summary>
    /// OwnerId
    /// </summary>
    [Parameter, EditorRequired]
    public required string OwnerId { get; set; }

    /// <summary>
    /// ReloadHandler
    /// </summary>
    [Parameter, EditorRequired]
    public required Action ReloadHandler { get; set; }

    bool isEdit;
    string? claimType;
    string? claimValue;

    bool CantSave
        => string.IsNullOrWhiteSpace(claimType) || string.IsNullOrWhiteSpace(claimValue) || Claim.Equals(claimType, claimValue);

    bool CannotSveClaim
        => CantSave || Claim.Equals(claimType, claimValue) || IsBusyProgress;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        CancelEdit();
    }

    void CancelEdit()
    {
        claimType = Claim.ClaimType;
        claimValue = Claim.ClaimValue;
        isEdit = false;
    }

    async Task SaveClaim()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await IdentityRepo.ClaimUpdateOrCreateAsync(new() { ClaimArea = ClaimArea, ClaimUpdate = ClaimModel.Build(Claim.Id, claimType, claimValue, OwnerId) });

        if (!res.Success())
            throw new Exception(res.Message());

        isEdit = false;
        ReloadHandler();

        await SetBusyAsync(false);
    }

    bool initRemoveClaim = false;
    async Task RemoveClaim()
    {
        if (!initRemoveClaim)
        {
            initRemoveClaim = true;
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await IdentityRepo.ClaimDeleteAsync(new() { ClaimArea = ClaimArea, Id = Claim.Id });

        initRemoveClaim = false;
        if (!res.Success())
            throw new Exception(res.Message());

        ReloadHandler();
        await SetBusyAsync(false);
    }
}