using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    public class Day18 : Problem
    {
        protected override string InputName => "Actual";
        private readonly List<SnailFishNumber> SnailFishNumbers;
        private string[] Input { get; set; }

        public Day18()
        {
            Input = GetInputValue();

            SnailFishNumbers = new List<SnailFishNumber>();

            foreach (var line in Input)
            {
                SnailFishNumbers.Add(new SnailFishNumber(line));
            }
        }

        public override object PartOne()
        {
            var result = SnailFishNumbers.First();

            foreach (var number in SnailFishNumbers.Skip(1))
            {
                result = SnailFishNumber.Add(result, number);
            }
            Console.WriteLine($"Result: {result}");

            return result.GetMagnitude();
        }

        public override object PartTwo()
        {
            var maxMagnitude = 0;

            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = i + 1; j < Input.Length; j++)
                {
                    SnailFishNumber a = new(Input[i]);
                    SnailFishNumber b = new(Input[j]);

                    // check a + b
                    var result = SnailFishNumber.Add(a, b);
                    var magnitude = result.GetMagnitude();
                    if (magnitude > maxMagnitude)
                    {
                        maxMagnitude = magnitude;
                    }

                    // check b + a
                    a = new(Input[i]);
                    b = new(Input[j]);

                    result = SnailFishNumber.Add(b, a);
                    magnitude = result.GetMagnitude();
                    if (magnitude > maxMagnitude)
                    {
                        maxMagnitude = magnitude;
                    }
                }
            }

            return maxMagnitude;
        }

        public class SnailFishNumber
        {
            public int Value = -1;
            public SnailFishNumber Left { get; set; }
            public SnailFishNumber Right { get; set; }
            public SnailFishNumber Parent { get; set; }

            /// <summary>
            /// CTOR to intialize from a parent only
            /// </summary>
            /// <remarks>
            /// Used during initial parsing from a string.
            /// </remarks>
            public SnailFishNumber(SnailFishNumber parent)
            {
                Parent = parent;
            }

            /// <summary>
            /// CTOR to initialize from a parent and value.
            /// </summary>
            /// <remarks>
            /// Used during splitting.
            /// </remarks>
            public SnailFishNumber(SnailFishNumber parent, int value)
            {
                Parent = parent;
                Value = value;
            }

            /// <summary>
            /// CTOR to initialize from left and right numbers
            /// </summary>
            /// <remarks>
            /// Used while adding two numbers.
            /// </remarks>
            public SnailFishNumber(SnailFishNumber left, SnailFishNumber right)
            {
                Left = left;
                Right = right;
            }

            /// <summary>
            /// CTOR to initialize from string input. 
            /// </summary>
            /// <remarks>
            /// Used while parsing the problem input. 
            /// </remarks>
            public SnailFishNumber(string line)
            {
                SnailFishNumber current = this;

                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '[':
                            {
                                SnailFishNumber child = new(current);
                                current.AddLeftFirst(child);
                                current = child;

                                break;
                            }
                        case ',':
                            {
                                SnailFishNumber child = new(current.Parent);
                                current.Parent.AddLeftFirst(child);
                                current = child;

                                break;
                            }
                        case ']':
                            {
                                current = current.Parent;

                                break;
                            }
                        default:
                            {
                                current.Value = (int)char.GetNumericValue(c);
                                break;
                            }
                    }
                }
            }

            /// <summary>
            /// Add a child. Try to add to the left node first, otherwise add it to the right node. 
            /// </summary>
            public void AddLeftFirst(SnailFishNumber child)
            {
                if (Left == null)
                {
                    Left = child;
                }
                else if (Right == null)
                {
                    Right = child;
                }
                else
                {
                    throw new Exception("Already has Left and Right child");
                }
            }

            /// <summary>
            /// Retruns if this number is a leaf node.
            /// </summary>
            public bool IsLeaf => Left == null && Right == null;

            /// <summary>
            /// Outputs a string representation of this number.
            /// Used during debuging and test.
            /// </summary>
            public override string ToString()
            {
                if (IsLeaf)
                {
                    return $"{Value}";
                }
                else
                {
                    return $"[{Left},{Right}]";
                }
            }

            /// <summary>
            /// Returns the magnitude of this number.
            /// </summary>
            public int GetMagnitude()
            {
                if (IsLeaf)
                {
                    return Value;
                }
                else
                {
                    return Left.GetMagnitude() * 3 + Right.GetMagnitude() * 2;
                }
            }

            /// <summary>
            /// Adds two <see cref="SnailFishNumber"/>.
            /// </summary>
            /// <remarks>
            /// This is a non-commutative addition. (a + b != b + a)
            /// </remarks>
            public static SnailFishNumber Add(SnailFishNumber a, SnailFishNumber b)
            {
                SnailFishNumber parent = new(a, b);
                a.Parent = parent;
                b.Parent = parent;
                Reduce(parent);

                return parent;
            }

            /// <summary>
            /// Reduces a number and all its children.
            /// </summary>
            public static void Reduce(SnailFishNumber number)
            {
                while (true)
                {
                    ExplodeAll(number);
                    if (!SplitFirst(number))
                    {
                        break;
                    }
                }
            }

            /// <summary>
            /// Explodes a number and its children. 
            /// </summary>
            protected static void ExplodeAll(SnailFishNumber start)
            {
                while (ExplodeFirst(start)) ;
            }

            /// <summary>
            /// Explodes a number and its children.
            /// </summary>
            protected static bool ExplodeFirst(SnailFishNumber current, int depth = 0)
            {
                if (current.IsLeaf)
                {
                    return false;
                }

                if (depth == 4)
                {
                    SnailFishNumber leftNeighbor = GetFirstLeft(current.Left);
                    SnailFishNumber rightNeighbor = GetFirstRight(current.Right);

                    if (leftNeighbor != null)
                    {
                        leftNeighbor.Value += current.Left.Value;
                    }
                    if (rightNeighbor != null)
                    {
                        rightNeighbor.Value += current.Right.Value;
                    }

                    current.Left = null;
                    current.Right = null;
                    current.Value = 0;

                    return true;
                }
                else
                {
                    if (ExplodeFirst(current.Left, depth + 1))
                    {
                        return true;
                    }
                    if (ExplodeFirst(current.Right, depth + 1))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Splits a number and its children.
            /// </summary>
            protected static bool SplitFirst(SnailFishNumber number)
            {
                if (number.IsLeaf)
                {
                    if (number.Value >= 10)
                    {
                        number.Left = new SnailFishNumber(number, number.Value / 2);
                        number.Right = new SnailFishNumber(number, (number.Value / 2) + number.Value % 2);
                        number.Value = -1;
                        return true;
                    }
                }
                else
                {
                    if (SplitFirst(number.Left))
                    {
                        return true;
                    }
                    else if (SplitFirst(number.Right))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Returns the first left leaf number of the current number.
            /// </summary>
            protected static SnailFishNumber GetFirstLeft(SnailFishNumber start)
            {
                SnailFishNumber current = start;

                while (current.Parent.Left == current)
                {
                    current = current.Parent;
                    if (current.Parent == null)
                    {
                        return null;
                    }
                }

                current = current.Parent.Left;
                while (!current.IsLeaf)
                {
                    current = current.Right;
                }

                return current;
            }

            /// <summary>
            /// Returns the first right leaf number of the current number. 
            /// </summary>
            protected static SnailFishNumber GetFirstRight(SnailFishNumber start)
            {
                SnailFishNumber current = start;

                while (current.Parent.Right == current)
                {
                    current = current.Parent;
                    if (current.Parent == null)
                    {
                        return null;
                    }
                }

                current = current.Parent.Right;
                while (!current.IsLeaf)
                {
                    current = current.Left;
                }
                return current;
            }
        }
    }
}
