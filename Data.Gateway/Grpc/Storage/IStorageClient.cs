namespace PvPixelService.DataGateway.Grpc.Storage
{
    using GrpcStorageClient;

    public interface IStorageClient
    {
        Task<DataResponse> StoreDataAsync(DataRequest request);
    }
}
