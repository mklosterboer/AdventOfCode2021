using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    internal record Step
    {
        private static readonly Regex LineRegex =
            new(@"(?<action>on|off) x=(?<x1>-?[0-9]\d*)..(?<x2>-?[0-9]\d*),y=(?<y1>-?[0-9]\d*)..(?<y2>-?[0-9]\d*),z=(?<z1>-?[0-9]\d*)..(?<z2>-?[0-9]\d*)");

        public bool Action;
        public Range X;
        public Range Y;
        public Range Z;

        public Step(string line)
        {
            var matchGroups = LineRegex.Match(line).Groups;

            X = new(int.Parse(matchGroups["x1"].Value), int.Parse(matchGroups["x2"].Value));
            Y = new(int.Parse(matchGroups["y1"].Value), int.Parse(matchGroups["y2"].Value));
            Z = new(int.Parse(matchGroups["z1"].Value), int.Parse(matchGroups["z2"].Value));

            Action = matchGroups["action"].Value == "on";
        }

        public bool IsInInitializationRegion =>
            IsWithinRange(X) &&
            IsWithinRange(Y) &&
            IsWithinRange(Z);

        private static bool IsWithinRange(Range range) =>
            range.Start is >= -50 and <= 50 &&
            range.End is >= -50 and <= 50;
    }

    internal record Range(int Start, int End)
    {
        public int Length => End - Start + 1;
    }
}
