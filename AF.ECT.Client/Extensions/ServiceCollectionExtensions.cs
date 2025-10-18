using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace AF.ECT.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped(sp =>
        {
            return new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            };
        });

        return services;
    }
}
