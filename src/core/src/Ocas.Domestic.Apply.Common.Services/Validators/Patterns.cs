using System.Text.RegularExpressions;

namespace Ocas.Domestic.Apply.Services.Validators
{
    public static class Patterns
    {
        public static Regex MonerisCvd { get; } = new Regex(@"^(\d{1,4})$");
        public static Regex Name { get; } = new Regex(@"^[a-zA-ZœŒàÀâÂáÁçÇéÉèÈêÊëËïÏîÎíÍñÑôÔóÓùÙûÛüÜúÚÿŸ\u0027 -]+$");
        public static Regex OcasEmailAddress { get; } = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$");
        public static Regex InternationalPhoneNumberLengthRegex { get; } = new Regex(@"^(\d{10,15})$");
    }
}
