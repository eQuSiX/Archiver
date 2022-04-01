using System;
using System.Collections.Generic;

namespace Archivator
{
    public class LzwStringTable
    {
        private readonly int _maxCode;
        private int _nextAvailableCode = 256;

        private readonly Dictionary<string, int> table = new();

        public LzwStringTable(int numBytesPerCode)
        {
            _maxCode = (1 << (8 * numBytesPerCode)) - 1;
        }

        public void AddCode(string s)
        {
            if (_nextAvailableCode <= _maxCode)
            {
                if (s.Length != 1 && !table.ContainsKey(s))
                    table[s] = _nextAvailableCode++;
            }
            else
            {
                throw new Exception("LZW string table overflow");
            }
        }

        public int GetCode(string s)
        {
            return s.Length == 1 ? s[0] : table[s];
        }

        public bool Contains(string s)
        {
            return s.Length == 1 || table.ContainsKey(s);
        }
    }
}