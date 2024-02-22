namespace PvPixelService.Services.Tracking
{
    public interface ITrackingService
    {
        Task<byte[]> GenerateImageAndStoreData(string referer, string userAgent, string ipAddress);
    }
}
