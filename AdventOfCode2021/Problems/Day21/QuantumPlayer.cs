namespace AdventOfCode2021.Problems
{
    internal record QuantumPlayer
    {
        public int Position { get; set; }
        public int Points { get; set; }
        public int Id { get; set; }

        private const int MaxPosition = 10;

        public QuantumPlayer(int position, int points, int id)
        {
            Position = position;
            Points = points;
            Id = id;
        }

        public QuantumPlayer Move(int diceRoll)
        {
            var nextPostion = (Position + diceRoll) % MaxPosition;

            return new(nextPostion, Points + nextPostion + 1, Id);
        }

        public QuantumPlayer Copy() => new(Position, Points, Id);
    };

}
