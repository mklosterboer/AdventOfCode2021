﻿using AdventOfCode2021.DataStructures;

namespace AdventOfCode2021.Problems
{
    public class Basin
    {
        public List<Vector2> contents = new();
        public Vector2 LowPoint { get; private set; }

        public bool Contains(Vector2 point)
        {
            return contents.Contains(point);
        }

        public void AddPoint(Vector2 point)
        {
            contents.Add(point);
        }

        public void SetLowPoint(Vector2 point)
        {
            LowPoint = point;
        }

        public int GetSize()
        {
            return contents.Count;
        }
    }
}
