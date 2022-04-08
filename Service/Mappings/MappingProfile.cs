using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => !i.IsGenericType && i == typeof(IResourceMapper)))
                .ForAll(t =>
                {
                    var instance = Activator.CreateInstance(t);
                    var methodInfo = t.GetMethod("Mapping");
                    methodInfo?.Invoke(instance, new object[] { this });
                }
                );
        }
    }
}
