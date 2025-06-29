﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Done client view
/// </summary>
public partial class DoneClientViewComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public required SessionOfDocumentDataModelDB SessionDocument { get; set; }

    /// <inheritdoc/>
    [CascadingParameter]
    public required bool InUse { get; set; }


    bool InitSend = false;

    /// <inheritdoc/>
    protected async Task SetAsDone()
    {
        if (string.IsNullOrWhiteSpace(SessionDocument.SessionToken))
        {
            SnackBarRepo.Error("string.IsNullOrWhiteSpace(SessionDocument.SessionToken). error 5E2D7979-53E7-4130-8DF2-53C00D378BEA");
            return;
        }

        if (!InitSend)
        {
            InitSend = true;
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel rest = await ConstructorRepo.SetDoneSessionDocumentDataAsync(SessionDocument.SessionToken);
        IsBusyProgress = false;

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Success())
            SessionDocument.SessionStatus = SessionsStatusesEnum.Sended;
    }
}