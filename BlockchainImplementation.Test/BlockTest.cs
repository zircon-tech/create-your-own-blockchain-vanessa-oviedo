using BlockchainImplementation.Implementation;
using Xunit;

namespace BlockchainImplementation.Test
{
    public class BlockTest
    {
        [Fact]
        public void IsValid()
        {
            var block = new Block(new byte[] { 0x00 , 0x00 , 0x00 , 0x00 , 0x00 });
            var isValid = block.IsValid();
            Assert.True(isValid);
        }

        [Fact]
        public void IsInValid_After_ChangingData()
        {
            var block = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            Assert.True(block.IsValid());

            block.ChangeData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 });
            Assert.False(block.IsValid());
        }

        [Fact]
        public void IsValid_After_Mining()
        {
            var block = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            Assert.True(block.IsValid());

            block.ChangeData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 });
            Assert.False(block.IsValid());

            block.MineHash(new byte[] { 0x45 });
            Assert.True(block.IsValid());
        }
    }
}