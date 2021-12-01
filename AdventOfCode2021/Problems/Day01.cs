using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    public class Day01 : Problem, IProblem
    {
        protected override string ProblemName => "Day01";
        private IEnumerable<int> Measurements { get; set; }

        public Day01()
        {
            Measurements = GetInputNumberList();
        }

        public override object PartOne()
        {
            var increasedCount = 0;

            for (int i = 1; i < Measurements.Count(); i++)
            {
                if (Measurements.ElementAt(i) > Measurements.ElementAt(i - 1))
                {
                    increasedCount++;
                }
            }

            return increasedCount;
        }

        public override object PartTwo()
        {
            var increasedCount = 0;

            for (int i = 1; i + 2 < Measurements.Count(); i++)
            {
                var nMinus1 = Measurements.ElementAt(i - 1);
                var n = Measurements.ElementAt(i);
                var nPlus1 = Measurements.ElementAt(i + 1);
                var nPlus2 = Measurements.ElementAt(i + 2);

                var currentSet = nMinus1 + n + nPlus1;
                var nextSet = n + nPlus1 + nPlus2;

                if (nextSet - currentSet > 0)
                {
                    increasedCount++;
                }
            }

            return increasedCount;
        }
    }
}
