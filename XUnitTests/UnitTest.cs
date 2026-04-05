using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommerceService;
using SharedLib;
using DbcLib;
using Moq;

namespace XUnitTests;

public class UnitTest
{
    readonly IDbContextFactory<CommerceContext> CommerceContextFactoryDb;
    readonly IDbContextFactory<HelpDeskContext> HelpdeskContextFactoryDb;
    readonly IParametersStorageTransmission StorageTransmissionRepo;
    readonly IIdentityTransmission IdentityTransmissionRepo;
    readonly IHistoryIndexing HistoryIndexingRepo;

    readonly CommerceImplementService CommerceService;
    readonly RetailService RetailService;
    // readonly RubricsService

    public UnitTest()
    {
        ServiceCollection services = new();
        
        services.AddDbContextFactory<CommerceContext>(options => options.UseInMemoryDatabase("TestCommerceContext"));
        services.AddDbContextFactory<HelpDeskContext>(options => options.UseInMemoryDatabase("TestHelpdeskContext"));

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
    }

    [Fact]
    public async Task Test()
    {
        NomenclatureModelDB nomenclature_1 = await CreateNomenclature();
        OfferModelDB offer_1 = await CreateOffer(nomenclature_1.Id);
        OfferModelDB offer_2 = await CreateOffer(nomenclature_1.Id);

        NomenclatureModelDB nomenclature_2 = await CreateNomenclature();
        OfferModelDB offer_3 = await CreateOffer(nomenclature_2.Id);

        WarehouseDocumentModelDB whDoc = await CreateWarehouseDocument();

        // Assert
        Assert.True(true);
    }

    async Task<WarehouseDocumentModelDB> CreateWarehouseDocument()
    {
        using CommerceContext context = await CommerceContextFactoryDb.CreateDbContextAsync();
        int countDocs = (await context.WarehouseDocuments.CountAsync()) + 1;
        WarehouseDocumentModelDB WarehouseDocumentDb = new()
        {
            DeliveryDate = DateTime.Now,
            NormalizedUpperName = "",
            CreatedAtUTC = DateTime.Now,
            Name = $"Test WH document: {countDocs}",
            WarehouseId = 0,
        };
        TAuthRequestStandardModel<WarehouseDocumentModelDB> warehouseDocCreate = new()
        {
            Payload = WarehouseDocumentDb,
            SenderActionUserId = GlobalStaticConstantsRoutes.Routes.SYSTEM_CONTROLLER_NAME
        };

        DocumentNewVersionResponseModel wh = await CommerceService.WarehouseDocumentUpdateOrCreateAsync(warehouseDocCreate);

        Assert.NotEqual(0, wh.Response);
        Assert.True(wh.Success());
        WarehouseDocumentDb.Id = wh.Response;

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
            SenderActionUserId = GlobalStaticConstantsRoutes.Routes.SYSTEM_CONTROLLER_NAME,
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