using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivator
{
    public class AlgorithmHuffman : Algorithm
    {
        protected override string Prefix  => "h";
        private HuffmanTree HuffmanTree { get; set; }

        public override void Compress(string inputFile, string encodedFile)
        {
            var textFromFile = Encoding.Default.GetString(ReadFile(inputFile));
            
            // create a Huffman tree based on the received file
            HuffmanTree = new HuffmanTree();
            HuffmanTree.Build(textFromFile);

            // compress the file
            var encoded = HuffmanTree.Encode(textFromFile);

            // output the result
            /*Console.Write("Encoded: \n");
            foreach (bool bit in encoded)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            Console.WriteLine();
            */
            // convert string to bytes, write array of bytes to file
            var aBld = new StringBuilder();
            var a = Encoding.Default.GetBytes(HuffmanTree.PrintTree(HuffmanTree.Root, aBld) + "###");
            var output = a.Concat(BitArrayToByteArray(encoded)).ToArray();
            
            // save compressed file
            WriteFile(encodedFile, output);
        }

        private static Tuple<string, byte[]> FindTree(IReadOnlyList<byte> c)
        {
            var o = Encoding.Default.GetBytes("###");
            var index = 0;
            for (var i = 0; i < c.Count; i++)
            {
                if (c[i] != o[0] || c[i + 1] != o[1] || c[i + 2] != o[2]) continue;
                index = i;
                break;
            }

            var tree = string.Join("", Encoding.Default.GetString(c.Take(index).ToArray()));
            var source = c.Skip(index + 3).ToArray();
            return Tuple.Create(tree, source);
        }

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var result = FindTree(ReadFile(encodedFile));
            var bits = new BitArray(result.Item2);

            //var b = HuffmanTree.TreeFromString(a).Root;
            // Decoding the file
            var decoded = HuffmanTree.Decode(bits);
            //Console.WriteLine("Decoded: \n" + decoded);

            // convert string to bytes, write array of bytes to file
            var output = Encoding.Default.GetBytes(decoded);
            
            // save the decoded file
            WriteFile(decodedFile, output);
        }

        private static IEnumerable<byte> BitArrayToByteArray(BitArray bits)
        {
            var ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}