using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    public class Day04 : Problem
    {
        protected override string ProblemName => "Day04";
        private string[] InputData { get; set; }

        public Day04()
        {
            InputData = GetInputValue();
        }

        public override object PartOne()
        {
            var winningGameScores = PlayGames();

            return winningGameScores.First();
        }

        public override object PartTwo()
        {
            var winningGameScores = PlayGames();

            return winningGameScores.Last();
        }

        private List<BingoCard> GetBingoCards()
        {
            var bingoCards = new List<BingoCard>();

            var i = 2;
            do
            {
                var boardList = new List<BingoNode>();
                var y = 0;
                for (var j = i; j < i + 5; j++)
                {
                    var cleanedString = InputData[j].TrimStart().Replace("  ", " ").Split(' ');
                    boardList.AddRange(cleanedString.Select((v, idx) => new BingoNode(idx, y, int.Parse(v))));
                    y++;
                }

                bingoCards.Add(new BingoCard(boardList));

                i += 6;
            } while (i < InputData.Length);

            return bingoCards;
        }

        private IEnumerable<int> PlayGames()
        {
            var bingoCards = GetBingoCards();
            var numbersToCall = InputData[0].Split(',').Select(x => int.Parse(x));

            var winningGameScores = new List<int>();

            foreach (var number in numbersToCall)
            {
                foreach (var card in bingoCards)
                {
                    if (!card.IsWinner)
                    {
                        card.HandleNumberCalled(number);
                        if (card.IsWinner)
                        {
                            winningGameScores.Add(card.GetScore(number));
                        }
                    }
                }
            }

            return winningGameScores;
        }
    }

    public class BingoNode
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int Value { get; init; }
        public bool Selected { get; private set; }

        public BingoNode(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
            Selected = false;
        }

        public void CheckNode(int numberCalled)
        {
            if (numberCalled == Value)
            {
                Selected = true;
            }
        }
    }

    public class BingoCard
    {
        public IEnumerable<BingoNode> BingoNodes { get; set; }
        public bool IsWinner { get; set; }

        private ILookup<int, BingoNode> Rows { get; set; }
        private ILookup<int, BingoNode> Columns { get; set; }

        public BingoCard(IEnumerable<BingoNode> bingoNodes)
        {
            BingoNodes = bingoNodes;
            Rows = bingoNodes.ToLookup(n => n.Y);
            Columns = bingoNodes.ToLookup(n => n.X);
        }

        public void HandleNumberCalled(int numberCalled)
        {
            foreach (var node in BingoNodes)
            {
                node.CheckNode(numberCalled);
            }

            for (var i = 0; i < 5; i++)
            {
                if (Rows[i].All(n => n.Selected) || Columns[i].All(n => n.Selected))
                {
                    IsWinner = true;
                }
            }
        }

        public int GetScore(int numberCalled)
        {
            var remainingItemSum = BingoNodes.Where(n => !n.Selected).Sum(x => x.Value);

            return numberCalled * remainingItemSum;
        }
    }
}
