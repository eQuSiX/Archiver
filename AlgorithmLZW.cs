using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivator
{
    public class AlgorithmLZW : Algorithm
    {
        private ArrayList Dictionary { get; set; }
        protected override string Prefix => "l";

        public override void Compress(string inputFile, string encodedFile)
        {
            var textFromFile = Encoding.Default.GetString(ReadFile(inputFile));
            // building a dictionary
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(((char) i).ToString(), i);

            var w = string.Empty;
            var compressed = new List<int>();

            foreach (var c in textFromFile)
            {
                var wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // add a new character to the archive
                    compressed.Add(dictionary[w]);
                    // add a new substring to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // complement the remaining character, if any
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            WriteFile(encodedFile, compressed.SelectMany(BitConverter.GetBytes).ToArray());
        }

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var bytes = ReadFile(encodedFile);
            var compressed = Enumerable.Range(0, bytes.Length / 4)
                .Select(i => BitConverter.ToInt32(bytes, i * 4))
                .ToList();

            // build a dictionary
            var dictionary = new Dictionary<int, string>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(i, ((char) i).ToString());

            var w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            var decompressed = new StringBuilder(w);

            foreach (var k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // add a new substring to the dictionary
                if (entry == null) continue;
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            // convert string to bytes, write array of bytes to file
            var output = Encoding.Default.GetBytes(decompressed.ToString());

            // save the decoded file
            WriteFile(decodedFile, output);
        }
    }
}