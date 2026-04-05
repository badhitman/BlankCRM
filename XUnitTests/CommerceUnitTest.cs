using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommerceService;
using HelpdeskService;
using SharedLib;
using DbcLib;
using Moq;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace XUnitTests;

public class CommerceUnitTest
{
    readonly IDbContextFactory<CommerceContext> CommerceContextFactoryDb;
    readonly IDbContextFactory<HelpDeskContext> HelpdeskContextFactoryDb;
    readonly IParametersStorageTransmission StorageTransmissionRepo;
    readonly IIdentityTransmission IdentityTransmissionRepo;
    readonly IHistoryIndexing HistoryIndexingRepo;

    readonly CommerceImplementService CommerceService;
    readonly RetailService RetailService;
    readonly RubricsImplementService RubricsService;

    public CommerceUnitTest()
    {
        ServiceCollection services = new();

        services.AddDbContextFactory<CommerceContext>(options => options.UseInMemoryDatabase($"CommerceContext{GetType().Name}"));
        services.AddDbContextFactory<HelpDeskContext>(options => options.UseInMemoryDatabase($"HelpdeskContext{GetType().Name}"));

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        CommerceContextFactoryDb = serviceProvider.GetRequiredService<IDbContextFactory<CommerceContext>>();
        HelpdeskContextFactoryDb = serviceProvider.GetRequiredService<IDbContextFactory<HelpDeskContext>>();

        IdentityTransmissionRepo = new Mock<IIdentityTransmission>().Object;
        HistoryIndexingRepo = new Mock<IHistoryIndexing>().Object;
        StorageTransmissionRepo = new Mock<IParametersStorageTransmission>().Object;

        Mock<WebConfigModel> mockWebConf = new();
        Mock<ILogger<CommerceImplementService>> mockLoggerCommerceRepo = new();
        Mock<IWebTransmission> mockWebTransmissionRepo = new();
        Mock<IHelpdeskTransmission> mockHelpdeskRepo = new();
        Mock<IRubricsService> mockRubricsRepo = new();
        Mock<IStorageTransmission> mockFilesRepo = new();
        Mock<ITelegramTransmission> mockTelegramRepo = new();
        CommerceService = new(IdentityTransmissionRepo,
                              CommerceContextFactoryDb,
                              mockWebTransmissionRepo.Object,
                              mockHelpdeskRepo.Object,
                              mockRubricsRepo.Object,
                              mockFilesRepo.Object,
                              HistoryIndexingRepo,
                              mockTelegramRepo.Object,
                              mockLoggerCommerceRepo.Object,
                              mockWebConf.Object,
                              StorageTransmissionRepo);

        Mock<IKladrNavigationService> mockKladrRepo = new();
        Mock<ILogger<RetailService>> mockLoggerRetailRepo = new();
        RetailService = new(IdentityTransmissionRepo,
                            mockLoggerRetailRepo.Object,
                            mockKladrRepo.Object,
                            HistoryIndexingRepo,
                            StorageTransmissionRepo,
                            CommerceContextFactoryDb);

        Mock<IMemoryCache> mockCacheRepo = new();
        RubricsService = new(HelpdeskContextFactoryDb, mockCacheRepo.Object);
    }

    [Fact]
    public async Task TestCommerce()
    {
        NomenclatureModelDB nomenclature_1 = await CreateNomenclature();
        OfferModelDB offer_1 = await CreateOffer(nomenclature_1.Id);
        OfferModelDB offer_2 = await CreateOffer(nomenclature_1.Id);

        NomenclatureModelDB nomenclature_2 = await CreateNomenclature();
        OfferModelDB offer_3 = await CreateOffer(nomenclature_2.Id);

        RubricModelDB whRubric_1 = await CreateWarehouseRubric();
        RubricModelDB whRubric_2 = await CreateWarehouseRubric();

        WarehouseDocumentModelDB whDoc = await CreateWarehouseDocument(whRubric_1.Id);

        // Assert
        Assert.True(true);
    }

    async Task<RubricModelDB> CreateWarehouseRubric()
    {
        using HelpDeskContext context = await HelpdeskContextFactoryDb.CreateDbContextAsync();
        int countOffers = (await context.Rubrics.Where(x => x.ContextName == Routes.WAREHOUSE_CONTROLLER_NAME).CountAsync()) + 1;
        string name = $"Test Warehouse rubric: {countOffers}";
        RubricModelDB WarehouseRubric = new()
        {
            ContextName = Routes.WAREHOUSE_CONTROLLER_NAME,
            SortIndex = (uint)countOffers,
            CreatedAtUTC = DateTime.UtcNow,
            Name = name,
            NormalizedNameUpper = name.ToUpper(),
        };
        TResponseModel<int> whCreate = await RubricsService.RubricCreateOrUpdateAsync(WarehouseRubric);

        Assert.NotEqual(0, whCreate.Response);
        Assert.True(whCreate.Success());
        WarehouseRubric.Id = whCreate.Response;

        return WarehouseRubric;
    }

    async Task<WarehouseDocumentModelDB> CreateWarehouseDocument(int warehouseRubricId)
    {
        using CommerceContext context = await CommerceContextFactoryDb.CreateDbContextAsync();
        int countDocs = (await context.WarehouseDocuments.CountAsync()) + 1;
        WarehouseDocumentModelDB WarehouseDocumentDb = new()
        {
            DeliveryDate = DateTime.Now,
            NormalizedUpperName = "",
            CreatedAtUTC = DateTime.Now,
            Name = $"Test WH document: {countDocs}",
            WarehouseId = warehouseRubricId,
        };
        TAuthRequestStandardModel<WarehouseDocumentModelDB> warehouseDocCreateRequest = new() { Payload = WarehouseDocumentDb, SenderActionUserId = GlobalStaticConstantsRoutes.Routes.SYSTEM_CONTROLLER_NAME };

        DocumentNewVersionResponseModel warehouseDocNew = await CommerceService.WarehouseDocumentUpdateOrCreateAsync(warehouseDocCreateRequest);

        Assert.NotEqual(0, warehouseDocNew.Response);
        Assert.True(warehouseDocNew.Success());
        WarehouseDocumentDb.Id = warehouseDocNew.Response;

        return WarehouseDocumentDb;
    }

    async Task<OfferModelDB> CreateOffer(int nomenclatureId)
    {
        using CommerceContext context = await CommerceContextFactoryDb.CreateDbContextAsync();
        int countOffers = (await context.Offers.Where(x => x.NomenclatureId == nomenclatureId).CountAsync()) + 1;
        OfferModelDB OfferDb = new()
        {
            CreatedAtUTC = DateTime.UtcNow,
            Multiplicity = 1,
            Name = $"Test offer: {countOffers}",
            NomenclatureId = nomenclatureId,
            OfferUnit = UnitsOfMeasurementEnum.Thing,
            Price = 123,
            ShortName = $"test {countOffers}",
            Weight = 3 / 2,
        };
        TResponseModel<int> offerCreate = await CommerceService.OfferUpdateOrCreateAsync(new()
        {
            SenderActionUserId = Routes.SYSTEM_CONTROLLER_NAME,
            Payload = OfferDb
        });

        Assert.NotEqual(0, offerCreate.Response);
        Assert.True(offerCreate.Success());
        OfferDb.Id = offerCreate.Response;
        return OfferDb;
    }

    async Task<NomenclatureModelDB> CreateNomenclature()
    {
        using CommerceContext context = await CommerceContextFactoryDb.CreateDbContextAsync();
        int countN = (await context.Nomenclatures.CountAsync()) + 1;
        string nameN = $"Test Nomenclature: {countN}";
        NomenclatureModelDB NomenclatureDb = new()
        {
            BaseUnit = UnitsOfMeasurementEnum.Thing,
            CreatedAtUTC = DateTime.UtcNow,
            ContextName = "test-ctx",
            Name = nameN,
            NormalizedNameUpper = nameN.ToUpper(),
            SortIndex = (uint)countN,
        };

        TResponseModel<int> nomenclatureCreate = await CommerceService.NomenclatureUpdateOrCreateAsync(NomenclatureDb);
        Assert.NotEqual(0, nomenclatureCreate.Response);
        Assert.True(nomenclatureCreate.Success());
        NomenclatureDb.Id = nomenclatureCreate.Response;

        return NomenclatureDb;
    }
}