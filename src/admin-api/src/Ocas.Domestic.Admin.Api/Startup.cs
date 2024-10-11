using System;
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
using Ocas.Domestic.Apply.Admin.Api.Configuration;
using Ocas.Domestic.Apply.Admin.Api.Filters;
using Ocas.Domestic.Apply.Admin.Api.Services.Behaviors;
using Ocas.Domestic.Apply.Admin.Services.Behavior;
using Ocas.Domestic.Apply.Core.Extensions;
using Serilog;

namespace Ocas.Domestic.Apply.Admin.Api
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

            if (env.EnvironmentName.Equals("tests", StringComparison.OrdinalIgnoreCase)) SerilogBootstrapper.Initialize(Configuration);
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            const string adfsScheme = "AdfsScheme";
            const string newAdfsScheme = "NewAdfsScheme";
            const string adminPolicy = "adminPolicy";

            #pragma warning disable CA2208 // Instantiate argument exceptions correctly
            var ocasRoles = Configuration.GetSection("ocas:idsvr:roles:ocas").GetChildren().Select(c => c.Value).ToList();
            if (ocasRoles.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:ocas");

            var collegeRoles = Configuration.GetSection("ocas:idsvr:roles:partnerCollegeUser").GetChildren().Select(c => c.Value).ToList();
            if (collegeRoles.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerCollegeUser");

            var highSchoolRoles = Configuration.GetSection("ocas:idsvr:roles:partnerHighSchoolUser").GetChildren().Select(c => c.Value).ToList();
            if (highSchoolRoles.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerHighSchoolUser");

            var hsBoardRoles = Configuration.GetSection("ocas:idsvr:roles:partnerHSBoardUser").GetChildren().Select(c => c.Value).ToList();
            if (hsBoardRoles.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerHSBoardUser");
            #pragma warning restore CA2208 // Instantiate argument exceptions correctly

            var partnerRoles = collegeRoles.Union(highSchoolRoles).Union(hsBoardRoles);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(adminPolicy, policy => policy
                    .AddAuthenticationSchemes(adfsScheme, newAdfsScheme)
                    .RequireAuthenticatedUser()
                    .RequireScope("applyadmin_api")
                    .RequireClaim("sub")
                    .RequireClaim("email")
                    .RequireAssertion(context =>
                    {
                        var identityServer = Configuration["ocas:idsvr:issuer"];
                        var ids4 = Configuration["ocas:ids4:issuer"];

                        if (context.User.GetIssuer() == identityServer)
                        {
                            switch (context.User.GetIdp())
                            {
                                case Constants.IdentityServer.Adfs.IdP:
                                    return ocasRoles.Any(role => context.User.IsInRole(role));
                                case Constants.IdentityServer.Partner.IdP:
                                    return partnerRoles.Any(role => context.User.IsInRole(role));
                                default:
                                    return false;
                            }
                        }

                        if (context.User.GetIssuer() == ids4)
                        {
                            switch (context.User.GetIdp())
                            {
                                case Constants.IdS4.Adfs.IdP:
                                    return ocasRoles.Any(role => context.User.IsInRole(role));
                                case Constants.IdS4.Partner.IdP:
                                    return partnerRoles.Any(role => context.User.IsInRole(role));
                                default:
                                    return false;
                            }
                        }

                        // if you get this far you are not allowed in
                        return false;
                    }));
            });

            services.AddAuthentication()
                .AddIdentityServerAuthentication(adfsScheme, options => options.Authority = Configuration["ocas:idsvr:authority"])
                .AddIdentityServerAuthentication(newAdfsScheme, options => options.Authority = Configuration["ocas:ids4:authority"]);

            // MVC
            services
                .AddRouting(x => x.LowercaseUrls = true)
                .AddMvc(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(adminPolicy));
                    options.Filters.Add(typeof(OcasLocaleFilter));
                    options.Filters.Add(typeof(UserFilter));
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
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(CollegeUserAuthorizationBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ApplicantUserAuthorizationBehavior<,>))
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
