using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Archivator
{
    public abstract class Algorithm
    {
        private const string PathToDirectory = "путь_к_папке/testSource"; //надо заменить путь до папки testSource
        private readonly List<string> _filenames = new() {"input.txt", "archive.txt", "output.txt"};
        protected virtual string Prefix => "";
        public abstract void Compress(string inputFile, string encodedFile);
        public abstract void Decompress(string encodedFile, string decodedFile);

        protected static byte[] ReadFile(string filename)
        {
            using var inputStream = File.OpenRead($"{PathToDirectory}{filename}");
            var input = new byte[inputStream.Length];
            inputStream.Read(input, 0, input.Length);
            return input;
        }

        protected static void WriteFile(string filename, byte[] output)
        {
            using var outputStream = new FileStream($"{PathToDirectory}{filename}", FileMode.OpenOrCreate);
            outputStream.Write(output, 0, output.Length);
            Console.WriteLine("Text Written in The File");
        }

        public List<string> GetFilenames(int testNumber)
        {
            return (from name in _filenames let prefix = $"/{Prefix}_{testNumber}_" select prefix + name).ToList();
        }
    }
}