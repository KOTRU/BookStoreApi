using System.Reflection;
using AutoMapper;
using BookApi.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapMethod = _ => false;
                cfg.AddProfile(new MappingProfile(provider.GetService<IServiceScopeFactory>()));
            }).CreateMapper(provider.GetService));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }

}