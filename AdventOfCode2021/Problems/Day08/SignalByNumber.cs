namespace AdventOfCode2021.Problems
{
    internal class SignalByNumber
    {
        private string Zero { get; set; }
        private string One { get; set; }
        private string Two { get; set; }
        private string Three { get; set; }
        private string Four { get; set; }
        private string Five { get; set; }
        private string Six { get; set; }
        private string Seven { get; set; }
        private string Eight { get; set; }
        private string Nine { get; set; }

        private List<string> SignalPatterns { get; set; }
        private List<string> Outputs { get; set; }

        public SignalByNumber(string inputValue)
        {
            var splitInput = inputValue.Split(" | ");

            SignalPatterns = splitInput.ElementAt(0).Trim().Split(' ').ToList();

            Outputs = splitInput.ElementAt(1).Trim().Split(' ').ToList();

            SetNumbersFromSignalPatterns();
        }

        public int GetTranslatedOutputValue()
        {
            string newOutputValue = "";

            foreach (var output in Outputs)
            {
                if (IsEqual(output, Zero))
                {
                    newOutputValue += "0";
                }
                else if (IsEqual(output, One))
                {
                    newOutputValue += "1";
                }
                else if (IsEqual(output, Two))
                {

                    newOutputValue += "2";
                }
                else if (IsEqual(output, Three))
                {

                    newOutputValue += "3";
                }
                else if (IsEqual(output, Four))
                {

                    newOutputValue += "4";
                }
                else if (IsEqual(output, Five))
                {

                    newOutputValue += "5";
                }
                else if (IsEqual(output, Six))
                {

                    newOutputValue += "6";
                }
                else if (IsEqual(output, Seven))
                {

                    newOutputValue += "7";
                }
                else if (IsEqual(output, Eight))
                {

                    newOutputValue += "8";
                }
                else if (IsEqual(output, Nine))
                {

                    newOutputValue += "9";
                }
            }

            return int.Parse(newOutputValue);
        }

        private void SetNumbersFromSignalPatterns()
        {
            // Assumption: SignalPatterns will always contain all numbers
            //  This was true of my data set but was not explicitly stated in requirements

            /// Seven is "acf"
            Seven = SignalPatterns.Single(x => x.Length is 3);

            /// Eight is "abcdefg"
            Eight = SignalPatterns.Single(x => x.Length is 7);

            /// One is "cf"
            One = SignalPatterns.Single(x => x.Length is 2);

            /// Six is "abdefg". it is 6 characters and differs from one by 5 characters.
            Six = SignalPatterns.Single(x => x.Length == 6 && GetDifferenceCount(x, One) == 5);

            /// Three is "acdfg" and contains "One"
            Three = FilterSignals(One, 5);

            /// Nine is "abcdfg" and contains "Three"
            Nine = FilterSignals(Three, 6);

            /// Zero is "abcefg" and is the last 6 character value.
            Zero = SignalPatterns.Single(x => x.Length == 6 && !IsEqual(x, Nine) && !IsEqual(x, Six));

            /// Four is "bcdf"
            Four = SignalPatterns.Single(x => x.Length is 4);

            /// Two is "acdeg". It is 5 characters and differs from Four by 3 characters.
            Two = SignalPatterns.Single(x => x.Length == 5 && GetDifferenceCount(x, Four) == 3);

            /// Five is "abdfg" and is the last 5 character value.
            Five = SignalPatterns.Single(x => x.Length == 5 && !IsEqual(x, Two) && !IsEqual(x, Three));
        }

        private string FilterSignals(string fiterString, int filterLength)
        {
            var newStringList = SignalPatterns.Where(x => x.Length == filterLength);

            foreach (char character in fiterString)
            {
                newStringList = newStringList.Where(x => x.Contains(character));
            }

            return newStringList.Single();
        }

        private static int GetDifferenceCount(string stringToFilter, string filterString)
        {
            var newString = stringToFilter;

            foreach (char character in filterString)
            {
                newString = newString.Replace(character.ToString(), "");
            }

            return newString.Length;
        }

        private static string SortString(string toSort)
        {
            var newString = string.Join("", toSort.OrderBy(x => x));

            return newString ?? "";
        }

        private static bool IsEqual(string a, string b)
        {
            var sortedA = SortString(a);
            var sortedB = SortString(b);

            return sortedA == sortedB;
        }
    }
}
