using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BookApi.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MappingProfile(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            ApplyMappingsFromAssembly(Assembly.GetEntryAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var interfaceType = typeof(IMapFrom<>);
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                    interfaceType.IsAssignableFrom(t)))
                .ToList();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                foreach (var type in types)
                {
                    var instance = services.GetService(type);


                    var methodInfo = type.GetMethod("Mapping")
                                     ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping")
                                     ?? type.GetInterface("IMapFrom")?.GetMethod("Mapping");

                    methodInfo?.Invoke(instance, new object[] {this});
                }
            }
        }
    }
}