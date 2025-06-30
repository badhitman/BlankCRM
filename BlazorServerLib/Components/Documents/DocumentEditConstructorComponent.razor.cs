﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Documents;

/// <summary>
/// Document edit
/// </summary>
public partial class DocumentEditConstructorComponent : DocumentEditBaseComponent
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Project Id
    /// </summary>
    [Parameter]
    public int? ProjectId { get; set; }


    DocumentSchemeConstructorModelDB[]? schemes;
    SessionOfDocumentDataModelDB? session;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (IsCancel)
            return;

        await SetBusyAsync();
        TResponseModel<DocumentSchemeConstructorModelDB[]?> ds = await JournalRepo.FindDocumentSchemes(DocumentNameOrId, ProjectId);
        SnackBarRepo.ShowMessagesResponse(ds.Messages);

        schemes = ds.Response;

        if (schemes?.Length == 1 && DocumentKey > 0)
        {
            TResponseModel<SessionOfDocumentDataModelDB> session_data = await ConstructorRepo.GetSessionDocumentAsync(new() { SessionId = DocumentKey.Value });
            SnackBarRepo.ShowMessagesResponse(session_data.Messages);
            session = session_data.Response;
        }
        IsBusyProgress = false;
    }

    /// <inheritdoc/>
    public override void TabChangeEvent(TTabOfDocumenBaseComponent tab_sender)
    {
        //TabOfDocumentConstructorComponent ct = (TabOfDocumentConstructorComponent)tab_sender;

        //return Task.CompletedTask;
    }


}