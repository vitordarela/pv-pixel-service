namespace PvPixelService.Services.Tracking
{
    using GrpcStorageClient;
    using PvPixelService.DataGateway.Grpc.Storage;

    public class TrackingService : ITrackingService
    {
        private readonly ILogger<TrackingService> logger;
        private readonly IStorageClient storageClient;

        public TrackingService(ILogger<TrackingService> logger, IStorageClient storageClient)
        {
            this.logger = logger;
            this.storageClient = storageClient;
        }

        public async Task<byte[]> GenerateImageAndStoreData(string referer, string userAgent, string ipAddress)
        {
            byte[] transparentPixel = [71, 73, 70, 56, 57, 97, 1, 0, 1, 0, 128, 255, 0, 255, 255, 255, 0, 0, 0, 33, 249, 4, 1, 0, 0, 0, 0, 44, 0, 0, 0, 0, 1, 0, 1, 0, 0, 2, 2, 68, 1, 0, 59];

            var message = await SaveData(referer, userAgent, ipAddress);
            this.logger.LogInformation(message);

            return transparentPixel;
        }

        private async Task<string> SaveData(string referer, string userAgent, string ipAddress)
        {
            try
            {
                var reply = await this.storageClient.StoreDataAsync(
                                  new DataRequest
                                  {
                                      IpAddress = ipAddress,
                                      Referer = referer,
                                      UserAgent = userAgent,
                                  });

                return reply.Message;

            }
            catch (Exception ex)
            {
                /* When any failure occurs in the service call, 
                 * we will always return a empty string to prevent the gif do not from rendering to the end consumer.
                */
                this.logger.LogError($"Error saving data > {ex.Message}", ex);
                return string.Empty;
            }

        }
    }
}
