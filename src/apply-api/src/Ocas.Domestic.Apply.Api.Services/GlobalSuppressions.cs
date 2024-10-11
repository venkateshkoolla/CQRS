// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Relax Roslyn, I got this")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("	Microsoft.Security", "CA3075:Insecure DTD Processing", Justification = "The XML is from a trusted source", Scope = "type", Target = "~T:Ocas.Domestic.Apply.Api.Services.Handlers.GetSupportingDocumentFileHandler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "Not culture specific string and can not ignore the case", Scope = "member", Target = "~M:Ocas.Domestic.Apply.Api.Services.Handlers.GetApplicantHistoriesHandler.Handle(Ocas.Domestic.Apply.Core.Messages.GetApplicantHistories,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Collections.Generic.IList{Ocas.Domestic.Apply.Models.ApplicantHistory}}")]
