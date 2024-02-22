using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using GrpcStorageClient;
using PvPixelService.DataGateway.Grpc.Storage;

namespace PvPixelService.Services.Tracking.Tests
{
    public class TrackingServiceTests
    {
        [Fact]
        public async Task GenerateImageAndStoreData_ReturnsTransparentPixel()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TrackingService>>();
            var storageClientMock = new Mock<IStorageClient>();
            var referer = "www.mywebsite.com";
            var userAgent = "Test/1.0.0";
            var ipAddress = "192.168.1.1";

            var trackingService = new TrackingService(loggerMock.Object, storageClientMock.Object);

            // Act
            var result = await trackingService.GenerateImageAndStoreData(referer, userAgent, ipAddress);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GenerateImageAndStoreData_CallsStorageClient()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TrackingService>>();
            var storageClientMock = new Mock<IStorageClient>();
            var referer = "www.mywebsite.com";
            var userAgent = "Test/1.0.0";
            var ipAddress = "192.168.1.1";

            var trackingService = new TrackingService(loggerMock.Object, storageClientMock.Object);

            // Act
            await trackingService.GenerateImageAndStoreData(referer, userAgent, ipAddress);

            // Assert
            storageClientMock.Verify(mock => mock.StoreDataAsync(It.IsAny<DataRequest>()), Times.Once);
        }
    }
}
