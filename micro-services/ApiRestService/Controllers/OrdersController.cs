////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;
using Microsoft.AspNetCore.Http.Extensions;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Заказы
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersReadCommerce)},{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
public class OrdersController(ICommerceTransmission commRepo, IHelpdeskTransmission hdRepo, IStorageTransmission storageRepo) : ControllerBase
{
    /// <summary>
    /// Подбор (поиск по параметрам) заказов
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrdersReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.ORDERS_CONTROLLER_NAME}/{Routes.SELECT_ACTION_NAME}")]
#if !DEBUG
    [LoggerNolog]
#endif
    public async Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelect(TPaginationRequestModel<OrdersSelectRequestModel> req)
        => await commRepo.OrdersSelectAsync(new TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>>() { Payload = new TAuthRequestModel<OrdersSelectRequestModel>() { Payload = req.Payload, SenderActionUserId = GlobalStaticConstants.Roles.System } });

    /// <summary>
    /// Прикрепить файл к заказу (счёт, акт и т.п.). Имя файла должно быть уникальным в контексте заказа. Если файл с таким именем существует, тогда он будет перезаписан новым
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/api/{Routes.ORDER_CONTROLLER_NAME}/{{OrderId}}/{Routes.ATTACHMENT_CONTROLLER_NAME}-{Routes.ADD_ACTION_NAME}")]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<TResponseModel<StorageFileModelDB>> AttachmentForOrder([FromRoute] int OrderId, IFormFile uploadedFile)
    {
        TResponseModel<StorageFileModelDB> response = new();
        if (uploadedFile is null || uploadedFile.Length == 0)
        {
            response.AddError("Данные файла отсутствуют");
            return response;
        }

        TResponseModel<OrderDocumentModelDB[]> call = await commRepo.OrdersReadAsync(new() { Payload = [OrderId], SenderActionUserId = GlobalStaticConstants.Roles.System });

        if (!call.Success())
        {
            response.AddRangeMessages(call.Messages);
            return response;
        }
        else if (call.Response?.Length != 1)
        {
            response.AddError($"Заказ #{OrderId} не найден или у вас не достаточно прав для выполнения команды");
            return response;
        }

        string _file_name = uploadedFile.FileName.Trim();
        if (string.IsNullOrWhiteSpace(_file_name))
            _file_name = $"без имени: {DateTime.UtcNow}";

        using MemoryStream stream = new();
        uploadedFile.OpenReadStream().CopyTo(stream);
        StorageImageMetadataModel reqSave = new()
        {
            ApplicationName = Routes.ORDER_CONTROLLER_NAME,
            PropertyName = Routes.ATTACHMENT_CONTROLLER_NAME,
            PrefixPropertyName = Routes.REST_CONTROLLER_NAME,
            AuthorUserIdentity = GlobalStaticConstants.Roles.System,
            FileName = _file_name,
            ContentType = uploadedFile.ContentType,
            OwnerPrimaryKey = OrderId,
            Referrer = Request.GetEncodedPathAndQuery(),
            Payload = stream.ToArray(),
        };

        return await storageRepo.SaveFileAsync(new() { Payload = reqSave, SenderActionUserId = GlobalStaticConstants.Roles.System });
    }

    /// <summary>
    /// Обновить (или создать) строку документа
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/api/{Routes.ORDER_CONTROLLER_NAME}/{Routes.ROW_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}")]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<TResponseModel<int>> RowForOrderUpdate(RowOfOrderDocumentModelDB row)
        => await commRepo.RowForOrderUpdateAsync(row);

    /// <summary>
    /// Удалить строки из заказа
    /// </summary>
    /// <param name="rows_ids">Идентификаторы строк, которые следует удалить</param>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    [HttpDelete($"/api/{Routes.ORDERS_CONTROLLER_NAME}/{Routes.ROW_CONTROLLER_NAME}-{Routes.DELETE_ACTION_NAME}")]
    public async Task<TResponseModel<bool>> RowForOrderDelete([FromBody] int[] rows_ids)
        => await commRepo.RowsForOrderDeleteAsync(rows_ids);

    /// <summary>
    /// Установить статус заказа
    /// </summary>
    /// <param name="OrderId">Идентификатор заказа</param>
    /// <param name="Step">Статус заказа, который нужно установить</param>
    [HttpPost($"/api/{Routes.ORDER_CONTROLLER_NAME}/{{OrderId}}/{Routes.STAGE_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}/{{Step}}")]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<TResponseModel<bool>> OrderStageSet([FromRoute] int OrderId, [FromRoute] StatusesDocumentsEnum Step)
    {
        TResponseModel<OrderDocumentModelDB[]> call = await commRepo.OrdersReadAsync(new() { Payload = [OrderId], SenderActionUserId = GlobalStaticConstants.Roles.System });
        TResponseModel<bool> response = new() { Response = false };
        if (!call.Success())
        {
            response.AddRangeMessages(call.Messages);
            return response;
        }
        else if (call.Response?.Length != 1)
        {
            response.AddError($"Заказ #{OrderId} не найден или у вас не достаточно прав для выполнения команды");
            return response;
        }

        OrderDocumentModelDB order_doc = call.Response.Single();

        if (order_doc.HelpdeskId.HasValue != true)
        {
            response.AddError($"Заказ #{OrderId} не найден или у вас не достаточно прав для выполнения команды");
            return response;
        }

        TAuthRequestModel<IssuesReadRequestModel> req_hd = new()
        {
            SenderActionUserId = GlobalStaticConstants.Roles.System,
            Payload = new()
            {
                IssuesIds = [order_doc.HelpdeskId.Value]
            }
        };
        TResponseModel<IssueHelpdeskModelDB[]> find_helpdesk = await hdRepo.IssuesReadAsync(req_hd);
        if (!find_helpdesk.Success() || find_helpdesk.Response is null || find_helpdesk.Response.Length != 1)
        {
            response.AddRangeMessages(find_helpdesk.Messages);
            return response;
        }
        IssueHelpdeskModelDB hd_obj = find_helpdesk.Response.Single();
        if (hd_obj.StatusDocument == Step)
        {
            response.AddInfo("Статус уже установлен!");
            return response;
        }
        TAuthRequestModel<StatusChangeRequestModel> status_change_req = new()
        {
            SenderActionUserId = GlobalStaticConstants.Roles.System,
            Payload = new()
            {
                DocumentId = hd_obj.Id,
                Step = Step,
            }
        };
        TResponseModel<bool> update_final = await hdRepo.StatusChangeAsync(status_change_req);
        response.AddRangeMessages(update_final.Messages);
        return response;
    }
}