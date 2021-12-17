namespace AdventOfCode2021.Problems
{
    public abstract class Packet2
    {
        public static Packet2 Create(string binaryInput)
        {
            var typeIdBits = binaryInput[3..6];
            var typeId = Convert.ToInt32(typeIdBits, 2);

            if (typeId == 4)
            {
                return new ValuePacket(binaryInput);
            }
            else return new OperatorPacket(binaryInput);
        }

        public abstract int GetBitLength();
        public abstract int GetVersionSumOfSubPackets();
    }

    public class Packet
    {
        public bool IsValuePacket => TypeId == 4;
        public List<Packet> SubPackets = new();

        private int Version { get; set; }
        private int TypeId { get; set; }
        private string BinaryRepresentation { get; init; }
        private long LiteralPacketValue { get; set; }
        private string LengthType { get; set; }
        private int BitLength { get; set; }

        private int LengthOfControlBits { get; set; }

        public Packet(string binaryInput)
        {
            BinaryRepresentation = binaryInput;

            var versionBits = BinaryRepresentation[0..3];
            Version = Convert.ToInt32(versionBits, 2);

            var typeIdBits = BinaryRepresentation[3..6];
            TypeId = Convert.ToInt32(typeIdBits, 2);

            if (IsValuePacket)
            {
                // This is a value packet
                InitLiteralPacketValue();
            }
            else
            {
                // This is a operator packet
                InitSubPackets();
            }
        }

        public static Packet InitFromHexInput(string hexInput)
        {
            var binaryRepresentation = Day16Helpers.HexToBinary(hexInput);

            return new Packet(binaryRepresentation);
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

        public int GetBitLength()
        {
            if (IsValuePacket)
            {
                return BitLength;
            }
            else
            {
                // bit length of value subpackets
                var subpacketLengths = SubPackets.Sum(x => x.GetBitLength());

                return subpacketLengths + LengthOfControlBits;
            }
        }

        public int GetVersionSumOfSubPackets()
        {
            if (IsValuePacket)
            {
                return Version;
            }
            else
            {
                return Version + SubPackets.Sum(x => x.GetVersionSumOfSubPackets());
            }
        }
    }
}
