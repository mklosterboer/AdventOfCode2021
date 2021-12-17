namespace AdventOfCode2021.Problems
{
    internal class ValuePacket : Packet
    {
        private int PacketLength { get; set; }
        private long LiteralPacketValue { get; set; }

        public ValuePacket(string binaryInput) : base(binaryInput)
        {
            // Initialize BitLength with header bit length
            PacketLength = 6;

            InitLiteralPacketValue();
        }

        private void InitLiteralPacketValue()
        {
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

                // Increment to keep track of how long this packet is
                PacketLength += 5;
            }

            LiteralPacketValue = Convert.ToInt64(packetValueString, 2);
        }

        public override int GetPacketLength() => PacketLength;

        public override int GetVersionSumOfSubPackets() => Version;

        public override long GetValue() => LiteralPacketValue;
    }
}
