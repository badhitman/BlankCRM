﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Articles;

/// <summary>
/// Редактирование статьи
/// </summary>
public partial class ArticleEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IHelpdeskTransmission ArticlesRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <summary>
    /// Article Id
    /// </summary>
    [Parameter]
    public int ArticleId { get; set; }


    ArticleModelDB? orignArticle;
    ArticleModelDB? editArticle;

    bool IsEdited => orignArticle is not null && editArticle is not null && !orignArticle.Equals(editArticle);

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    int[] SelectedNodesRead() => orignArticle?.RubricsJoins?.Select(x => x.RubricId).ToArray() ?? [];

    async Task SaveArticle()
    {
        if (editArticle is null)
            throw new Exception();

        await SetBusy();

        TResponseModel<int> res = await ArticlesRepo.ArticleCreateOrUpdateAsync(editArticle);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (editArticle.Id < 1 && res.Response > 0)
            NavRepo.NavigateTo($"/articles/edit-card/{res.Response}");
        else
        {
            await LoadArticleData();
            editArticle = GlobalTools.CreateDeepCopy(orignArticle) ?? throw new Exception();
        }
    }

    async Task LoadArticleData()
    {
        await SetBusy();

        TResponseModel<ArticleModelDB[]> res = await ArticlesRepo.ArticlesReadAsync([ArticleId]);
        await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
            throw new Exception();

        orignArticle = res.Response.Single();
    }

    async void SelectedRubricsChange(IReadOnlyCollection<UniversalBaseModel?> req)
    {
        if (editArticle?.RubricsJoins is not null && !req.Any(x => !editArticle!.RubricsJoins.Any(y => y.RubricId == x?.Id)) && !editArticle.RubricsJoins.Any(x => !req.Any(y => y?.Id == x.RubricId)))
            return;

        await SetBusy();
        ResponseBaseModel res = await ArticlesRepo.UpdateRubricsForArticleAsync(new() { ArticleId = ArticleId, RubricsIds = req.Select(x => x!.Id).ToArray() });
        await LoadArticleData();
        // await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();
        await ReadCurrentUser();
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{GlobalStaticConstants.Routes.ARTICLE_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.BODY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={GlobalStaticConstants.Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={ArticleId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        if (CurrentUserSession is null)
            throw new Exception();

        if (ArticleId > 0)
            await LoadArticleData();
        else
            orignArticle = new()
            {
                AuthorIdentityId = CurrentUserSession.UserId,
                Name = "",
            };

        editArticle = GlobalTools.CreateDeepCopy(orignArticle) ?? throw new Exception();
        await SetBusy(false);
    }
}