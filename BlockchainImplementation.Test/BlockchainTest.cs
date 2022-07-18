using BlockchainImplementation.Implementation;
using Xunit;

namespace BlockchainImplementation.Test
{
    public class BlockchainTest
    {
        [Fact]
        public void Can_Add_Block_InBlockchain()
        {
            var difficulty = new byte[] { 0x45 }; 

            var genesisBlock = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            var blockchain = new Blockchain(difficulty, genesisBlock);

            var isBlockchainValid = blockchain.IsValid();
            Assert.True(isBlockchainValid);

            var block = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            blockchain.Add(block);

            Assert.True(blockchain.IsValid());
        }

        [Fact]
        public void CannotChangeValueOfBlock_InBlockchain()
        {
            var difficulty = new byte[] { 0x45 };

            var genesisBlock = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            var blockchain = new Blockchain(difficulty, genesisBlock);

            var isBlockchainValid = blockchain.IsValid();
            Assert.True(isBlockchainValid);

            var block = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            blockchain.Add(block);

            Assert.True(blockchain.IsValid());

            blockchain.Blocks[0].ChangeData(new byte[] { 0x45 });
            Assert.False(blockchain.Blocks[0].IsValid());
            Assert.False(blockchain.Blocks[1].IsPreviousBlockValid(blockchain.Blocks[0]));
            Assert.False(blockchain.IsValid());

            blockchain.Blocks[0].MineHash(difficulty);
            Assert.True(blockchain.Blocks[0].IsValid());
            Assert.False(blockchain.Blocks[1].IsPreviousBlockValid(blockchain.Blocks[0]));
            Assert.False(blockchain.IsValid());
        }

        [Fact]
        public void Can_ReMineBlocks_After_ChangeData()
        {
            var difficulty = new byte[] { 0x45 };

            var genesisBlock = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            var blockchain = new Blockchain(difficulty, genesisBlock);

            var isBlockchainValid = blockchain.IsValid();
            Assert.True(isBlockchainValid);

            var block = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            blockchain.Add(block);

            Assert.True(blockchain.IsValid());

            blockchain.Blocks[0].ChangeData(new byte[] { 0x45 });
            Assert.False(blockchain.Blocks[0].IsValid());
            Assert.False(blockchain.Blocks[1].IsPreviousBlockValid(blockchain.Blocks[0]));
            Assert.False(blockchain.IsValid());

            for (int i = 0; i < blockchain.Blocks.Count(); i++)
            {
                blockchain.Blocks[i].MineHash(difficulty);
                Assert.True(blockchain.Blocks[i].IsValid());

                if (blockchain.Blocks[i].PreviousHash.Length > 1)
                {
                    blockchain.Blocks[i].ChangePreviousHash(blockchain.Blocks[i - 1].GenerateHash());
                    blockchain.Blocks[i].MineHash(difficulty);

                    Assert.True(blockchain.Blocks[i].IsValid());
                    Assert.True(blockchain.Blocks[i].IsPreviousBlockValid(blockchain.Blocks[i-1]));
                }
            }
                        
            Assert.True(blockchain.IsValid());
        }
    }
}
