using Grpc.Net.Client;
using PvPixelService.DataGateway.Grpc.Storage;
using PvPixelService.Services.Tracking;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IStorageClient>(provider =>
{
    var channel = GrpcChannel.ForAddress(builder.Configuration["GrpcServerHost"]);
    return new StorageClient(channel);
});

builder.Services.AddScoped<ITrackingService, TrackingService>();

var app = builder.Build();

app.MapGet("/track", async (HttpContext context, ILogger <Program> logger, ITrackingService trackingService) =>
{
    // In the documentation it says Referrer but the default HTTP Header is Referer, I identified it as if it were a typo and left the correct default. 
    var referer = context.Request.Headers["Referer"].ToString();
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    var ipAddress = context.Request.Headers["x-forwarded-for"].ToString();

    logger.LogInformation($"Referrer: {referer}, User-Agent: {userAgent}, IP Address: {ipAddress}");

    var imageGeneration = await trackingService.GenerateImageAndStoreData(referer, userAgent, ipAddress).ConfigureAwait(false);

    return Results.File(imageGeneration, "image/gif");

});

app.Run();

public partial class Program { }