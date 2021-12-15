using AdventOfCode2021.Utilities;

namespace AdventOfCode2021.Problems
{
    internal class Day12 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            var input = GetInputValue();

            var nodes = GetNodes();

            var startNode = nodes.First(x => x.IsStart);
            var endNode = nodes.First(x => x.IsEnd);

            var graph = new Graph();

            var result = graph.GetAllPaths(startNode, endNode, true);

            return result;
        }

        public override object PartTwo()
        {
            var input = GetInputValue();

            var nodes = GetNodes();

            var startNode = nodes.First(x => x.IsStart);
            var endNode = nodes.First(x => x.IsEnd);

            var graph = new Graph();

            var result = graph.GetAllPaths(startNode, endNode, false);

            return result;
        }

        private List<Node> GetNodes()
        {
            var input = GetInputValue();

            var nodes = new Dictionary<string, Node>();

            foreach (var item in input)
            {
                var splitItem = item.Split('-');

                var thisNodeName = splitItem[0];
                var connectionName = splitItem[1];
                if (nodes.ContainsKey(thisNodeName))
                {
                    if (nodes.ContainsKey(connectionName))
                    {
                        // Both nodes exist, just add the children to each
                        nodes[thisNodeName].AddChild(nodes[connectionName]);
                        nodes[connectionName].AddChild(nodes[thisNodeName]);
                    }
                    else
                    {
                        // Only this node exists, create a new node for connection and add children to each
                        var newConnectionNode = new Node(connectionName);
                        newConnectionNode.AddChild(nodes[thisNodeName]);

                        nodes[thisNodeName].AddChild(newConnectionNode);

                        nodes.Add(connectionName, newConnectionNode);
                    }
                }
                else
                {
                    if (nodes.ContainsKey(connectionName))
                    {
                        // The connection exists, but this node doesn't
                        // Make this node and add children to both
                        var thisNode = new Node(thisNodeName);
                        thisNode.AddChild(nodes[connectionName]);

                        nodes[connectionName].AddChild(thisNode);

                        nodes.Add(thisNodeName, thisNode);
                    }
                    else
                    {
                        // Neither exists, create both
                        var thisNode = new Node(thisNodeName);
                        var newConnectionNode = new Node(connectionName);

                        thisNode.AddChild(newConnectionNode);
                        newConnectionNode.AddChild(thisNode);

                        nodes.Add(thisNodeName, thisNode);
                        nodes.Add(connectionName, newConnectionNode);
                    }
                }
            }

            return nodes.Values.ToList();
        }
    }

    internal class Node
    {
        public string Name { get; set; }

        public HashSet<Node> Children { get; set; }
        public bool IsSmallCave { get; init; }
        public bool IsStart { get; init; }
        public bool IsEnd { get; init; }

        public Node(string name)
        {
            Name = name;
            Children = new HashSet<Node>();
            IsSmallCave = char.IsLower(name[0]);
            IsStart = name == "start";
            IsEnd = name == "end";
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
        }
    }

    internal class Graph
    {
        public int PathCount { get; set; }
        private readonly Stack<Node> CurrentPath = new();
        private readonly List<List<Node>> AllPaths = new();

        public int GetAllPaths(Node startAt, Node endAt, bool shouldLimitVisit)
        {
            DFS(startAt, endAt, shouldLimitVisit);

            return AllPaths.Count;
        }

        public void DFS(Node startAt, Node endAt, bool shouldLimitVisit)
        {
            if (startAt.IsStart && CurrentPath.Contains(startAt))
            {
                return;
            }

            CurrentPath.Push(startAt);

            if (startAt == endAt)
            {
                AllPaths.Add(CurrentPath.Reverse().ToList());
                CurrentPath.Pop();

                return;
            }

            if (startAt.IsSmallCave && ShouldSkip(startAt, shouldLimitVisit))
            {
                CurrentPath.Pop();
                return;
            }

            foreach (var child in startAt.Children)
            {
                DFS(child, endAt, shouldLimitVisit);
            }

            CurrentPath.Pop();
        }

        private bool ShouldSkip(Node node, bool shouldLimitVisit)
        {
            if (shouldLimitVisit)
            {
                // Should limit to one
                return CurrentPath.Count(n => n.Equals(node)) > 1;
            }
            else
            {
                // Should limit to twice for one small cave,
                // then only once for every other cave.
                var otherCavesVisitedTwiceAlready = CurrentPath
                    .Where(n => n.IsSmallCave && !n.Equals(node))
                    .GroupBy(c => c)
                    .Any(g => g.Count() > 1);

                return otherCavesVisitedTwiceAlready
                    && CurrentPath.Count(c => c.Equals(node)) == 2
                    || CurrentPath.Count(c => c.Equals(node)) > 2;
            }
        }
    }
}
