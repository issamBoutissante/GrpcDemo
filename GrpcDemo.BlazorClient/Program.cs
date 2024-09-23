using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GrpcDemo.BlazorClient;
using Grpc.Net.Client.Web;
using GrpcDemo.Service;
using GrpcDemo.BlazorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add gRPC-Web client services
builder.Services.AddScoped(sp => GrpcWebService.GetGrpcClient());
// Register the gRPC client
builder.Services.AddSingleton(services =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var backendUrl = "https://localhost:5001"; // Replace with your gRPC server URL
    var channel = GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpClient = httpClient });
    return new Greeter.GreeterClient(channel);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
