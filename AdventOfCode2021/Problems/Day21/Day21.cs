using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day21 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            var startingPositions = GetInputValue()
                .Select(line => int.Parse(line.Split(" ").Last()))
                .ToArray();

            // Create dice
            var dice = new DeterministicDice();

            // Create Game from dice and players
            var game = new BasicGame(1000, dice, startingPositions);

            // Run game until winner is found
            var result = game.RunGame();

            // Get scores and value
            return result.Loser.Points * dice.GetNumberOfRolls();
        }

        public override object PartTwo()
        {
            var startingPositions = GetInputValue()
                .Select(line => int.Parse(line.Split(" ").Last()))
                .ToArray();

            var game = new QuantumGame(21, startingPositions);

            var result = game.RunGame();

            return result.Max(w => w.Value);
        }
    }
}
