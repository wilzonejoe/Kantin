using System;

namespace Core.Extensions
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetServiceType<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
