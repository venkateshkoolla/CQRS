using FluentValidation.Validators;

namespace Ocas.Domestic.Apply.Api.Services.Validators
{
    internal class OenCheckSumValidator : PropertyValidator
    {
        private readonly int[] _mainArray = new int[100]
        {
            0, 2, 4, 6, 8, 1, 3, 5, 7,
            9, 1, 3, 5, 7, 9, 2, 4, 6,
            8, 0, 2, 4, 6, 8, 0, 3, 5,
            7, 9, 1, 3, 5, 7, 9, 1, 4,
            6, 8, 0, 2, 4, 6, 8, 0, 2,
            5, 7, 9, 1, 3, 5, 7, 9, 1,
            3, 6, 8, 0, 2, 4, 6, 8, 0,
            2, 4, 7, 9, 1, 3, 5, 7, 9,
            1, 3, 5, 8, 0, 2, 4, 6, 8,
            0, 2, 4, 6, 9, 1, 3, 5, 7,
            9, 1, 3, 5, 7, 0, 2, 4, 6, 8
        };

        public OenCheckSumValidator()
            : base("'{PropertyName}' must match checksum.")
        {
        }

        /// <summary>
        /// Performs a validation on supplied OntarioEducationNumber based on the algorithm given in UC-206.
        /// </summary>
        /// <param name="context"></param>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var oen = context.PropertyValue as string;

            int oenCheckSum;
            int matchIndex1;
            int matchIndex2;
            int matchIndex3;
            int matchIndex4;

            try
            {
                oenCheckSum = int.Parse(oen.Substring(oen.Length - 1));
                matchIndex1 = int.Parse(oen.Substring(0, 2));
                matchIndex2 = int.Parse(oen.Substring(2, 2));
                matchIndex3 = int.Parse(oen.Substring(4, 2));
                matchIndex4 = int.Parse(oen.Substring(6, 2));
            }
            catch
            {
                return false;
            }

            var maskTotal = _mainArray[matchIndex1] + _mainArray[matchIndex2] + _mainArray[matchIndex3] + _mainArray[matchIndex4];
            var checkSum = (10 - (maskTotal % 10)) % 10;

            return oenCheckSum == checkSum;
        }
    }
}
