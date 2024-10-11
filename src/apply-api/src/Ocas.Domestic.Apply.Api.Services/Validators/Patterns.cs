using System.Text.RegularExpressions;

namespace Ocas.Domestic.Apply.Api.Services.Validators
{
    internal static class Patterns
    {
        internal static Regex Iso8859 { get; } = new Regex(@"[\u0020-\u007e\u00a0-\u00ff\u0152\u0153\u0178]");
        internal static Regex Name { get; } = new Regex(@"^[a-zA-ZœŒàÀâÂáÁçÇéÉèÈêÊëËïÏîÎíÍñÑôÔóÓùÙûÛüÜúÚÿŸ\u0027 -]+$");
        internal static Regex NorthAmericanPhoneNumberLengthRegex { get; } = new Regex(@"^(\d{10})$");
        internal static Regex OntarioEducationNumber { get; } = new Regex(@"^(\d{9})$");
        internal static Regex PostalCode { get; } = new Regex("^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$");
        internal static Regex StudentNumber { get; } = new Regex("^[a-zA-Z0-9]+$");
        internal static Regex ZipCode { get; } = new Regex("(^[0-9]{5}$)|(^[0-9]{5}-[0-9]{4}$)");
    }
}
