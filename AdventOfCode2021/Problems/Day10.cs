using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    public class Day10 : Problem
    {
        protected override string ProblemName => "Day10";

        private int PartOneScore { get; set; }
        private long PartTwoScore { get; set; }

        public Day10()
        {
            var lines = GetInputValue();
            (int partOneScore, long partTwoScore) = CalculateScores(lines);

            PartOneScore = partOneScore;
            PartTwoScore = partTwoScore;
        }

        public static (int partOneScore, long partTwoScore) CalculateScores(string[] lines)
        {
            var partOneScore = 0;
            var partTwoScores = new List<long>();

            foreach (var line in lines)
            {
                var unclosedParens = new Stack<char>();

                var lineIsCorrupted = false;

                foreach (var c in line)
                {
                    // check if opening or closing character
                    if (CharacterSets.IsOpeningCharacter(c))
                    {
                        // if opening, add current char to stack
                        unclosedParens.Push(c);
                        continue;
                    }

                    // This is a closing character. 
                    if (unclosedParens.Any())
                    {
                        var topOfStack = unclosedParens.Peek();

                        // Check if the top of the stack is the pair to this character
                        if (CharacterSets.IsSet(topOfStack, c))
                        {
                            // if it is, this is a valid set. Remove this from the stack.
                            unclosedParens.Pop();
                        }
                        else
                        {
                            // if not, this is a corrupted line. Get the score for part one.
                            lineIsCorrupted = true;
                            partOneScore += PartOneScorer.GetScore(c);
                            break;
                        }
                    }
                }

                if (!lineIsCorrupted)
                {
                    // This is an incomplete line. Get the score for part two.
                    partTwoScores.Add(PartTwoScorer.GetScore(unclosedParens.ToList()));
                }
            }

            // Find middle score for part two.
            var halfIndex = partTwoScores.Count / 2;
            var partTwoScore = partTwoScores.OrderBy(x => x).ElementAt(halfIndex);

            return (partOneScore, partTwoScore);
        }

        public override object PartOne()
        {
            return PartOneScore;
        }

        public override object PartTwo()
        {
            return PartTwoScore;
        }
    }

    internal static class CharacterSets
    {
        private static readonly Dictionary<char, char> Pairs = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' }
        };

        public static bool IsOpeningCharacter(char item)
        {
            return Pairs.ContainsKey(item);
        }

        public static bool IsSet(char topOfStack, char testCharacter)
        {
            return Pairs[topOfStack] == testCharacter;
        }
    }

    internal static class PartOneScorer
    {
        private static readonly Dictionary<char, int> ScoreValues = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        public static int GetScore(char item)
        {
            return ScoreValues[item];
        }
    }

    internal static class PartTwoScorer
    {
        private static readonly Dictionary<char, int> ScoreValues = new()
        {
            { '(', 1 },
            { '[', 2 },
            { '{', 3 },
            { '<', 4 }
        };

        public static long GetScore(List<char> items)
        {
            var score = 0L;
            foreach (var item in items)
            {
                score *= 5;
                score += ScoreValues[item];
            }
            return score;
        }
    }

}
