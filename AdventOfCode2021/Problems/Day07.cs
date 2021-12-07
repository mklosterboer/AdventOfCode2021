using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day07 : Problem
    {
        protected override string ProblemName => "Day07";
        private IEnumerable<int> InputData { get; set; }

        public Day07()
        {
            InputData = GetInputValue().ElementAt(0).Split(',').Select(x => int.Parse(x));
        }

        public override object PartOne()
        {
            var maxValue = InputData.Max();
            var minValue = InputData.Min();

            var costs = new List<int>();

            for (var i = minValue; i < maxValue; i++)
            {
                var crabCost = 0;
                foreach (var location in InputData)
                {
                    crabCost += Math.Abs(i - location);
                }
                costs.Add(crabCost);
            }

            return costs.Min();
        }

        public override object PartTwo()
        {
            var maxValue = InputData.Max();
            var minValue = InputData.Min();

            var costs = new List<int>();

            for (var i = minValue; i < maxValue; i++)
            {
                var crabCost = 0;
                foreach (var location in InputData)
                {
                    var steps = Math.Abs(i - location);

                    // 1 + 2 + ... + (n) = (n (n + 1) /2
                    crabCost += (steps * (steps + 1)) / 2;
                }
                costs.Add(crabCost);
            }
            return costs.Min();
        }
    }
}
