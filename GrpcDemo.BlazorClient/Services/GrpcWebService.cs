using Grpc.Net.Client;
using Grpc.Net.Client.Web;

using GrpcDemo.Shared;

namespace GrpcDemo.BlazorClient.Services;

public static class GrpcWebService
{
    public static Greeter.GreeterClient GetGrpcClient()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7259", new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
        });

        return new Greeter.GreeterClient(channel);
    }
}
