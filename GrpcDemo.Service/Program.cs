using GrpcDemo.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Load CORS settings from appsettings.json
var corsOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add gRPC services to the container
builder.Services.AddGrpc();

var app = builder.Build();

// Use CORS
app.UseCors("AllowBlazorClient");

app.UseGrpcWeb(); // Enable gRPC-Web support

// Map gRPC services and enable gRPC-Web
app.MapGrpcService<GreeterService>().EnableGrpcWeb();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
