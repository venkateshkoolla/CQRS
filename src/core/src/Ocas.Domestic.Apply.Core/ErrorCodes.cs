namespace Ocas.Domestic.Apply.Core
{
    public static class ErrorCodes
    {
        public static class General // E00**
        {
            public const string UnknownError = "E0001";
            public const string ConcurrencyError = "E0010";
            public const string ConcurrencyVersionMismatchError = "E0011";
            public const string ConflictError = "E0020";
            public const string ConflictVerificationError = "E0021";
            public const string NotFoundError = "E0030";
            public const string ValidationError = "E0040";
            public const string UnauthorizedError = "E0050";
            public const string ForbiddenError = "E0055";
            public const string MediatorError = "E0060";
        }

        public static class Applicant // A0***
        {
            public const string IncompleteError = "A0401";
        }
    }
}
