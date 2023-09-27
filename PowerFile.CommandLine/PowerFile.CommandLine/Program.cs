using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerFile.CommandLine;

return await RunAsync(args);

async Task<int> RunAsync(string[] args)
{
    var builder = Host.CreateApplicationBuilder();
    var services = builder.Services;

    services.AddApplication();
    var host = builder.Build();

    var app = host.Services.GetRequiredService<IPowerFileCommandLineApplication>();

    return await app.RunAsync(args);
}