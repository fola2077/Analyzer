// WordInfo.cs
using System;
using System.Collections.Generic;

namespace TextAnalyser
{
    // Class to store information about each word
    public class WordInfo
    {
        public string Word { get; }
        public int Frequency { get; private set; } = 1;
        private readonly List<int> _lineNumbers = new();
        public IReadOnlyList<int> LineNumbers => _lineNumbers;

        public WordInfo(string word, int line)
        {
            Word = word;
            _lineNumbers.Add(line);
        }

        public void AddOccurrence(int line)
        {
            Frequency++;
            _lineNumbers.Add(line);
        }

        public override string ToString() => $"{Word,-20} {Frequency,5}";
    }
} 