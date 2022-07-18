using System.Collections;

namespace BlockchainImplementation
{
    public class Blockchain : IEnumerable<IBlock>
    {
        private List<IBlock> _blocks = new List<IBlock>();

        public Blockchain(byte[] difficulty, IBlock genesisBlock)
        {
            Difficulty = difficulty;
            genesisBlock.Hash = genesisBlock.MineHash(difficulty);
            Add(genesisBlock);
        }


        public List<IBlock> Blocks => _blocks;
        public int Count => _blocks.Count;
        public IBlock this[int index] => _blocks[index];
        public byte[] Difficulty { get; set; }

        public void Add(IBlock item)
        {
            if (_blocks.LastOrDefault() != null)
            {
                item.PreviousHash = _blocks.LastOrDefault()?.Hash;
            }

            item.Hash = item.MineHash(Difficulty);

            _blocks.Add(item);
        }

        public bool IsValid()
        {
            return this._blocks
                    .Zip(this._blocks.Skip(1), Tuple.Create)
                    .All(block => block.Item2.IsValid() && 
                        block.Item2.IsPreviousBlockValid(block.Item1));
        }

        public IEnumerator<IBlock> GetEnumerator()
        {
            return _blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _blocks.GetEnumerator();
        }
    }
}
