using System.Collections.Generic;

namespace Archivator
{
    public class HuffmanNode
    {
        public HuffmanNode()
        {
        }

        public HuffmanNode(char symbol, int frequency)
        {
            Frequency = frequency;
            Symbol = symbol;
        }

        public char Symbol { get; init; }
        public int Frequency { get; init; }
        public HuffmanNode Right { get; init; }
        public HuffmanNode Left { get; init; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            // Leaf
            if (Right == null && Left == null) return symbol.Equals(Symbol) ? data : null;

            List<bool> left = null;
            List<bool> right = null;

            if (Left != null)
            {
                var leftPath = new List<bool>();
                leftPath.AddRange(data);
                leftPath.Add(false);

                left = Left.Traverse(symbol, leftPath);
            }

            if (Right == null) return left ?? right;
            var rightPath = new List<bool>();
            rightPath.AddRange(data);
            rightPath.Add(true);
            right = Right.Traverse(symbol, rightPath);

            return left ?? right;
        }
    }
}