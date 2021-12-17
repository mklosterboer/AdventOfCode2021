using AdventOfCode2021.Utilities;
using System.Collections.Immutable;

namespace AdventOfCode2021.Problems
{
    public class Day16 : Problem
    {
        protected override string InputName => "Actual";

        public override object PartOne()
        {
            var input = GetFirstRow();
            var packet = Packet.InitFromHexInput(input);

            return packet.GetVersionSumOfSubPackets();
        }

        public override object PartTwo()
        {
            return "";
        }

        //private int SumVersions(Packet packet)
        //{
        //    if (packet.IsValuePacket)
        //    {
        //        return packet.Version;
        //    }
        //    else
        //    {
        //        return packet.Version + packet.SubPackets.Select(SumVersions).Sum();
        //    }
        //}
    }

    public static class Day16Helpers
    {
        private static readonly ImmutableDictionary<char, string> HexToBinaryDictionary =
            new Dictionary<char, string>()
            {
                ['0'] = "0000",
                ['1'] = "0001",
                ['2'] = "0010",
                ['3'] = "0011",
                ['4'] = "0100",
                ['5'] = "0101",
                ['6'] = "0110",
                ['7'] = "0111",
                ['8'] = "1000",
                ['9'] = "1001",
                ['A'] = "1010",
                ['B'] = "1011",
                ['C'] = "1100",
                ['D'] = "1101",
                ['E'] = "1110",
                ['F'] = "1111",
            }.ToImmutableDictionary();

        public static string HexToBinary(string input)
        {
            var result = "";

            foreach (var c in input)
            {
                result += HexToBinaryDictionary[c];
            }

            return result;
        }
        public static string BinaryToHex(string input)
        {
            var paddedBits = PadValueWithZeros(input);
            return HexToBinaryDictionary.First(c => c.Value == paddedBits).Key.ToString();
        }

        public static string PadValueWithZeros(string input)
        {
            var result = "";

            var originalLength = input.Length;

            for (var i = 0; i < 4 - originalLength; i++)
            {
                result += "0";
            }

            result += input;

            return result;
        }
    }
}
