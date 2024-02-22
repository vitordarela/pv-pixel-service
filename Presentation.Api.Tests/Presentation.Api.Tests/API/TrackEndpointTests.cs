namespace PvPixelService.Test.API
{
    using PvPixelService.Services.Tracking;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class TrackEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public TrackEndpointTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task TrackEndpoint_Should_Return_Image()
        {
            // Arrange
            byte[] transparentPixel = [71, 73, 70, 56, 57, 97, 1, 0, 1, 0, 128, 255, 0, 255, 255, 255, 0, 0, 0, 33, 249, 4, 1, 0, 0, 0, 0, 44, 0, 0, 0, 0, 1, 0, 1, 0, 0, 2, 2, 68, 1, 0, 59];

            var storageServiceMock = new Mock<ITrackingService>();
            storageServiceMock.Setup(x => x.GenerateImageAndStoreData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(transparentPixel);

            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(storageServiceMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/track");

            // Assert
            Assert.True(response.EnsureSuccessStatusCode().IsSuccessStatusCode);
            storageServiceMock.Verify(x => x.GenerateImageAndStoreData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.Equal("image/gif", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task TrackEndpoint_Should_Handle_Empty_Headers()
        {
            // Arrange
            var storageServiceMock = new Mock<ITrackingService>();
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(storageServiceMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/track");

            // Assert
            Assert.True(response.EnsureSuccessStatusCode().IsSuccessStatusCode);
            storageServiceMock.Verify(x => x.GenerateImageAndStoreData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task TrackEndpoint_Should_Handle_Exceptions()
        {
            // Arrange
            var storageServiceMock = new Mock<ITrackingService>();
            storageServiceMock.Setup(x => x.GenerateImageAndStoreData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ThrowsAsync(new Exception("Simulated exception"));

            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(storageServiceMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/track");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            storageServiceMock.Verify(x => x.GenerateImageAndStoreData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
