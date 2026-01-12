////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// OrderCreateComponent
/// </summary>
public partial class OrderCreateComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    bool _visibleChangeAddresses;
    bool _visibleChangeOrganization;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        CloseOnEscapeKey = true,
    };

    OrderDocumentModelDB? CurrentCart;
    readonly Func<OfficeOrganizationModelDB, string> converter = p => p.Name;

    List<OrganizationModelDB> Organizations { get; set; } = [];
    OrganizationModelDB? prevCurrOrg;
    OrganizationModelDB? CurrentOrganization
    {
        get => CurrentCart?.Organization;
        set
        {
            if (SelectedAddresses?.Any() == true)
            {
                _visibleChangeOrganization = true;
                prevCurrOrg = value;
                return;
            }

            if (CurrentCart is null)
#pragma warning disable CA2208
                throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);
#pragma warning restore CA2208

            CurrentCart.Organization = value;
            CurrentCart.OrganizationId = value?.Id ?? 0;
            ResetAddresses();
        }
    }

    /// <summary>
    /// Предварительный набор адресов, который пользователь должен утвердить/подтвердить.
    /// </summary>
    IEnumerable<OfficeOrganizationModelDB>? _prevSelectedAddresses;
    List<OfficeOrganizationModelDB>? _selectedAddresses = [];
    IEnumerable<OfficeOrganizationModelDB>? SelectedAddresses
    {
        get => _selectedAddresses ?? [];
        set
        {
            if (CurrentUserSession is null)
                throw new Exception("CurrentUserSession is null");

            if (_prevSelectedAddresses is not null || CurrentCart is null)
                return;

            CurrentCart.OfficesTabs ??= [];

            // адреса/вкладки, которые следует добавить
            OfficeOrganizationModelDB[] addresses = value is null ? [] : [.. value.Where(x => !CurrentCart.OfficesTabs.Any(y => y.OfficeId == x.Id))];
            if (addresses.Length != 0)
            {
                CurrentCart.OfficesTabs.AddRange(addresses.Select(x => new TabOfficeForOrderModelDb()
                {
                    OfficeId = x.Id,
                    Rows = [],
                    Order = CurrentCart,
                    OrderId = CurrentCart.Id,
                    Office = new()
                    {
                        Id = x.Id,
                        AddressUserComment = x.AddressUserComment,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        Contacts = x.Contacts,
                        OrganizationId = CurrentOrganization!.Id,
                        Organization = CurrentOrganization,
                        KladrCode = x.KladrCode,
                        KladrTitle = x.KladrTitle,
                    }
                }));
                _selectedAddresses = [.. CurrentCart.OfficesTabs.Select(Convert)];
                InvokeAsync(async () => await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), false));
            }
            static OfficeOrganizationModelDB Convert(TabOfficeForOrderModelDb x) => new()
            {
                Id = x.Office!.Id,
                Name = x.Office.Name,
                AddressUserComment = x.Office.AddressUserComment,
                Contacts = x.Office.Contacts,
                ParentId = x.Office.ParentId,
                Organization = x.Office.Organization,
                OrganizationId = x.Office.OrganizationId,
                KladrCode = x.Office.KladrCode,
                KladrTitle = x.Office.KladrTitle,
            };

            // адреса/вкладки, которые пользователь хочет удалить
            TabOfficeForOrderModelDb[] _qr = CurrentCart
                .OfficesTabs
                .Where(x => value?.Any(y => y.Id == x.OfficeId) != true).ToArray();

            // адреса/вкладки, которые можно свободно удалить (без строк)
            OfficeOrganizationModelDB[] _prev = [.. _qr
                 .Where(x => x.Rows is null || x.Rows.Count == 0)
                 .Select(Convert)];
            if (_prev.Length != 0)
            {
                CurrentCart.OfficesTabs!.RemoveAll(x => _prev.Any(y => y.Id == x.OfficeId));
                _selectedAddresses = [.. CurrentCart.OfficesTabs.Select(Convert)];
                InvokeAsync(async () => { await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true); });
            }

            // адреса/вкладки, которые имеют строки: требуют подтверждения у пользователя
            _prev = [.. _qr
                .Where(x => x.Rows is not null && x.Rows.Count != 0)
                .Select(Convert)];

            if (_prev.Length != 0)
            {
                _prevSelectedAddresses = value;
                _visibleChangeAddresses = true;
            }
            else
            {
                _prevSelectedAddresses = null;
                _selectedAddresses = value?.ToList();
            }
        }
    }

    RowOfOrderDocumentModelDB[] AllRows = default!;

    /// <summary>
    /// Сгруппировано по OfferId
    /// </summary>
    List<IGrouping<int, RowOfOrderDocumentModelDB>> GroupingRows { get; set; } = default!;

    private readonly Dictionary<int, PriceRuleForOfferModelDB[]?> RulesCache = [];
    readonly Dictionary<int, decimal> DiscountsDetected = [];

    async Task ActualityData()
    {
        int[]? offersIds = CurrentCart?
            .OfficesTabs?
            .SkipWhile(x => x.Rows is null)
            .SelectMany(x => x.Rows!.Select(y => y.OfferId))
            .Distinct()
            .ToArray();

        if (offersIds is null || offersIds.Length == 0 || CurrentUserSession is null)
        {
            await SetBusyAsync(false);
            return;
        }

        TResponseModel<OfferModelDB[]> offersRes = await CommerceRepo.OffersReadAsync(new() { Payload = offersIds, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);
        if (!offersRes.Success() || offersRes.Response is null || offersRes.Response.Length == 0)
        {
            SnackBarRepo.ShowMessagesResponse(offersRes.Messages);
            return;
        }

        CurrentCart!.OfficesTabs!.ForEach(adRow =>
        {
            adRow.Rows?.ForEach(orderRow =>
                {
                    orderRow.Offer = offersRes.Response.First(sr => sr.Id == orderRow.OfferId);
                });
        });

    }

    void CalculateDiscounts()
    {
        if (GroupingRows.Any(x => x.Any(y => y.Offer is null)))
            return;

        string json_dump_discounts_before = JsonConvert.SerializeObject(DiscountsDetected, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        DiscountsDetected.Clear();
        foreach (IGrouping<int, RowOfOrderDocumentModelDB> node in GroupingRows)
        {
            decimal qnt = node.Sum(y => y.Quantity); // всего количество в заказе
            if (qnt <= 1 || !RulesCache.TryGetValue(node.Key, out PriceRuleForOfferModelDB[]? _rules) || _rules?.Any() != true)
                continue;

            decimal base_price = node.First().Offer!.Price;
            PriceRuleForOfferModelDB? find_rule = null;
            DiscountsDetected.Add(node.Key, 0);
            for (int i = 2; i <= qnt; i++)
            {
                find_rule = _rules.FirstOrDefault(x => x.QuantityRule == i) ?? find_rule;
                if (find_rule is null)
                    continue;

                DiscountsDetected[node.Key] += base_price - find_rule.PriceRule;
            }

            if (DiscountsDetected[node.Key] == 0)
                DiscountsDetected.Remove(node.Key);
        }

        string json_dump_discounts_after = JsonConvert.SerializeObject(DiscountsDetected, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        if (json_dump_discounts_before != json_dump_discounts_after)
            StateHasChanged();
    }

    decimal SumOfDocument
    {
        get
        {
            if (CurrentCart is null)
                throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);

            if (CurrentCart.OfficesTabs is null || CurrentCart.OfficesTabs.Count == 0)
                return 0;

            IQueryable<RowOfOrderDocumentModelDB> rows = CurrentCart
                .OfficesTabs
                .Where(x => x.Rows is not null)
                .SelectMany(x => x.Rows!)
                .AsQueryable();

            if (!rows.Any())
                return 0;

            return rows.Sum(x => x.Quantity * x.Offer!.Price);
        }
    }

    async Task SubmitChangeAddresses()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (CurrentCart is null)
            throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);

        CurrentCart.OfficesTabs ??= [];
        if (_prevSelectedAddresses is null || !_prevSelectedAddresses.Any())
            CurrentCart.OfficesTabs.Clear();
        else
            CurrentCart.OfficesTabs.RemoveAll(x => !_prevSelectedAddresses.Any(y => y.Id == x.OfficeId));

        _selectedAddresses = _prevSelectedAddresses?.ToList();
        _prevSelectedAddresses = null;
        _visibleChangeAddresses = false;
        await SetBusyAsync();

        await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);
        await SetBusyAsync(false);
    }

    void CancelChangeAddresses()
    {
        _visibleChangeAddresses = false;
        _prevSelectedAddresses = null;
    }

    void SubmitChangeOrganizations()
    {
        if (CurrentCart is null)
            throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);

        CurrentCart.Organization = prevCurrOrg;
        CurrentCart.OrganizationId = prevCurrOrg?.Id ?? 0;
        prevCurrOrg = null;
        CurrentCart.OfficesTabs?.RemoveAll(x => !CurrentOrganization!.Offices!.Any(y => y.Id == x.OfficeId));
        ResetAddresses();
        _visibleChangeOrganization = false;
    }

    async Task ClearOrder()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (CurrentCart?.OfficesTabs is null)
            return;
        CurrentCart.OfficesTabs.ForEach(x => x.Rows?.Clear());
        await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);
        NavRepo.Refresh(true);
    }

    async void DocumentUpdateAction()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (CurrentCart is null)
            throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);

        await SetBusyAsync();
        CurrentCart.OfficesTabs?.RemoveAll(x => !CurrentOrganization!.Offices!.Any(y => y.Id == x.OfficeId));
        await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);
        await SetBusyAsync(false);
    }

    void CancelChangeOrganizations()
    {
        _visibleChangeOrganization = false;
        prevCurrOrg = null;
    }

    void ResetAddresses()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        _selectedAddresses?.Clear();
        InvokeAsync(async () => { await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true); });
    }

    async Task OrderDocumentSendAsync()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (CurrentCart is null)
            throw new ArgumentNullException(nameof(CurrentCart), GetType().FullName);

        if (CurrentCart.OfficesTabs?.Any(x => x.Rows is null || x.Rows.Count == 0) == true)
        {
            SnackBarRepo.Error("Присутствуют адреса без номенклатуры заказа. Исключите пустую вкладку или заполните её данными");
            return;
        }

        await SetBusyAsync();
        TResponseModel<int> rest = await CommerceRepo.OrderUpdateOrCreateAsync(new()
        {
            Payload = CurrentCart,
            SenderActionUserId = CurrentUserSession.UserId
        });
        SnackBarRepo.ShowMessagesResponse(rest.Messages);

        if (rest.Response == 0)
        {
            await SetBusyAsync(false);
            return;
        }

        if (rest.Success())
        {
            TResponseModel<OrderDocumentModelDB[]> doc = await CommerceRepo.OrdersReadAsync(new() { Payload = [rest.Response], SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });
            CurrentCart.Description = CurrentCart.Description?.Trim();
            CurrentCart = OrderDocumentModelDB.NewEmpty(CurrentUserSession.UserId);

            await StorageRepo
            .SaveParameterAsync<OrderDocumentModelDB?>(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);

            NavRepo.NavigateTo($"/issue-card/{doc.Response!.First().HelpDeskId}");
        }
        else
        {
            await SetBusyAsync(false);
        }
    }

    async Task UpdateCachePriceRules()
    {
        if (CurrentUserSession is null)
            return;

        CurrentCart ??= OrderDocumentModelDB.NewEmpty(CurrentUserSession.UserId);

        if (CurrentCart.OfficesTabs is null)
        {
            AllRows = [];
            GroupingRows = [];
            return;
        }

        AllRows = CurrentCart.OfficesTabs?.Where(x => x.Rows is not null).SelectMany(x => x.Rows!).ToArray() ?? [];
        GroupingRows = AllRows.GroupBy(x => x.OfferId).ToList();
        List<int> offers_load = [.. GroupingRows.Where(dc => !RulesCache.ContainsKey(dc.Key)).Select(x => x.Key).Distinct()];

        if (offers_load.Count == 0)
            return;

        //await SetBusy();
        TResponseModel<List<PriceRuleForOfferModelDB>> res = await CommerceRepo.PricesRulesGetForOffersAsync(new() { Payload = [.. offers_load], SenderActionUserId = CurrentUserSession.UserId });
        //await SetBusy(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Success() && res.Response is not null)
            offers_load.ForEach(x =>
            {
                if (RulesCache.ContainsKey(x))
                    RulesCache[x] = res.Response.Where(y => x == y.OfferId && !y.IsDisabled).ToArray();
                else
                    RulesCache.Add(x, res.Response.Where(y => x == y.OfferId && !y.IsDisabled).ToArray());
            });
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && CurrentCart is not null)
        {
            await UpdateCachePriceRules();
            CalculateDiscounts();
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        await OrganizationReset();
        await UpdateCachePriceRules();
        await ActualityData();
        CalculateDiscounts();
    }

    async Task OrganizationReset()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req = new()
        {
            Payload = new()
            {
                ForUserIdentityId = CurrentUserSession.IsAdmin ? null : CurrentUserSession.UserId,
                IncludeExternalData = true,
            },
            SenderActionUserId = CurrentUserSession.UserId,
            PageNum = 0,
            PageSize = 100,
            SortBy = nameof(OrderDocumentModelDB.Name),
            SortingDirection = DirectionsEnum.Up,
        };

        TPaginationResponseStandardModel<OrganizationModelDB> res = await CommerceRepo.OrganizationsSelectAsync(req);

        if (res.Response is null || res.Response.Count == 0)
            return;

        if (res.TotalRowsCount > req.PageSize)
            SnackBarRepo.Error($"Записей больше: {res.TotalRowsCount}");

        Organizations = res.Response;
        TResponseModel<OrderDocumentModelDB?> current_cart = await StorageRepo
            .ReadParameterAsync<OrderDocumentModelDB>(GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId));

        CurrentCart = current_cart.Response ?? new()
        {
            AuthorIdentityUserId = CurrentUserSession.UserId,
            Name = "Новый заказ",
        };

        if (CurrentCart.OrganizationId != 0)
        {
            CurrentCart.Organization = Organizations.FirstOrDefault(x => x.Id == CurrentCart.OrganizationId);
            if (CurrentCart.Organization is null)
                CurrentCart.OrganizationId = 0;

            await StorageRepo.SaveParameterAsync(CurrentCart, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);
        }

        CurrentCart.OfficesTabs ??= [];
        CurrentCart.OfficesTabs.RemoveAll(x => !CurrentOrganization!.Offices!.Any(y => y.Id == x.OfficeId));
        if (CurrentCart.OfficesTabs.Count != 0)
        {
            _selectedAddresses = [.. CurrentCart
                .OfficesTabs
                .Select(x => CurrentOrganization!.Offices!.First(y => y.Id == x.OfficeId))];
        }
    }
}