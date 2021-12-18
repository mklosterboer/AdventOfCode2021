namespace AdventOfCode2021.Problems
{
    internal class OperatorPacket : Packet
    {
        public List<Packet> SubPackets = new();
        private int LengthOfControlBits { get; set; }
        private int LengthType { get; set; }

        private const int TotalLengthControlBits = 22;
        private const int TotalCountControlBits = 18;

        public OperatorPacket(string binaryInput) : base(binaryInput)
        {
            InitSubPackets();
        }

        private void InitSubPackets()
        {
            var workingString = BinaryRepresentation[6..];

            // Get the length type then pop off this bit
            LengthType = int.Parse(workingString[0].ToString());
            workingString = workingString[1..];

            if (LengthType == 0)
            {
                // The next 15 bits contain the total length of the subpackets
                var lengthOfSubPackets = Convert.ToInt32(workingString[0..15], 2);
                workingString = workingString[15..];

                var lengthProcessed = 0;

                while (lengthProcessed < lengthOfSubPackets)
                {
                    // Create the new packet
                    var newPacket = Create(workingString);

                    // Get the length of bits used to create that packet
                    var bitsUsed = newPacket.GetPacketLength();

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
                // The next 11 bits contain the number of subpackets
                var numberOfSubPackets = Convert.ToInt32(workingString[0..11], 2);
                workingString = workingString[11..];

                var numberOfSubPacketsProcessed = 0;

                while (numberOfSubPacketsProcessed < numberOfSubPackets)
                {
                    // Create the new packet
                    var newPacket = Create(workingString);

                    // Get the length of bits used to create that packet
                    var bitsUsed = newPacket.GetPacketLength();

                    // Update working string to move to the new index
                    workingString = workingString[bitsUsed..];

                    // Increment number of subpackets processed
                    numberOfSubPacketsProcessed++;

                    // Add the subpacket to this packet's list
                    SubPackets.Add(newPacket);
                }
            }
        }

        public override int GetPacketLength()
        {
            // packet length of all subpackets
            var subpacketLengths = SubPackets.Sum(x => x.GetPacketLength());

            // Depending on the packet length type, there will be a different
            // number of control bits.
            var lengthOfControlBits = LengthType == 0
                ? TotalLengthControlBits
                : TotalCountControlBits;

            return subpacketLengths + lengthOfControlBits;
        }

        public override int GetVersionSumOfSubPackets()
        {
            return Version + SubPackets.Sum(x => x.GetVersionSumOfSubPackets());
        }

        public override long GetValue()
        {
            switch (TypeId)
            {
                case 0:
                    return SubPackets.Sum(x => x.GetValue());
                case 1:
                    var acc = SubPackets[0].GetValue();
                    if (SubPackets.Count > 1)
                    {
                        for (var i = 1; i < SubPackets.Count; i++)
                        {
                            acc *= SubPackets[i].GetValue();
                        }
                    }
                    return acc;
                case 2:
                    return SubPackets.Min(x => x.GetValue());
                case 3:
                    return SubPackets.Max(x => x.GetValue());
                case 5:
                    return SubPackets.First().GetValue() > SubPackets.Last().GetValue()
                        ? 1
                        : 0;
                case 6:
                    return SubPackets.First().GetValue() < SubPackets.Last().GetValue()
                        ? 1
                        : 0;
                case 7:
                    return SubPackets.First().GetValue() == SubPackets.Last().GetValue()
                        ? 1
                        : 0;
                default:
                    break;
            }

            return 0L;
        }
    }
}
