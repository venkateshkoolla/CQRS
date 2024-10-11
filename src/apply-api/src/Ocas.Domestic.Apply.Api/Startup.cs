using System.Collections.Generic;
using System.Linq;
using Autofac;
using CorrelationId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NSwag;
using NSwag.Generation.Processors.Security;
using Ocas.Domestic.Apply.Api.Configuration;
using Ocas.Domestic.Apply.Api.Filters;
using Ocas.Domestic.Apply.Api.Services.Behaviors;
using Ocas.Domestic.Apply.Core.Extensions;
using Serilog;

namespace Ocas.Domestic.Apply.Api
{
    public class Startup
    {
        static Startup()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            };
        }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            const string ocasScheme = "OcasScheme";
            const string applicantScheme = "ApplicantScheme";
            const string applyPolicy = "applyPolicy";
            const string newIdentityScheme = "NewIdentityScheme";

            var ocasRoles = Configuration.GetSection("ocas:idsvr:roles:ocas").GetChildren().Select(c => c.Value).ToList();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(applyPolicy, policy => policy
                    .AddAuthenticationSchemes(ocasScheme, applicantScheme, newIdentityScheme)
                    .RequireAuthenticatedUser()
                    .RequireScope("apply_api")
                    .RequireClaim("sub")
                    .RequireClaim("email")
                    .RequireAssertion(context =>
                    {
                        var applicantIdp = Configuration["ocas:idpvr:issuer"];
                        var identityServer = Configuration["ocas:idsvr:issuer"];
                        var ids4 = Configuration["ocas:ids4:issuer"];

                        if (context.User.GetIssuer() == identityServer)
                        {
                            switch (context.User.GetIdp())
                            {
                                case Constants.IdentityServer.Adfs.IdP:
                                    // If a user signed in through ADFS, they could be an OCAS user or a partner.
                                    // Only OCAS users are allowed in the API so verify that they have a customer code claim with the value "OCAS".
                                    // Also make sure the OCAS user belongs to one of the valid roles.
                                    return ocasRoles.Any(context.User.IsInRole);
                                default:
                                    // Either Agent or Partner (college/high school) so they are not allowed in
                                    return false;
                            }
                        }

                        if (context.User.GetIssuer() == ids4)
                        {
                            switch (context.User.GetIdp())
                            {
                                case Constants.IdS4.Adfs.IdP:
                                    // If a user signed in through ADFS, they could be an OCAS user or a partner.
                                    // Only OCAS users are allowed in the API so verify that they have a customer code claim with the value "OCAS".
                                    // Also make sure the OCAS user belongs to one of the valid roles.
                                    return ocasRoles.Any(context.User.IsInRole);
                                case Constants.IdS4.Applicants.Idp:
                                    // Applicants allowed in
                                    return true;
                                default:
                                    // Partner (college/high school) so they are not allowed in
                                    return false;
                            }
                        }

                        // if they signed in through ApplicantIdp they are an applicant, so let them in
                        return context.User.GetIssuer() == applicantIdp;
                    }));
            });

            services.AddAuthentication()
                .AddIdentityServerAuthentication(ocasScheme, options => options.Authority = Configuration["ocas:idsvr:authority"])
                .AddIdentityServerAuthentication(applicantScheme, options => options.Authority = Configuration["ocas:idpvr:authority"])
                .AddIdentityServerAuthentication(newIdentityScheme, options => options.Authority = Configuration["ocas:ids4:authority"]);

            // MVC
            services
                .AddRouting(x => x.LowercaseUrls = true)
                .AddMvc(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(applyPolicy));
                    options.Filters.Add(typeof(OcasLocaleFilter));
                    options.Filters.Add(typeof(OcasPartnerFilter));
                    options.AddDashedRouting();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.Formatting = Formatting.None;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                });

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddCorrelationId();

            services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(UserAuthorizationBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
                .AddMediatR(typeof(Services.AutofacModule), typeof(Apply.Services.AutofacModule));

            services.AddOpenApiDocument(document =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Copy 'Bearer ' + valid JWT token into field",
                    In = OpenApiSecurityApiKeyLocation.Header
                };

                document.AddSecurity("JWT Token", Enumerable.Empty<string>(), securityScheme);
                document.OperationProcessors.Add(new AcceptLanguageSwaggerProcessor());
                document.OperationProcessors.Add(new PartnerSwaggerProcessor());
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                document.Title = HostingEnvironment.ApplicationName;
                document.AllowReferencesWithProperties = true;
            });
            
            services.AddCacheManagerConfiguration(Configuration);

            services.AddCacheManager();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app
                .UseCorrelationId(new CorrelationIdOptions { UseGuidForCorrelationId = true, UpdateTraceIdentifier = false })
                .UseOcasEnrichLogContext()
                .UseOcasEnrichSerilogHttpContext()
                .UseOcasHttpRequestBodyLogging(new OcasHttpRequestBodyLoggingOptions
                {
                    MaximumRecordedRequestLength = 1000000,
                    ExclusionPaths = new[] { "/swagger/" }
                })
                .UseOcasExceptionHandler() // Global exception handler, Formats the response to our specific JSON structure, and logs exception
                .UseMvc();

            app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
            app.UseSwaggerUi3(); // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

            lifetime.ApplicationStopping.Register(Log.CloseAndFlush);
        }
    }
}
