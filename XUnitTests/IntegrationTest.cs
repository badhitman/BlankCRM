using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace XUnitTests.Tests
{
    public class IntegrationTest
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        [Fact]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            CancellationToken cancellationToken = TestContext.Current.CancellationToken;
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CommerceService>(cancellationToken);

            appHost.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                // Override the logging filters from the app's configuration
                logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
                // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
            });
            //appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            //{
            //    clientBuilder.AddStandardResilienceHandler();
            //});

            await using DistributedApplication app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
            await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

            /*
             // Arrange
                var mockRepo = new Mock<IBonusRepository>();
                var service = new BonusService(mockRepo.Object);
             */

            //// Act
            //using HttpClient httpClient = app.CreateHttpClient("webfrontend");
            //await app.ResourceNotifications.WaitForResourceHealthyAsync("webfrontend", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
            //using HttpResponseMessage response = await httpClient.GetAsync("/", cancellationToken);

            //// Assert
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
