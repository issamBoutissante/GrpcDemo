using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcDemo.Shared;

namespace GrpcDemo.Common
{
    public static class GrpcService
    {
        public static Greeter.GreeterClient GetGreeterClient()
        {
            GrpcChannel channel;

            if (OperatingSystem.IsBrowser())
            {
                // Blazor WebAssembly (browser) uses gRPC-Web
                channel = GrpcChannel.ForAddress("https://localhost:7259", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
                });
            }
            else
            {
                // Native apps (WPF) use standard gRPC over HTTP/2
                channel = GrpcChannel.ForAddress("https://localhost:7259");
            }

            // Return the gRPC client
            return new Greeter.GreeterClient(channel);
        }
    }
}
