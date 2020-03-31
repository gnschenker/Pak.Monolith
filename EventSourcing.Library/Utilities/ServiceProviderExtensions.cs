using System;

namespace EventSourcing.Library.Utilities
{
    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider provider) where T : class
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
