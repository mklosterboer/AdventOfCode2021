namespace AdventOfCode2021.Problems
{
    internal class SignalByVisual
    {
        private const int OneLength = 2;
        private const int FourLength = 4;
        private const int SevenLength = 3;
        private const int EightLength = 7;

        private char B { get; set; }
        private char C { get; set; }
        private char E { get; set; }
        private char F { get; set; }

        private List<string> SignalPatterns { get; set; }
        private List<string> Outputs { get; set; }

        public SignalByVisual(string inputValue)
        {
            var splitInput = inputValue.Split(" | ");

            SignalPatterns = splitInput.ElementAt(0).Trim().Split(' ').ToList();

            Outputs = splitInput.ElementAt(1).Trim().Split(' ').ToList();

            SetCharactersFromSignalPatterns();
        }

        public int GetTranslatedOutputValue()
        {
            string newOutputValue = "";

            foreach (var output in Outputs)
            {
                if (IsZero(output))
                {
                    newOutputValue += "0";
                }
                else if (IsOne(output))
                {
                    newOutputValue += "1";
                }
                else if (IsTwo(output))
                {

                    newOutputValue += "2";
                }
                else if (IsThree(output))
                {

                    newOutputValue += "3";
                }
                else if (IsFour(output))
                {

                    newOutputValue += "4";
                }
                else if (IsFive(output))
                {

                    newOutputValue += "5";
                }
                else if (IsSix(output))
                {

                    newOutputValue += "6";
                }
                else if (IsSeven(output))
                {

                    newOutputValue += "7";
                }
                else if (IsEight(output))
                {

                    newOutputValue += "8";
                }
                else if (IsNine(output))
                {

                    newOutputValue += "9";
                }
            }

            return int.Parse(newOutputValue);
        }


        private void SetCharactersFromSignalPatterns()
        {
            // Assumption: SignalPatterns will always contain all numbers
            //  This was true of my data set but was not explicitly stated in requirements

            /* If we know we have all 10 numbers, we can count the number of times each
              segment shows up in the 10 digits and use that to identify the Letters.

              Number of times each segment appears;
                E - 4

                B - 6

                D - 7
                G - 7

                A - 8
                C - 8

                F - 9
             */

            // This still seems to be slower than the SignalByLetters version,
            // but I genuinely don't know why...

            var countOfLetters = SignalPatterns
                .SelectMany(x => x)
                .GroupBy(x => x)
                .Select(x => new
                {
                    Value = x.Key,
                    Count = x.Count()
                })
                .ToList();

            // These segments show a unique amount of times.
            E = countOfLetters.Where(x => x.Count == 4).Select(x => x.Value).Single();
            B = countOfLetters.Where(x => x.Count == 6).Select(x => x.Value).Single();
            F = countOfLetters.Where(x => x.Count == 9).Select(x => x.Value).Single();

            // Find C
            /// One is a unique length.
            var foundOne = SignalPatterns.Single(x => x.Length is OneLength);

            /// One is "cf", so remove "F" to find "C".
            C = ReplaceAllOtherCharacters(foundOne, $"{F}");

            // We don't actually need all the letter to deduce the output values
        }

        private bool IsTwo(string input) => input.Length == 5 && !input.Contains(F);
        private bool IsThree(string input) => input.Length == 5 && input.Contains(C) && input.Contains(F);
        private bool IsFive(string input) => input.Length == 5 && input.Contains(B);

        private bool IsZero(string input) => input.Length == 6 && input.Contains(C) && input.Contains(E);
        private bool IsSix(string input) => input.Length == 6 && !input.Contains(C) && input.Contains(E);
        private bool IsNine(string input) => input.Length == 6 && !input.Contains(E);

        private static bool IsOne(string input) => input.Length is OneLength;
        private static bool IsFour(string input) => input.Length is FourLength;
        private static bool IsSeven(string input) => input.Length is SevenLength;
        private static bool IsEight(string input) => input.Length is EightLength;

        private static char ReplaceAllOtherCharacters(string stringToFilter, string filterString)
        {
            var newString = stringToFilter;

            foreach (char character in filterString)
            {
                newString = newString.Replace(character.ToString(), "");
            }

            return newString.ToCharArray().Single();

        }
    }
}
