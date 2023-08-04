using PRoxy;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
builder.Services.AddSingleton<IForwarderHttpClientFactory, CustomHttpClientFactory>();
builder.Services.AddSingleton<BaseUrlProvider>();
builder.Services.AddReverseProxy()
    .LoadFromConfig(Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();
app.UseMiddleware<BaseUrlMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy();
});

app.Run();

