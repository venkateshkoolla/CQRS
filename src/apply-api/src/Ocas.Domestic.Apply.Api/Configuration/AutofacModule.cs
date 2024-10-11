using Autofac;

namespace Ocas.Domestic.Apply.Api
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
        }
    }
}
