using Autofac;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services.Configuration;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services
{
    public class AutofacModule : Module
    {
        private static readonly MapperConfiguration _ocasMapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile { IsOcasUser = true }));

        private static readonly MapperConfiguration _nonOcasMapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile { IsOcasUser = false }));

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

            // Automapper
            builder.Register(c =>
            {
                if (!c.IsRegistered<RequestCache>())
                    return _nonOcasMapperConfig;

                c.Resolve<RequestCache>().TryGetValue(Constants.RequestCacheKeys.UserIsOcas, out var userIsOcas);
                return (bool)userIsOcas ? _ocasMapperConfig : _nonOcasMapperConfig;
            })
                .InstancePerLifetimeScope();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                    .As<IMapper>()
                    .InstancePerLifetimeScope(); // Mapper is created for the request

            builder.RegisterType<ApiMapper>()
                .As<IApiMapper>()
                .InstancePerLifetimeScope(); // ApiMapper is created for the request

            builder.RegisterType<DtoMapper>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); //DtoMapper is created for the request

            builder.RegisterType<AppSettingsExtras>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

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
        }
    }
}
