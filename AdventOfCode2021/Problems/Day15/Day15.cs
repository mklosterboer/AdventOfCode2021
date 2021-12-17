using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day15 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            //return "";
            var input = GetInputValue();
            var nodes = GetNodes(input);

            var shortestPath = GetShortestPath(nodes);

            return shortestPath.Sum(n => n.Risk);
        }

        public override object PartTwo()
        {
            var input = GetInputValue();
            var nodes = GetNodesPt2(input);

            //PrintPath(nodes, new List<Node>(), new List<Node> { });

            var shortestPath = GetShortestPath(nodes);

            return shortestPath.Sum(n => n.Risk);
        }

        private static List<Node> GetShortestPath(Dictionary<(int x, int y), Node> nodes)
        {
            Node startNode = nodes.First().Value;
            Node endNode = nodes.Last().Value;

            var queue = new List<Node>() { startNode };
            var visited = new List<Node>();

            // Use A* to find best path
            while (queue.Any())
            {
                if (visited.Count % 100 == 0)
                {
                    Console.WriteLine($"Visited {visited.Count} out of {nodes.Count}");
                }

                var checkNode = queue.OrderBy(x => x.PathHueristic).First();

                if (checkNode == endNode)
                {
                    var path = new List<Node>();

                    var currentNode = endNode;

                    while (currentNode.ParentNode != null)
                    {
                        path.Add(currentNode);
                        currentNode = currentNode.ParentNode;
                    }

                    path.Reverse();

                    Console.WriteLine($"Checked {visited.Count} out of {nodes.Count} nodes");

                    //PrintPath(nodes, path, visited);

                    return path;
                }

                visited.Add(checkNode);
                queue.Remove(checkNode);

                foreach (var node in checkNode.AdjacentNodes)
                {
                    if (visited.Contains(node))
                    {
                        // I need to double check if this is correct.
                        continue;
                    }

                    if (queue.Contains(node))
                    {
                        if(node.Distance > node.RiskDistance)
                        {
                        }
                        node.TryAddNewParent(checkNode);
                    }
                    else
                    {
                        node.UpdateParentNode(checkNode);
                        queue.Add(node);
                    }
                }
            }

            // Just to keep the complier happy while I work
            return null;
        }


        private static void PrintPath(Dictionary<(int x, int y), Node> allNodes, List<Node> path, List<Node> visited)
        {
            var height = allNodes.Keys.Max(k => k.y);
            var width = allNodes.Keys.Max(k => k.x);

            Console.WriteLine("");
            for (int y = 0; y < height + 1; y++)
            {
                Console.WriteLine("");
                for (int x = 0; x < width + 1; x++)
                {
                    if (path.Any(n => n.X == x && n.Y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(allNodes[(x, y)].Risk);

                    }
                    else if (visited.Any(n => n.X == x && n.Y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(allNodes[(x, y)].Risk);
                    }
                    else
                    {
                        Console.Write(allNodes[(x, y)].Risk);
                        //Console.Write(".");
                    }
                    Console.ResetColor();
                }
            }
            Console.WriteLine("");
        }


        private static Dictionary<(int x, int y), Node> GetNodes(string[] input)
        {
            var nodes = new Dictionary<(int x, int y), Node>();

            var height = input.Length;
            var width = input[0].Length;
            (int X, int Y) endNodeCoordinates = (width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var newNode = new Node(x, y, int.Parse(input[y][x].ToString()), endNodeCoordinates);
                    nodes[(x, y)] = newNode;
                }
            }

            return FillAdjacentNodes(nodes, height, width);
        }

        private static Dictionary<(int X, int Y), Node> GetNodesPt2(string[] input)
        {
            var nodes = new Dictionary<(int X, int Y), Node>();

            var height = input.Length;
            var width = input[0].Length;
            (int X, int Y) endNodeCoordinates = (width * 5, height * 5);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var newNode = new Node(x, y, int.Parse(input[y][x].ToString()), endNodeCoordinates);
                    nodes.Add((x, y), newNode);
                }
            }

            var nodesX5 = new Dictionary<(int X, int Y), Node>();

            for (int x = 0; x < width * 5; x++)
            {
                for (int y = 0; y < height * 5; y++)
                {
                    var oldNode = nodes[(x % width, y % height)];
                    long toAdd = x / width + y / height;
                    long newRisk = (oldNode.Risk + toAdd - 1) % 9 + 1;
                    nodesX5[(x, y)] = new Node(x, y, newRisk, (width * 5, height * 5));
                }
            }

            return FillAdjacentNodes(nodesX5, height * 5, width * 5);

        }

        private static Dictionary<(int X, int Y), Node> FillAdjacentNodes(Dictionary<(int X, int Y), Node> nodes, int height, int width)
        {

            foreach (var dn in nodes)
            {
                var node = dn.Value;
                if (node.X > 0)
                {
                    // Left
                    node.AddAdjacentNode(nodes[(node.X - 1, node.Y)]);
                }
                if (node.X < width - 1)
                {
                    // Right
                    node.AddAdjacentNode(nodes[(node.X + 1, node.Y)]);
                }
                if (node.Y > 0)
                {
                    // Top
                    node.AddAdjacentNode(nodes[(node.X, node.Y - 1)]);
                }
                if (node.Y < height - 1)
                {
                    // Bottom
                    node.AddAdjacentNode(nodes[(node.X, node.Y + 1)]);
                }
            }

            return nodes;
        }

        private class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public long Risk { get; private set; }
            public long Distance { get; init; }
            public long RiskDistance => (Distance) + Risk;
            public long PathHueristic { get; private set; }
            public long PathRisk { get; private set; }
            public List<Node> AdjacentNodes { get; private set; }
            public Node? ParentNode { get; private set; }

            public Node(int x, int y, long risk, (int X, int Y) endNodeCoordinates)
            {
                X = x;
                Y = y;
                Risk = risk;
                AdjacentNodes = new List<Node>();
                Distance = Math.Abs(endNodeCoordinates.X - x) + Math.Abs(endNodeCoordinates.Y - y);
                PathHueristic = RiskDistance;
            }

            public void AddAdjacentNode(Node adjacentNode)
            {
                AdjacentNodes.Add(adjacentNode);
            }

            public void UpdateParentNode(Node parent)
            {
                ParentNode = parent;
                PathHueristic = parent.PathHueristic + RiskDistance;
                PathRisk = parent.PathRisk + Risk;
            }

            public void TryAddNewParent(Node newParent)
            {
                var potentialNewRisk = newParent.PathRisk + Risk;
                if (potentialNewRisk < PathRisk)
                {
                    UpdateParentNode(newParent);
                }
            }
        }
    }
}
