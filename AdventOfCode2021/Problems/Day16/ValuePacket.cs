namespace AdventOfCode2021.Problems
{
    internal class ValuePacket : Packet2
    {
        private string BinaryRepresentation { get; init; }
        private int Version { get; set; }
        private int TypeId { get; set; }
        private int BitLength { get; set; }
        private long LiteralPacketValue { get; set; }

        public ValuePacket(string binaryInput)
        {
            BinaryRepresentation = binaryInput;

            var versionBits = BinaryRepresentation[0..3];
            Version = Convert.ToInt32(versionBits, 2);

            var typeIdBits = BinaryRepresentation[3..6];
            TypeId = Convert.ToInt32(typeIdBits, 2);

            InitLiteralPacketValue();
        }

        private void InitLiteralPacketValue()
        {
            var _bitLength = 6;
            var workingString = BinaryRepresentation[6..];

            var packetValueString = "";

            var bitPrefix = 1;

            while (bitPrefix == 1)
            {
                var nextBit = workingString[0..5];

                // The last bit will have a "0" as a prefix
                bitPrefix = int.Parse(nextBit[0].ToString());

                packetValueString += nextBit[1..];

                workingString = workingString[5..];
                _bitLength += 5;
            }

            LiteralPacketValue = Convert.ToInt64(packetValueString, 2);
            BitLength = _bitLength;
        }

        public override int GetBitLength()
        {
            return BitLength;
        }

        public override int GetVersionSumOfSubPackets()
        {
            return Version;
        }
    }
}
