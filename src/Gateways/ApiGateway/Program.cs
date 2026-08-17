var builder = WebApplication.CreateBuilder(args);

// Add YARP Reverse Proxy services and load configuration from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

// Enable reverse proxy endpoints
app.MapReverseProxy();

app.Run();