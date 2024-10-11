// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Configuration property can not be passed as an argument in configuring services", Scope = "member", Target = "~M:Ocas.Domestic.Admin.Api.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "We need to log the key instead of generic configuration property name", Scope = "member", Target = "~M:Ocas.Domestic.Admin.Api.AppSettings.#ctor(Microsoft.Extensions.Configuration.IConfiguration)")]
