namespace BlockchainImplementation
{
    public interface IBlock
    {
        public byte[] Data { get; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public byte[] PreviousHash { get; set; }
        public DateTime TimeStamp { get; }


        void ChangeData(byte[] data);
        public byte[] MineHash(byte[] difficulty);
        public byte[] GenerateHash();
        public bool IsValid();
        public bool IsPreviousBlockValid(IBlock previousBlock);
        public void ChangePreviousHash(byte[] previousBlockHash);
    }
}