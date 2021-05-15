using BookApi.Application;
using Microsoft.Extensions.DependencyInjection;

namespace BookApi.Infrastructure.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure();
        }
    }
}