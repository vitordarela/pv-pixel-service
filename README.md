# pv-pixel-service

The `pv-pixel-service` is an API with a single endpoint `/track`. This endpoint captures the traffic passing through it, logging the HTTP headers `Referer`, `X-Forwarded-For`, and `User-Agent`.

As a response, it generates a transparent 1-pixel image with the `.gif` extension

## Installation

Ensure you have [.NET Core 8](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine before proceeding.

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/pv-storagedata-service.git
   ````
2. Navigate to the project directory:

   ```bash
   cd pv-storagedata-service
   ````
3. Build and run the service:

   ```bash
   dotnet build
   dotnet run
   ````

## Usage

Tracking traffic on a website can be accomplished using the following HTML snippet:
```bash
<img src="https://pixel-service/track" alt="SuperTracker Pixel" />
```

If you want to simulate the web call locally, you can use this cURL as an example.
```bash
curl --location 'https://localhost:7243/track' \
--header 'x-forwarded-for: 192.168.1.30' \
--header 'Referer: www.mywebsite3.com' \
--header 'User-Agent: PostmanRuntime/7.36.3'
```

## Result Task

This service is responsible solely for capturing traffic and sends it via gRPC Client to the [pv-storagedata-service](https://github.com/vitordarela/pv-storagedata-service), where data is processed and stored."