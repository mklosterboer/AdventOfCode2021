using AdventOfCode2021.DataStructures;
using AdventOfCode2021.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    internal class Day19 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            var input = GetInputValue();

            var scanners = LocateScanners(input);

            var beacons = scanners.SelectMany(scanner => scanner.GetBeaconsInWorld())
                .OrderBy(v => v.X)
                .Distinct();

            return beacons.Count();
        }

        public override object PartTwo()
        {
            var input = GetInputValue();
            var scanners = LocateScanners(input);

            var maxDistace = 0;

            foreach (var sA in scanners)
            {
                foreach (var sB in scanners)
                {
                    var distance =
                        Math.Abs(sA.Center.X - sB.Center.X) +
                        Math.Abs(sA.Center.Y - sB.Center.Y) +
                        Math.Abs(sA.Center.Z - sB.Center.Z);

                    if (distance > maxDistace)
                    {
                        maxDistace = distance;
                    }
                }
            }

            return maxDistace;
        }

        private static HashSet<Scanner> LocateScanners(string[] input)
        {
            var scanners = new HashSet<Scanner>(Parse(input));

            var locatedScanners = new HashSet<Scanner>();
            var queue = new Queue<Scanner>();

            locatedScanners.Add(scanners.First());
            queue.Enqueue(scanners.First());

            scanners.Remove(scanners.First());

            while (queue.Any())
            {
                var scannerA = queue.Dequeue();

                foreach (var scannerB in scanners.ToArray())
                {
                    if (TryLocateScanner(scannerA, scannerB, out var locatedScanner))
                    {
                        locatedScanners.Add(locatedScanner);
                        queue.Enqueue(locatedScanner);

                        scanners.Remove(scannerB);
                    }
                }
            }

            return locatedScanners;
        }

        private static bool TryLocateScanner(
            Scanner scannerA, Scanner scannerB, [NotNullWhen(true)] out Scanner? locatedScanner)
        {
            var beaconsInA = scannerA.GetBeaconsInWorld().ToArray();

            var potentialMatchingBeacons = PotentialMatchingBeacons(scannerA, scannerB);

            foreach (var (beaconInA, beaconInB) in potentialMatchingBeacons)
            {
                // Try to find the orientation for B
                var rotatedB = scannerB;
                for (var rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate())
                {
                    // Moving the rotated scanner so that beaconA and beaconB overlaps
                    var beaconInRotatedB = rotatedB.Transform(beaconInB);

                    var locatedB = rotatedB.Translate(new Vector3(
                        beaconInA.X - beaconInRotatedB.X,
                        beaconInA.Y - beaconInRotatedB.Y,
                        beaconInA.Z - beaconInRotatedB.Z
                        ));

                    if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12)
                    {
                        locatedScanner = locatedB;
                        return true;
                    }
                }
            }

            // No match
            locatedScanner = null;
            return false;
        }

        private static IEnumerable<(Vector3 beaconInA, Vector3 beaconInB)> PotentialMatchingBeacons(
            Scanner scannerA, Scanner scannerB)
        {
            // If we use the absolute value of the coordinates,
            // then we don't need to loop over all the beacons.
            // This cancels out the rotation of B.
            // Since they must share 12 and there are only 24 total,
            // we can just loop over 13 of the beacons 

            foreach (var beaconInA in Pick13(scannerA.GetBeaconsInWorld()))
            {
                // Get the absolute value of all of the coordinates of the beacons in A
                var absA = GetAbsoluteCoordinates(
                        scannerA.Translate(new Vector3(-beaconInA.X, -beaconInA.Y, -beaconInA.Z))
                    ).ToHashSet();

                foreach (var beaconInB in Pick13(scannerB.GetBeaconsInWorld()))
                {
                    // Get the absolute value of all of the coordiates of the beacon in B
                    var absB = GetAbsoluteCoordinates(
                            scannerB.Translate(new Vector3(-beaconInB.X, -beaconInB.Y, -beaconInB.Z))
                        );

                    // The scanners must share 12 points * axis (plural)
                    if (absB.Count(d => absA.Contains(d)) >= 3 * 12)
                    {
                        yield return (beaconInA, beaconInB);
                    }
                }
            }
        }

        /// <summary>
        /// Returns 13 items from a list of 24
        /// </summary>
        private static IEnumerable<T> Pick13<T>(IEnumerable<T> ts) =>
            ts.Take(ts.Count() - 11);

        private static IEnumerable<int> GetAbsoluteCoordinates(Scanner scanner)
        {
            return scanner.GetBeaconsInWorld()
                .Select(c => new[] { c.X, c.Y, c.Z })
                .SelectMany(g => g)
                .Select(i => Math.Abs(i));
        }


        private static IEnumerable<Scanner> Parse(string[] input)
        {
            var blockHeadRegex = new Regex(@"^---");
            var coordRegex = new Regex(@"(?<x>-?[0-9]+),(?<y>-?[0-9]+),(?<z>-?[0-9]+)");

            var inBlock = false;
            var beaconsInLocal = new List<Vector3>();
            foreach (var line in input)
            {
                if (blockHeadRegex.IsMatch(line))
                {
                    // Found the top of a block.
                    inBlock = true;
                    continue;
                }

                if (inBlock)
                {
                    var coordsMatch = coordRegex.Match(line);

                    if (coordsMatch.Success)
                    {
                        // Save off this beacon
                        var coordsGroups = coordsMatch.Groups;
                        var newBeacon = new Vector3(
                            coordsGroups["x"].Value, coordsGroups["y"].Value, coordsGroups["z"].Value);

                        beaconsInLocal.Add(newBeacon);
                    }

                    if (line == input.Last() || !coordsMatch.Success)
                    {
                        // Found the end of a block
                        // Create a scanner with all the beacons
                        var newScanner = new Scanner(new Vector3(0, 0, 0), 0, new List<Vector3>(beaconsInLocal));
                        yield return newScanner;


                        // Update the state variables
                        inBlock = false;
                        beaconsInLocal.Clear();
                        continue;
                    }
                }
            }
        }
    }
}
