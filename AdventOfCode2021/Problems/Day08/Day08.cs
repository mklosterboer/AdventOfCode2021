using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day08 : Problem
    {
        protected override string InputName => "Actual";

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
                .Select(x => new SignalByVisual(x))
                .Sum(s => s.GetTranslatedOutputValue());
        }
    }
}
