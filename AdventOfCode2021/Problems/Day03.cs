using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day03 : Problem
    {
        protected override string ProblemName => "Day03";

        private IEnumerable<string> Data { get; set; }
        private int LengthOfBit { get; set; }

        public Day03()
        {
            Data = GetInputStringList();
            LengthOfBit = Data.ElementAt(0).Length;
        }

        public override object PartOne()
        {
            var gammaBit = "";
            var epsilonBit = "";

            for (int i = 0; i < LengthOfBit; i++)
            {
                var (oneCount, zeroCount) = GetCounts(Data, i);

                var mostCommonBit = oneCount > zeroCount ? '1' : '0';
                var leastCommonBit = mostCommonBit == '1' ? '0' : '1';

                gammaBit += mostCommonBit;
                epsilonBit += leastCommonBit;
            }

            var gammaValue = GetIntValue(gammaBit);
            var epsilonValue = GetIntValue(epsilonBit);

            return gammaValue * epsilonValue;
        }

        public override object PartTwo()
        {
            var oxygenValue = GetOxygenValue();

            var co2Value = GetCo2Value();

            return oxygenValue * co2Value;
        }

        private static int GetIntValue(string bitString) => Convert.ToInt32(bitString, 2);

        private static (int ones, int zeros) GetCounts(IEnumerable<string> data, int index)
        {
            var oneCount = data.Where(x => x[index] == '1').Count();
            var zeroCount = data.Count() - oneCount;

            return (oneCount, zeroCount);
        }

        private int GetOxygenValue()
        {
            IEnumerable<string> oxygenData = new List<string>(Data);

            for (int i = 0; i < LengthOfBit; i++)
            {
                var (oneCount, zeroCount) = GetCounts(oxygenData, i);

                char mostCommonBit = oneCount < zeroCount ? '0' : '1';

                if (oxygenData.Count() == 1)
                {
                    break;
                }

                oxygenData = oxygenData.Where(x => x[i] == mostCommonBit).ToList();
            }

            return GetIntValue(oxygenData.First());
        }

        private int GetCo2Value()
        {
            IEnumerable<string> co2Data = new List<string>(Data);

            for (int i = 0; i < LengthOfBit; i++)
            {
                var (oneCount, zeroCount) = GetCounts(co2Data, i);

                char leastCommonBit = oneCount < zeroCount ? '1' : '0';

                if (co2Data.Count() == 1)
                {
                    break;
                }

                co2Data = co2Data.Where(x => x[i] == leastCommonBit).ToList();
            }

            return GetIntValue(co2Data.First());
        }
    }
}
