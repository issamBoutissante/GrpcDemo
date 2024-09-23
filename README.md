# GrpcDemo

This repository demonstrates how to use a **gRPC client** to communicate with a **gRPC server** from both a **WPF native application** and a **Blazor WebAssembly** client. The project uses a common service library to handle the gRPC communication logic, avoiding code duplication across both platforms.

## Project Structure

- **GrpcDemo.Service**: The gRPC server project, which hosts the gRPC service and responds to client requests. It also enables gRPC-Web for browser-based communication.
- **GrpcDemo.Shared**: Contains the shared `.proto` file that defines the gRPC services and message types. This project generates client and server code.
- **GrpcDemo.Common**: A common library that provides a shared `GrpcService` class, which handles gRPC client creation for both WPF and Blazor.
- **GrpcDemo.WpfClient**: The WPF client that communicates with the gRPC server using the common gRPC service.
- **GrpcDemo.BlazorClient**: The Blazor WebAssembly client that communicates with the gRPC server using gRPC-Web, also using the common gRPC service.

## Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio or VS Code
- gRPC installed (included with .NET SDK)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/GrpcDemo.git
   cd GrpcDemo
   ```

2. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

3. Open the solution in Visual Studio:

   ```bash
   GrpcDemo.sln
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

### Running the gRPC Server

1. Navigate to the `GrpcDemo.Service` project.
2. Run the server:
   ```bash
   dotnet run
   ```

The gRPC server will start and listen on `https://localhost:7259`. It also supports gRPC-Web for browser-based clients.

### Running the WPF Client

1. Navigate to the `GrpcDemo.WpfClient` project.
2. Run the WPF client:

   ```bash
   dotnet run
   ```

3. Enter a name in the TextBox, click the "Call gRPC Server" button, and the response from the gRPC server will be shown in the Label below the button.

### Running the Blazor WebAssembly Client

1. Navigate to the `GrpcDemo.BlazorClient` project.
2. Run the Blazor client:

   ```bash
   dotnet run
   ```

3. Open a browser and navigate to `https://localhost:7051` (replace the port if necessary). Enter a name in the input field, and click "Call gRPC Server". The response from the gRPC server will be displayed below the button.

## gRPC Client - Shared Service

The `GrpcDemo.Common` project contains the shared service used by both the WPF and Blazor clients. This service creates the gRPC client, selecting the appropriate transport (gRPC-Web for Blazor, gRPC for WPF) based on the runtime environment.

### Example Code (Shared gRPC Service)

```csharp
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcDemo.Shared;
using System;
using System.Net.Http;

namespace GrpcDemo.Common
{
    public static class GrpcService
    {
        public static Greeter.GreeterClient GetGreeterClient()
        {
            GrpcChannel channel;

            if (OperatingSystem.IsBrowser())
            {
                // For Blazor WebAssembly, use gRPC-Web
                channel = GrpcChannel.ForAddress("https://localhost:7259", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
                });
            }
            else
            {
                // For WPF and other native clients, use standard gRPC
                channel = GrpcChannel.ForAddress("https://localhost:7259");
            }

            return new Greeter.GreeterClient(channel);
        }
    }
}
```

## CORS and gRPC-Web Setup

To enable gRPC-Web for Blazor WebAssembly, the gRPC server must be configured to support CORS and gRPC-Web. This is configured in the `GrpcDemo.Service` project in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7051")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddGrpc();

var app = builder.Build();

app.UseCors("AllowBlazorClient");
app.UseGrpcWeb();

app.MapGrpcService<GreeterService>().EnableGrpcWeb();
app.MapGet("/", () => "gRPC server is running.");

app.Run();
```

This setup ensures that the gRPC server can handle requests from both the WPF and Blazor clients.

## Conclusion

With this project setup, you can run both a native WPF application and a Blazor WebAssembly application that communicate with the same gRPC server. The `GrpcService` class in the shared library ensures that the gRPC client is created correctly for each environment.

Feel free to modify the project and expand it to fit your use case!
