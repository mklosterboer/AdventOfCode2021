using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day08 : Problem
    {
        protected override string ProblemName => "Day08";
        private string[] Input { get; set; }

        private const int OneLength = 2;
        private const int FourLength = 4;
        private const int SevenLength = 3;
        private const int EightLength = 7;

        public Day08()
        {
            Input = GetInputValue();
        }

        public override object PartOne()
        {
            return Input
                .Select(x => x
                    .Split(" | ")
                    .ElementAt(1)
                    .Trim()
                    .Split(' '))
                .SelectMany(x => x)
                .Count(o => o.Length is
                    OneLength
                    or FourLength
                    or SevenLength
                    or EightLength);
        }

        public override object PartTwo()
        {
            return Input
                .Select(x => new Signal(x))
                .Sum(s => s.GetTranslatedOutputValue());
        }
    }

    internal class Signal
    {
        private const int OneLength = 2;
        private const int FourLength = 4;
        private const int SevenLength = 3;
        private const int EightLength = 7;

        private char A { get; set; }
        private char B { get; set; }
        private char C { get; set; }
        private char D { get; set; }
        private char E { get; set; }
        private char F { get; set; }
        private char G { get; set; }

        private List<string> SignalPatterns { get; set; }
        private List<string> Outputs { get; set; }

        public Signal(string inputValue)
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

            // Find A
            /// One is "cf"
            var foundOne = SignalPatterns.Where(x => x.Length is OneLength).First();
            /// Seven is "acf"
            var foundSeven = SignalPatterns.Where(x => x.Length is SevenLength).First();

            /// Remove "c" and "f" to find "a" 
            A = ReplaceAllOtherCharacters(foundSeven, foundOne);

            // Find B
            /// Three is "acdfg". It is 5 digits and contains all the character of Seven
            var foundThree = FilterSignals(foundSeven, 5);

            /// Nine is "abcdfg". It contains all the the same characters as Three, except for "b"
            var foundNine = FilterSignals(foundThree, 6);

            /// Remove "A", "C", "D", "F", "G" to find "B"
            B = ReplaceAllOtherCharacters(foundNine, foundThree);

            // Find D
            /// Four is "bcdf".
            var foundFour = SignalPatterns.Where(x => x.Length is FourLength).First();

            /// Remove "B", "C", "F" to find "D"
            D = ReplaceAllOtherCharacters(foundFour, $"{B}${foundOne}");

            // Find G
            /// Remove "A", "C", "D", "F" to find "G"
            G = ReplaceAllOtherCharacters(foundThree, $"{A}{foundOne}{D}");

            // Find F
            /// Five is "abdfg". It contains "B" and is 5 characters
            var foundFive = FilterSignals($"{B}", 5);

            /// Remove "A", "B", "D", "G" to find "F"
            F = ReplaceAllOtherCharacters(foundFive, $"{A}{B}{D}{G}");

            // Find C
            /// Remove "F" to find "C"
            C = ReplaceAllOtherCharacters(foundOne, $"{F}");

            // Find E
            /// Eight is "abcdefg"
            var foundEight = SignalPatterns.Where(x => x.Length is EightLength).First();

            /// Eight contains all characters, so replace everything but "E"
            E = ReplaceAllOtherCharacters(foundEight, $"{A}{B}{C}{D}{F}{G}");
        }

        private bool IsTwo(string input) => input.Length == 5 && input.Contains(C) && input.Contains(E);
        private bool IsThree(string input) => input.Length == 5 && input.Contains(C) && input.Contains(F);
        private bool IsFive(string input) => input.Length == 5 && input.Contains(B);

        private bool IsZero(string input) => input.Length == 6 && input.Contains(C) && input.Contains(E);
        private bool IsSix(string input) => input.Length == 6 && input.Contains(D) && input.Contains(E);
        private bool IsNine(string input) => input.Length == 6 && input.Contains(C) && input.Contains(D);

        private static bool IsOne(string input) => input.Length is OneLength;
        private static bool IsFour(string input) => input.Length is FourLength;
        private static bool IsSeven(string input) => input.Length is SevenLength;
        private static bool IsEight(string input) => input.Length is EightLength;


        private string FilterSignals(string fiterString, int filterLength)
        {
            var newStringList = SignalPatterns.Where(x => x.Length == filterLength);

            foreach (char character in fiterString)
            {
                newStringList = newStringList.Where(x => x.Contains(character));
            }

            return newStringList.Single();
        }

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
