namespace AdventOfCode2021.Problems
{
    internal record Player
    {
        public int Position { get; set; }
        public int Points { get; set; }
        public int Id { get; set; }

        private const int MaxPosition = 10;

        public Player(int position, int points, int id)
        {
            Position = position;
            Points = points;
            Id = id;
        }

        public void Move(int diceRoll)
        {
            var nextPostion = (Position + diceRoll) % MaxPosition;
            Position = nextPostion;
            Points += nextPostion + 1;
        }
    };
}
