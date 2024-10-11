using Autofac;
using FluentValidation;
using Ocas.Domestic.Apply.Services.Clients;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Settings;
using Ocas.Domestic.Apply.TemplateProcessors;
using Ocas.Domestic.Data.Extras;

namespace Ocas.Domestic.Apply.Services
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

            builder.RegisterType<DomesticContextExtras>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<ApiMapperBase>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // ApiMapper is created for the request

            builder.RegisterType<DtoMapperBase>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // DtoMapper is created for the request

            builder.RegisterType<TemplateMapper>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // TemplateMapper is created for the request

            // Moneris
            builder.RegisterType<MonerisClient>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // MonerisClient is created for the request

            // EvoPdf
            builder.RegisterType<RazorTemplateService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.Register(c => new TemplateProcessorSettings(
                    c.Resolve<IAppSettingsBase>().GetAppSetting("ocas:evopdf:licenseKey"),
                    c.Resolve<IAppSettingsBase>().GetAppSetting("ocas:evopdf:serviceUrl"),
                    c.Resolve<IAppSettingsBase>().GetAppSetting("ocas:evopdf:servicePassword")))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
