namespace AdventOfCode2021.Problems
{
    internal class QuantumGame
    {
        private static readonly Dictionary<int, int> PossibleRollsCount = InitPossibleRollsCount();
        private QuantumPlayer[] InitialPlayers { get; init; }
        private int WinningPoints { get; init; }
        private readonly Dictionary<int, long> WinCounts = new()
        {
            { 1, 0 },
            { 2, 0 }
        };

        public QuantumGame(int winningPoints, int[] startingPlayerPositions)
        {
            WinningPoints = winningPoints;
            InitPossibleRollsCount();
            InitialPlayers = new QuantumPlayer[]
            {
                new QuantumPlayer(startingPlayerPositions[0] - 1, 0, 1),
                new QuantumPlayer(startingPlayerPositions[1] - 1, 0, 2)
            };
        }

        public Dictionary<int, long> RunGame()
        {
            // This is a little funky.
            // The first round needs to be player 1.
            // To do this, we have to pass in Player 2 as the "active" player
            // so that the first set of rolls will be for player 1.
            RollQuantumDice(InitialPlayers[1], InitialPlayers[0], 1);

            return WinCounts;
        }

        private void RollQuantumDice(QuantumPlayer activePlayer, QuantumPlayer idlePlayer, long universes)
        {
            if (activePlayer.Points >= WinningPoints)
            {
                // Increment the winCount for this player by the number
                // of universes this win represents
                WinCounts[activePlayer.Id] += universes;

                return;
            }

            foreach (var possibleRollValue in PossibleRollsCount.Keys)
            {
                // Roll the dice for every possible universe of rolls.
                // These will be rolled with new copies of the players.
                RollQuantumDice(
                    idlePlayer.Move(possibleRollValue),
                    activePlayer.Copy(),
                    universes * PossibleRollsCount[possibleRollValue]);
            }

        }

        private static Dictionary<int, int> InitPossibleRollsCount()
        {
            // Fill an dictionary with every possible combination of the 
            // three dice rolls and the count of each instance of that combination.
            var result = new Dictionary<int, int>();
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        var sum = i + j + k;
                        if (result.ContainsKey(sum))
                        {
                            result[sum]++;
                        }
                        else
                        {
                            result[sum] = 1;
                        }
                    }
                }
            }

            return result;
        }
    }
}
