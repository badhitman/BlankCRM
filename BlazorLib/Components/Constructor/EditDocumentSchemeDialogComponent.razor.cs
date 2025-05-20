////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor.Document;
using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Edit questionnaire dialog
/// </summary>
public partial class EditDocumentSchemeDialogComponent : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Inject]
    protected IConstructorTransmission ConstructorRepo { get; set; } = default!;


    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DocumentSchemeConstructorModelDB DocumentScheme { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    /// <inheritdoc/>
    protected bool IsEdited => DocumentScheme.Name != DocumentNameOrigin || DocumentScheme.Description != DocumentDescriptionOrigin;
    /// <inheritdoc/>
    protected TabsOfDocumentsSchemesViewComponent? pages_questionnaires_view_ref;
    /// <inheritdoc/>
    protected InputRichTextComponent? _currentTemplateInputRichText;


    string DocumentNameOrigin { get; set; } = "";
    string? DocumentDescriptionOrigin { get; set; }

    /// <inheritdoc/>
    protected void Close() => MudDialog.Close(DialogResult.Ok(DocumentScheme));

    async Task ResetDocumentForm()
    {
        if (DocumentScheme.Id > 0)
        {
            await SetBusyAsync();
            TResponseModel<DocumentSchemeConstructorModelDB> rest = await ConstructorRepo.GetDocumentSchemeAsync(DocumentScheme.Id);
            IsBusyProgress = false;

            SnackbarRepo.ShowMessagesResponse(rest.Messages);
            if (rest.Response is null)
            {
                SnackbarRepo.Add($"rest.Content.DocumentScheme is null. error 84DC51AA-74C1-4FA1-B9C6-B60548C10820", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                return;
            }
            DocumentScheme = rest.Response;
        }

        DocumentNameOrigin = DocumentScheme.Name;
        DocumentDescriptionOrigin = DocumentScheme.Description;

        pages_questionnaires_view_ref?.Update(DocumentScheme);
        _currentTemplateInputRichText?.SetValue(DocumentDescriptionOrigin);
    }

    /// <inheritdoc/>
    protected async Task SaveDocument()
    {
        if (ParentFormsPage.MainProject is null)
            throw new Exception("No main/used project selected");

        await SetBusyAsync();
        
        TResponseModel<DocumentSchemeConstructorModelDB> rest = await ConstructorRepo.UpdateOrCreateDocumentSchemeAsync(new() { Payload = new EntryConstructedModel() { Id = DocumentScheme.Id, Name = DocumentNameOrigin, Description = DocumentDescriptionOrigin, ProjectId = ParentFormsPage.MainProject.Id }, SenderActionUserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            SnackbarRepo.Add($"Ошибка C7172378-05A5-4547-ADA4-EA15B84C2CE1 Action: {rest.Message()}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        if (rest.Response is null)
        {
            SnackbarRepo.Add($"Ошибка FCB62EA3-689E-4222-9D59-8D1DEF18CFC5 rest.Content.DocumentScheme is null", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        DocumentScheme = rest.Response;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        await ResetDocumentForm();
        base.OnInitialized();
    }
}