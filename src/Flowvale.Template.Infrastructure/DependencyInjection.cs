using Flowvale.Template.Application.Repositories;
using Flowvale.Template.Infrastructure.Clients;
using Flowvale.Template.Infrastructure.Interfaces.Clients;
using Flowvale.Template.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flowvale.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpClient<IWolneLektury, WolneLektury>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:WolneLektury:BaseUrl"] ?? "https://wolnelektury.pl/api");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<ILibraryRepository, LibraryRepository>();

        return services;
    }
}
