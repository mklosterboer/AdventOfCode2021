using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    public class Day02 : Problem
    {
        protected override string InputName => "Actual";

        private IEnumerable<string> InstructionStrings { get; set; }

        public Day02()
        {
            InstructionStrings = GetInputStringList();
        }

        public override object PartOne()
        {
            var submarine = new BasicSubmarine();

            foreach (var instructionString in InstructionStrings)
            {
                var instruction = new Instruction(instructionString);

                submarine.Move(instruction);
            }

            return submarine.Depth * submarine.HorizontalPosition;
        }

        public override object PartTwo()
        {
            var submarine = new AimedSubmarine();

            foreach (var instructionString in InstructionStrings)
            {
                var instruction = new Instruction(instructionString);

                submarine.Move(instruction);
            }

            return submarine.Depth * submarine.HorizontalPosition;
        }

        public enum Direction
        {
            Foward,
            Down,
            Up
        }

        public class Instruction
        {
            public Direction Direction { get; set; }
            public int Amount { get; set; }

            protected const string FORWARD = "forward";
            protected const string DOWN = "down";
            protected const string UP = "up";

            public Instruction(string instruction)
            {
                var splitInstruction = instruction.Split(' ');

                Amount = int.Parse(splitInstruction[1]);

                switch (splitInstruction[0])
                {
                    case FORWARD:
                        Direction = Direction.Foward;
                        break;
                    case DOWN:
                        Direction = Direction.Down;
                        break;
                    case UP:
                        Direction = Direction.Up;
                        break;
                    default:
                        break;
                }
            }

            public Instruction(Direction direciton, int amount)
            {
                Direction = direciton;
                Amount = amount;
            }
        }

        public abstract class Submarine
        {
            public int Depth = 0;
            public int HorizontalPosition = 0;

            public void Move(Instruction instruction)
            {
                switch (instruction.Direction)
                {
                    case Direction.Foward:
                        Forward(instruction.Amount);
                        break;
                    case Direction.Down:
                        Down(instruction.Amount);
                        break;
                    case Direction.Up:
                        Up(instruction.Amount);
                        break;
                    default:
                        break;
                }
            }

            protected abstract void Forward(int amount);

            protected abstract void Up(int amount);

            protected abstract void Down(int amount);
        }

        public class BasicSubmarine : Submarine
        {
            protected override void Forward(int amount)
            {
                HorizontalPosition += amount;
            }

            protected override void Up(int amount)
            {
                Depth -= amount;
            }

            protected override void Down(int amount)
            {
                Depth += amount;
            }
        }

        public class AimedSubmarine : Submarine
        {
            private int Aim = 0;

            protected override void Forward(int amount)
            {
                HorizontalPosition += amount;
                Depth += Aim * amount;
            }

            protected override void Up(int amount)
            {
                Aim -= amount;
            }

            protected override void Down(int amount)
            {
                Aim += amount;
            }
        }
    }
}
