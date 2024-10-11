using Autofac;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Caches;
using Ocas.Domestic.Apply.Api.Services.Clients;
using Ocas.Domestic.Apply.Api.Services.Configuration;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Fluent Validators
            builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Caching
            builder.RegisterType<LookupsCache>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestCache>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TranslationsCache>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Authorization
            builder.RegisterType<UserAuthorization>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Data
            builder.Register(c => new DomesticContext(
                    new DomesticContextConfig(
                        c.Resolve<IAppSettings>().GetAppSetting("ocas:crm:service:connection"),
                        c.Resolve<IAppSettings>().GetAppSetting("ocas:crm:service:url"),
                        c.Resolve<IAppSettings>().GetAppSetting("ocas:crm:extras"),
                        c.Resolve<IAppSettings>().GetAppSettingOrDefault("ocas:commandTimeout", 60)),
                        new LoggerFactory().CreateLogger(nameof(DomesticContext))))
                .As<IDomesticContext>()
                .InstancePerLifetimeScope();

            builder.Register(c =>
                new ColtraneBdsProvider(
                    c.Resolve<IAppSettings>().GetAppSetting("ocas:coltrane:bds"),
                    c.Resolve<IAppSettings>().GetAppSettingOrDefault("ocas:commandTimeout", 60)))
                .As<IColtraneBdsProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AppSettingsExtras>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // Automapper
            builder.Register(_ => new MapperConfiguration(cfg => cfg.AddMaps(typeof(AutofacModule).Assembly)))
                .SingleInstance(); // Stateless, so singleton is okay

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope(); // Mapper is created for the request

            builder.RegisterType<ApiMapper>()
                .As<IApiMapper>()
                .InstancePerLifetimeScope(); // ApiMapper is created for the request

            //Dto Mapper
            builder.RegisterType<DtoMapper>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); //DtoMapper is created for the request

            // Osap
            builder.RegisterType<OsapClient>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
