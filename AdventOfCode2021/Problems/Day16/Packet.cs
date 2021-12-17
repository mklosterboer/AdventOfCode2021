namespace AdventOfCode2021.Problems
{
    public abstract class Packet
    {
        protected string BinaryRepresentation { get; init; }
        protected int Version { get; set; }
        protected int TypeId { get; set; }

        public Packet(string binaryInput)
        {
            BinaryRepresentation = binaryInput;

            var versionBits = BinaryRepresentation[0..3];
            Version = Convert.ToInt32(versionBits, 2);

            var typeIdBits = BinaryRepresentation[3..6];
            TypeId = Convert.ToInt32(typeIdBits, 2);
        }

        public static Packet Create(string binaryInput)
        {
            var typeIdBits = binaryInput[3..6];
            var typeId = Convert.ToInt32(typeIdBits, 2);

            if (typeId == 4)
            {
                return new ValuePacket(binaryInput);
            }
            else
            {
                return new OperatorPacket(binaryInput);
            }
        }

        public abstract int GetPacketLength();
        public abstract int GetVersionSumOfSubPackets();
        public abstract long GetValue();
    }
}
