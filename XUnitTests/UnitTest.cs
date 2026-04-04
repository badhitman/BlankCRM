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
    readonly IParametersStorageTransmission StorageTransmissionRepo;
    readonly IDbContextFactory<CommerceContext> ContextFactoryDb;
    readonly IIdentityTransmission IdentityTransmissionRepo;
    readonly IHistoryIndexing HistoryIndexingRepo;

    public UnitTest()
    {
        ServiceCollection services = new();
        services.AddDbContextFactory<CommerceContext>(options => options.UseInMemoryDatabase("TestDb"));
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        ContextFactoryDb = serviceProvider.GetRequiredService<IDbContextFactory<CommerceContext>>();

        IdentityTransmissionRepo = new Mock<IIdentityTransmission>().Object;
        HistoryIndexingRepo = new Mock<IHistoryIndexing>().Object;
        StorageTransmissionRepo = new Mock<IParametersStorageTransmission>().Object;
    }

    [Fact]
    public async Task Test()
    {
        Mock<WebConfigModel> mockWebConf = new();
        Mock<ILogger<CommerceImplementService>> mockLoggerCommerceRepo = new();

        Mock<IWebTransmission> mockWebTransmissionRepo = new();
        Mock<IHelpDeskTransmission> mockHelpDeskRepo = new();
        Mock<IRubricsTransmission> mockRubricsRepo = new();
        Mock<IStorageTransmission> mockFilesRepo = new();

        Mock<ITelegramTransmission> mockTelegramRepo = new();

        CommerceImplementService commerceService = new(IdentityTransmissionRepo,
                                                       ContextFactoryDb,
                                                       mockWebTransmissionRepo.Object,
                                                       mockHelpDeskRepo.Object,
                                                       mockRubricsRepo.Object,
                                                       mockFilesRepo.Object,
                                                       HistoryIndexingRepo,
                                                       mockTelegramRepo.Object,
                                                       mockLoggerCommerceRepo.Object,
                                                       mockWebConf.Object,
                                                       StorageTransmissionRepo);

        NomenclatureModelDB Nomenclature_1 = await CreateNomenclature(commerceService);
        OfferModelDB Offer_1 = await CreateOffer(commerceService, Nomenclature_1.Id);

        NomenclatureModelDB Nomenclature_2 = await CreateNomenclature(commerceService);
        OfferModelDB Offer_2 = await CreateOffer(commerceService, Nomenclature_2.Id);

        Mock<IKladrNavigationService> mockKladrRepo = new();
        Mock<ILogger<RetailService>> mockLoggerRetailRepo = new();
        RetailService service = new(IdentityTransmissionRepo,
                                    mockLoggerRetailRepo.Object,
                                    mockKladrRepo.Object,
                                    HistoryIndexingRepo,
                                    StorageTransmissionRepo,
                                    ContextFactoryDb);

        // Assert
        Assert.True(true);
    }

    async Task<OfferModelDB> CreateOffer(CommerceImplementService commerceService, int nomenclatureId)
    {
        using CommerceContext context = await ContextFactoryDb.CreateDbContextAsync();
        int countOffers = (await context.Offers.Where(x => x.NomenclatureId == nomenclatureId).CountAsync()) + 1;
        OfferModelDB OfferDb = new()
        {
            CreatedAtUTC = DateTime.UtcNow,
            Multiplicity = 1,
            Name = $"test {countOffers}",
            NomenclatureId = nomenclatureId,
            OfferUnit = UnitsOfMeasurementEnum.Thing,
            Price = 123,
            ShortName = $"test {countOffers}",
            Weight = 3 / 2,
        };
        TResponseModel<int> offerCreate = await commerceService.OfferUpdateOrCreateAsync(new()
        {
            SenderActionUserId = GlobalStaticConstantsRoutes.Routes.SYSTEM_CONTROLLER_NAME,
            Payload = OfferDb
        });

        Assert.NotEqual(0, offerCreate.Response);
        OfferDb.Id = offerCreate.Response;
        return OfferDb;
    }

    async Task<NomenclatureModelDB> CreateNomenclature(CommerceImplementService commerceService)
    {
        using CommerceContext context = await ContextFactoryDb.CreateDbContextAsync();
        int countN = (await context.Nomenclatures.CountAsync()) + 1;
        string nameN = $"test {countN}";
        NomenclatureModelDB NomenclatureDb = new()
        {
            BaseUnit = UnitsOfMeasurementEnum.Thing,
            CreatedAtUTC = DateTime.UtcNow,
            ContextName = "test-ctx",
            Name = nameN,
            NormalizedNameUpper = nameN.ToUpper(),
            SortIndex = (uint)countN,
        };

        TResponseModel<int> nomenclatureCreate = await commerceService.NomenclatureUpdateOrCreateAsync(NomenclatureDb);
        Assert.NotEqual(0, nomenclatureCreate.Response);
        NomenclatureDb.Id = nomenclatureCreate.Response;

        return NomenclatureDb;
    }
}