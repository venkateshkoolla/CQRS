using Bogus;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class OenExtension
    {
        private static readonly int[] _mainArray = new int[100]
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

        /// <summary>
        /// Ontario Education Number for Canada
        /// </summary>
        public static string Oen()
        {
            var r = new Randomizer();
            var numbers = r.Digits(8);

            var matchIndex1 = Concat(numbers[0], numbers[1]);
            var matchIndex2 = Concat(numbers[2], numbers[3]);
            var matchIndex3 = Concat(numbers[4], numbers[5]);
            var matchIndex4 = Concat(numbers[6], numbers[7]);

            var maskTotal = _mainArray[matchIndex1] + _mainArray[matchIndex2] + _mainArray[matchIndex3] + _mainArray[matchIndex4];
            var checkSum = (10 - (maskTotal % 10)) % 10;

            return $"{numbers[0]}{numbers[1]}{numbers[2]}{numbers[3]}{numbers[4]}{numbers[5]}{numbers[6]}{numbers[7]}{checkSum}";
        }

        private static int Concat(int a, int b)
        {
            const int pow = 10;
            return (a * pow) + b;
        }
    }
}
