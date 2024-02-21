namespace PvPixelService.DataGateway.Grpc.Storage
{
    using global::Grpc.Net.Client;
    using GrpcStorageClient;

    public class StorageClient : IStorageClient
    {
        private readonly Storage.StorageClient grpcClient;

        public StorageClient(GrpcChannel channel)
        {
            this.grpcClient = new Storage.StorageClient(channel);
        }

        public async Task<DataResponse> StoreDataAsync(DataRequest request)
        {
            return await this.grpcClient.StoreDataAsync(request);
        }
    }
}
