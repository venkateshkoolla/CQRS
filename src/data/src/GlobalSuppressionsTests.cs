using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Unit/Integration methods should be allowed to have underscores for very descriptive names")]
[assembly: SuppressMessage("Naming", "CA1308:Normalize strings to uppercase", Justification = "Ok in tests")]
[assembly: SuppressMessage("Code", "RCS1079:Implement the functionality instead of throwing new NotImplementedException", Justification = "Ok in tests")]
[assembly: SuppressMessage("Redundancy", "RCS1182", Justification = "Ok in tests")]
[assembly: SuppressMessage("Redundancy", "RCS1194", Justification = "Ok in tests")]
[assembly: SuppressMessage("Usage", "RCS1202", Justification = "Ok in tests")]
[assembly: SuppressMessage("Usage", "RCS1186", Justification = "Ok in tests")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace must be followed by blank line", Justification = "This doesn't work for List object initializer")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1500:Braces for multi-line statements must not share line", Justification = "This doesn't work for List object initializer")]
[assembly: SuppressMessage("FluentAssertionCodeSmell", "NullConditionalAssertion:Code Smell", Justification = "Ok in tests to conditional assert")]