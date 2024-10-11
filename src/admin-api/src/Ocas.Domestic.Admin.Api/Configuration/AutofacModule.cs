using Autofac;
using CacheManager.Core;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppSettings>()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register our Autofac Modules within the Services Library
            builder.RegisterModule(new Services.AutofacModule());
            builder.RegisterModule(new Apply.Services.AutofacModule());

            // Caching
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings => settings.WithSystemRuntimeCacheHandle());
            builder.RegisterGeneric(typeof(BaseCacheManager<>))
                .WithParameters(new[] { new TypedParameter(typeof(ICacheManagerConfiguration), cacheConfig) })
                .As(typeof(ICacheManager<>))
                .SingleInstance();
        }
    }
}
