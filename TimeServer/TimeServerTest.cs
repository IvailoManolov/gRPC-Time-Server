using Grpc.Core;
using Grpc.Net.Client;
using gRPC_Time_Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;
using System.Threading.Channels;

namespace TimeServer
{
    [TestFixture]
    public class TimeServerTest
    {
        private GrpcChannel? _httpChannel;
        private GrpcChannel? _httpsChannel;

        private TimeService.TimeServiceClient? _httpTimeServiceClient;
        private TimeService.TimeServiceClient? _httpsTimeServiceClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // HTTP setup
            var httpFactory = new WebApplicationFactory<Program>();
            var httpOptions = new GrpcChannelOptions { HttpHandler = httpFactory.Server.CreateHandler() };
            _httpChannel = GrpcChannel.ForAddress(httpFactory.Server.BaseAddress, httpOptions);
            _httpTimeServiceClient = new TimeService.TimeServiceClient(_httpChannel);

            // HTTPS setup
            var httpsFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseUrls("https://localhost:7149"); // Change the URL to your HTTPS URL
            });
            var httpsOptions = new GrpcChannelOptions { HttpHandler = httpsFactory.Server.CreateHandler() };
            _httpsChannel = GrpcChannel.ForAddress(httpsFactory.Server.BaseAddress, httpsOptions);
            _httpsTimeServiceClient = new TimeService.TimeServiceClient(_httpsChannel);
        }

        [SetUp]
        public void Setup()
        {
            _httpTimeServiceClient = new TimeService.TimeServiceClient(_httpChannel);
            _httpsTimeServiceClient = new TimeService.TimeServiceClient(_httpsChannel);
        }

        [Test]
        public async Task QueryTimeDatabase_ShouldReturnLogs_WhenUsingHttps()
        {
            // Arrange
            Certificate? certificate = null;
            // Provide a valid certificate if required

            // Act
            var response = await _httpsTimeServiceClient?.QueryTimeDatabaseAsync(certificate);

            // Assert
            NUnit.Framework.Assert.That(response, Is.Not.Null);
            NUnit.Framework.Assert.That(response.TimeLogs, Is.Not.Null);
            // Add other assertions as needed
        }

        [Test]
        public async Task QueryTimeDatabase_ShouldReturnNull_WhenUsingHttp()
        {
            // Arrange
            Certificate? certificate = null; // Provide a valid certificate if required

            // Act
            var response = await _httpTimeServiceClient.QueryTimeDatabaseAsync(certificate);

            // Assert
            NUnit.Framework.Assert.That(response, Is.Not.Null);
            NUnit.Framework.Assert.That(response.TimeLogs, Is.Null);
            // Add other assertions as needed
        }

    }
}