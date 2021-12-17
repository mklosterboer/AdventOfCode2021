namespace AdventOfCode2021.Problems
{
    internal class OperatorPacket : Packet2
    {
        public List<Packet> SubPackets = new();
        private string BinaryRepresentation { get; init; }
        private int Version { get; set; }
        private int TypeId { get; set; }
        private int LengthOfControlBits { get; set; }

        public OperatorPacket(string binaryInput)
        {
            BinaryRepresentation = binaryInput;

            var versionBits = BinaryRepresentation[0..3];
            Version = Convert.ToInt32(versionBits, 2);

            var typeIdBits = BinaryRepresentation[3..6];
            TypeId = Convert.ToInt32(typeIdBits, 2);

            InitSubPackets();
        }

        private void InitSubPackets()
        {
            var workingString = BinaryRepresentation[6..];

            // Get the length type then pop off this bit
            var lengthType = workingString[0];
            workingString = workingString[1..];

            if (lengthType == '0')
            {
                LengthOfControlBits = 6 + 16;
                // The next 15 bits contain the total length of the subpackets
                var lengthOfSubPackets = Convert.ToInt32(workingString[0..15], 2);
                workingString = workingString[15..];

                var lengthProcessed = 0;

                while (lengthProcessed < lengthOfSubPackets)
                {
                    // Create the new packet
                    var newPacket = new Packet(workingString);

                    // Get the length of bits used to create that packet
                    var bitsUsed = newPacket.GetBitLength();

                    // Update working string to move to the new index
                    workingString = workingString[bitsUsed..];

                    // Update length processed with the number of bits used
                    lengthProcessed += bitsUsed;

                    // Add the subpacket to this packet's list
                    SubPackets.Add(newPacket);
                }
            }
            else
            {
                // This is the number of bits used by this packet
                LengthOfControlBits = 6 + 12;

                // The next 11 bits contain the number of subpackets
                var numberOfSubPackets = Convert.ToInt32(workingString[0..11], 2);
                workingString = workingString[11..];

                var numberOfSubPacketsProcessed = 0;

                while (numberOfSubPacketsProcessed < numberOfSubPackets)
                {
                    // Create the new packet
                    var newPacket = new Packet(workingString);

                    // Get the length of bits used to create that packet
                    var bitsUsed = newPacket.GetBitLength();

                    // Update working string to move to the new index
                    workingString = workingString[bitsUsed..];

                    // Increment number of subpackets processed
                    numberOfSubPacketsProcessed++;

                    // Add the subpacket to this packet's list
                    SubPackets.Add(newPacket);
                }
            }
        }

        public override int GetBitLength()
        {
            // bit length of value subpackets
            var subpacketLengths = SubPackets.Sum(x => x.GetBitLength());

            return subpacketLengths + LengthOfControlBits;
        }

        public override int GetVersionSumOfSubPackets()
        {
            return Version + SubPackets.Sum(x => x.GetVersionSumOfSubPackets());
        }
    }
}
