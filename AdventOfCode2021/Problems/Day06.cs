using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day06 : Problem
    {
        protected override string ProblemName => "Day06";
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
            // Setup dictionary of all possible ages of fish and a count of how many fish
            // are that age on this day.
            var fishCountByAge = new Dictionary<long, long>
            {
                {0,0}, {1,0}, {2,0}, {3,0}, {4,0}, {5,0}, {6,0}, {7,0}, {8,0}
            };

            var initalData = Input
                .GroupBy(x => x)
                .Select<IGrouping<int, int>, (int Age, int Count)>(x => new
                (
                    x.Key,
                    x.Count()
                ));

            // Intialize fishCoundByAge with the input data
            foreach (var (Age, Count) in initalData)
            {
                fishCountByAge[Age] = Count;
            }

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
                fishCountByAge[8] = CountToReproduce;

                // Reset counter of fish that reproduced. 
                fishCountByAge[6] += CountToReproduce;
            }

            return fishCountByAge.Sum(x => x.Value);
        }
    }
}
