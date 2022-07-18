using System.Diagnostics;
using System.Security.Cryptography;

namespace BlockchainImplementation.Implementation
{
    public class Block : IBlock
    {
        public byte[] Data { get; private set; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public byte[] PreviousHash { get; set; }
        public DateTime TimeStamp { get; }

        public Block(byte[] data)
        {
            Data = data ?? throw new ArgumentException(nameof(data));
            Nonce = 0; 
            PreviousHash = new byte[] { 0x00 };
            TimeStamp = DateTime.Now;

            Hash = GenerateHash();
        }

        public void ChangeData(byte[] data)
        {
            Data = data ?? throw new ArgumentException(nameof(data));
        }

        public byte[] MineHash(byte[] difficulty)
        {
            if (difficulty == null)
            {
                throw new ArgumentNullException(nameof(difficulty));
            }

            var hash = new byte[0];
            int diffLength = difficulty.Length;

            while (!hash.Take(diffLength).SequenceEqual(difficulty))
            {
                this.Nonce++;
                Trace.WriteLine($"Current Nonce is {this.Nonce}");
                hash = GenerateHash();
            }

            Hash = hash;
            return hash;
        }

        public byte[] GenerateHash()
        {
            using (SHA256 sha = SHA256.Create())
            {
                using (var stream = new MemoryStream())
                {
                    using (var writter = new BinaryWriter(stream))
                    {
                        writter.Write(this.Data);
                        writter.Write(this.Nonce);
                        writter.Write(this.TimeStamp.ToBinary());
                        writter.Write(this.PreviousHash);

                        return sha.ComputeHash(stream.ToArray());
                    }
                }
            }
        }

        public bool IsValid()
        {
            var bk = GenerateHash();
            var isBlockValid =  Hash.SequenceEqual(bk);
            return isBlockValid; 
        }

        public bool IsPreviousBlockValid(IBlock previousBlock)
        {
            if (previousBlock == null)
            {
                throw new ArgumentNullException(nameof(previousBlock));
            }

            var previousBlockHash = previousBlock.GenerateHash();
            var isPreviousBlockValid =  previousBlock.IsValid() && PreviousHash.SequenceEqual(previousBlockHash);
            return isPreviousBlockValid;
        }

        public void ChangePreviousHash(byte[] hash)
        {
            PreviousHash = hash;
        }

        public override string ToString()
        {
            return $"{BitConverter.ToString(Hash).Replace("-", string.Empty)}:\n" +
                    $"{BitConverter.ToString(PreviousHash).Replace("-", string.Empty)}:\n" +
                    $"{Nonce}:\n" +
                    $"{TimeStamp}:\n";
        }
    }
}
