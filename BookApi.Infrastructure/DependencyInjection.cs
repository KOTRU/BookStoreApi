using System.IO;
using BookApi.Application.Common.Database;
using BookApi.Application.Options;
using BookApi.Infrastructure.Presistance;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BookApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IDbContext, FireBaseDatabase>();

            var path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            using (var stream =
                new FileStream($"{path}/settings.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var settings = JsonSerializer.Create().Deserialize<Settings>(new JsonTextReader(reader));
                    services.AddTransient(s => settings.ApiKey);
                }
            }

            return services;
        }
    }
}