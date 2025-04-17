using System;
using System.Collections.Generic;

namespace starterCode
{
    public sealed class WordInfo : IComparable<WordInfo>
    {
        public string Word { get; }
        public int Count { get; private set; } = 1;
        private readonly List<int> _lines = new();
        public IReadOnlyList<int> LineNumbers => _lines;

        public WordInfo(string word, int line)  { Word = word;  _lines.Add(line); }

        public void AddOccurrence(int line) { Count++; _lines.Add(line); }

        public int CompareTo(WordInfo? other) =>
            string.Compare(Word, other!.Word, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => $"{Word,-20} {Count,5}";
    }
}
