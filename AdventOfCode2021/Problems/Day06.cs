using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day06 : Problem
    {
        protected override string ProblemName => "Day06";

        private const int NewFishGestationPeriod = 8;
        private const int ExistingFishGestationPeriod = 6;

        private IEnumerable<int> Input { get; set; }

        public Day06()
        {
            Input = GetFirstRow().Split(',').Select(x => int.Parse(x));
        }

        public override object PartOne()
        {
            return GetFishCountAfter(80);
        }

        public override object PartTwo()
        {
            return GetFishCountAfter(256);
        }

        private long GetFishCountAfter(int days)
        {
            // Setup dictionary of all possible ages of fish
            // and a count of how many fish are that age on this day.
            Dictionary<int, long> fishCountByAge = Enumerable
                .Range(0, NewFishGestationPeriod + 1)
                .ToDictionary(
                    x => x,
                    x => 0L);

            // Intialize fishCountByAge with the input data
            Input.GroupBy(x => x)
                .ForEach(gp =>
                {
                    fishCountByAge[gp.Key] = gp.Count();
                });

            // Loop over the days and shift the count of fish at a given age,
            // then handle reproducing
            for (int i = 0; i < days; i++)
            {
                // Save off how many fish will reproduce
                var CountToReproduce = fishCountByAge[0];

                // Move count down by one age 
                for (int j = 0; j < 8; j++)
                {
                    fishCountByAge[j] = fishCountByAge[j + 1];
                }

                // New fish
                fishCountByAge[NewFishGestationPeriod] = CountToReproduce;

                // Reset counter of fish that reproduced. 
                fishCountByAge[ExistingFishGestationPeriod] += CountToReproduce;
            }

            return fishCountByAge.Sum(x => x.Value);
        }
    }
}
