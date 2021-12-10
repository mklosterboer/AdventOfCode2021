using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day09 : Problem
    {
        protected override string ProblemName => "Day09Test";

        public override object PartOne()
        {
            var input = GetInputValue().Select(x =>
                 x.ToCharArray()
                     .Select(y => Convert.ToInt32(char.GetNumericValue(y)))
                     .ToArray())
                .ToArray();
            var width = input[0].Length;
            var depth = input.Length;

            var dangerScore = 0;
            var lowSpots = new List<(int x, int y, int value)>();

            for (int y = 0; y < depth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var currentValue = input[y][x];
                    // Check if row low
                    var isLowerThanLeft = x <= 0 || currentValue < input[y][x - 1];
                    var isLowerThanRight = x >= width - 1 || currentValue < input[y][x + 1];
                    var isRowLow = isLowerThanLeft && isLowerThanRight;

                    // if row low, check if true low
                    if (isRowLow)
                    {
                        var isLowerThanTop = y <= 0 || currentValue < input[y - 1][x];
                        var isLowerThanBottom = y >= depth - 1 || currentValue < input[y + 1][x];
                        var isTrueLow = isLowerThanTop && isLowerThanBottom;

                        // if true low, add to score
                        if (isTrueLow)
                        {
                            lowSpots.Add(new(x, y, currentValue));
                            dangerScore += currentValue + 1;
                        }
                    }
                }
            }

            return dangerScore;
        }

        public override object PartTwo()
        {
            var input = GetInputValue().Select(x =>
                 x.ToCharArray()
                     .Select(y => Convert.ToInt32(char.GetNumericValue(y)))
                     .ToArray())
                .ToArray();

            var width = input[0].Length;
            var depth = input.Length;

            // Initialize a list of Basins

            // A Basin will be an array of (x, y) coordinates

            // Loop over each point
            // If it is NOT 9
            // Then loop over each point in each basin
            // compare the x and y 
            // if the value is within 1 for x OR y, then it is connected
            // then add it to that basin

            // If it is not contained in any basin's create a new Basin

            var basins = new List<HashSet<(int x, int y)>>();


            for (int y = 0; y < depth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var currentValue = input[y][x];

                    if (input[y][x] == 9)
                    {
                        continue;
                    }

                    var basinFound = false;

                    for (int z = 0; z < basins.Count && !basinFound; z++)
                    {
                        var currentBasin = basins[z];

                        if (currentBasin.Contains(new(x, y)))
                        {
                            basinFound = true;
                            // maybe explore down one more number? 
                            if (y + 1 < depth && input[y + 1][x] != 9)
                            {
                                currentBasin.Add(new(x, y + 1));
                            }
                            break;
                        }

                        for (int i = 0; i < currentBasin.Count && !basinFound; i++)
                        {
                            var point = currentBasin.ElementAt(i);

                            var isConnected = CompareTwoPoints(point, new(x, y));

                            if (isConnected)
                            {
                                // This new point is attached to this basin
                                currentBasin.Add(new(x, y));
                                basinFound = true;

                                // maybe explore down one more number? 
                                if (y + 1 < depth && input[y + 1][x] != 9)
                                {
                                    currentBasin.Add(new(x, y + 1));
                                }
                            }
                        }
                    }

                    if (!basinFound)
                    {
                        basins.Add(new HashSet<(int x, int y)>() { new(x, y) });
                    }
                }
            }

            var largestBasins = basins.OrderByDescending(x => x.Count)
                .Take(3)
                .Select(x => x.Count);

            return "";
        }

        private bool CompareTwoPoints((int x, int y) a, (int x, int y) b)
        {
            var xDiff = Math.Abs(a.x - b.x);
            var yDiff = Math.Abs(a.y - b.y);

            var sameRow = xDiff == 1 && yDiff == 0;
            var sameColumn = xDiff == 0 && yDiff == 1;

            return sameRow && sameColumn;
        }

    }
}
