namespace AdventOfCode2021.Problems
{
    internal class BasicGame
    {
        private IDice Dice { get; init; }
        private int WinningPoints { get; init; }
        private Player[] Players { get; set; }
        private int CurrentPlayerIndex { get; set; }

        public BasicGame(int winningPoints, IDice dice, int[] startingPlayerPositions)
        {
            CurrentPlayerIndex = 0;

            WinningPoints = winningPoints;
            Dice = dice;
            Players = new Player[]
            {
                new Player(startingPlayerPositions[0] -1, 0, 1),
                new Player(startingPlayerPositions[1] -1, 0, 2),

            };
        }

        public GameResult RunGame()
        {
            Player currentPlayer;

            do
            {
                // Get current player
                currentPlayer = Players[CurrentPlayerIndex];

                // Roll the dice
                var diceRoll = Dice.Roll() + Dice.Roll() + Dice.Roll();

                // Move the player
                currentPlayer.Move(diceRoll);

                // Switch to next player
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Length;

            } while (currentPlayer.Points < WinningPoints);

            return new GameResult(Players);
        }
    }

    internal class GameResult
    {
        public Player Winner { get; init; }
        public Player Loser { get; init; }

        public GameResult(Player[] players)
        {
            var sortedPlayers = players.OrderByDescending(p => p.Points);

            Winner = sortedPlayers.First();
            Loser = sortedPlayers.Last();
        }
    }
}
