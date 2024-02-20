var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/track", (HttpContext context) =>
{
    var referer = context.Request.Headers["Referer"].ToString();
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    var ipAddress = context.Request.Headers["x-forwarded-for"].ToString();
    
    Console.WriteLine($"Referrer: {referer}, User-Agent: {userAgent}, IP Address: {ipAddress}");

    byte[] transparentPixel = [71, 73, 70, 56, 57, 97, 1, 0, 1, 0, 128, 255, 0, 255, 255, 255, 0, 0, 0, 33, 249, 4, 1, 0, 0, 0, 0, 44, 0, 0, 0, 0, 1, 0, 1, 0, 0, 2, 2, 68, 1, 0, 59];

    return Results.File(transparentPixel, "image/gif");

});

app.Run();
