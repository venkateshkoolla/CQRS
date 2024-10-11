using System.Text.RegularExpressions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators
{
    internal static class Patterns
    {
        internal static Regex AccountNumber { get; } = new Regex(@"^(\d{1,12})$");
        internal static Regex ApplicationNumber { get; } = new Regex(@"^(\d{8,9})$");
        internal static Regex Name { get; } = new Regex(@"^[a-zA-ZœŒàÀâÂáÁçÇéÉèÈêÊëËïÏîÎíÍñÑôÔóÓùÙûÛüÜúÚÿŸ\u0027 -]+$");
        internal static Regex OntarioEducationNumber { get; } = new Regex(@"^(\d{9})$");
    }
}
