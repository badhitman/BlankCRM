////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Constructor.Pages;

/// <summary>
/// DocumentClientViewPage
/// </summary>
public partial class DocumentClientViewPage : BlazorBusyComponentBaseModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public Guid DocumentGuid { get; set; } = default!;


    SessionOfDocumentDataModelDB SessionOfDocumentData = default!;


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        ReloadSessionAction();
        await SetBusyAsync(false);
    }

    async void ReloadSessionAction()
    {
        await SetBusyAsync();

        TResponseModel<SessionOfDocumentDataModelDB> rest = await ConstructorRepo.GetSessionDocumentDataAsync(DocumentGuid.ToString());

        if (rest.Response is null)
            throw new Exception("rest.SessionDocument is null. error 5E20961A-3F1A-4409-9481-FA623F818918");

        SessionOfDocumentData = rest.Response;
        if (SessionOfDocumentData.DataSessionValues is not null && SessionOfDocumentData.DataSessionValues.Count != 0)
            SessionOfDocumentData.DataSessionValues.ForEach(x =>
            {
                x.Owner ??= SessionOfDocumentData;
                x.OwnerId = SessionOfDocumentData.Id;
            });

        await SetBusyAsync(false);
    }
}