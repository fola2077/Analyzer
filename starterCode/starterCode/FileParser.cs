// FileParser.cs
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace starterCode
{
    public class FileParser
    {
        private static readonly char[] DELIMS =
        { ' ', '\t', ',', '"', ':', ';', '?', '!', '-', '.', '\'', '*', '—','–', '(', ')', '[', ']', '{', '}', '/', '\\' };

        private static readonly Regex RX =
            new(@"\b(?:[a-z]{2,}|[ai])\b", RegexOptions.IgnoreCase);

        public static BinaryTree ParseWithProgress(string path)
        {
            var tree = new BinaryTree();
            string[] lines = File.ReadAllLines(path);
            int total = lines.Length;

            for (int i = 0; i < total; i++)
            {
                ParseLineInto(tree, lines[i], i + 1);
                if (i % 100 == 0) ShowBar(i, total);
            }
            ShowBar(total, total);
            Console.WriteLine();
            return tree;
        }

        private static void ParseLineInto(BinaryTree tree, string line, int lineNumber)
        {
            foreach (string raw in line.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries))
            {
                string word = raw.ToLowerInvariant();
                if (!RX.IsMatch(word)) continue;
                tree.Insert(word, lineNumber);
            }
        }

        private static void ShowBar(int done, int total)
        {
            const int width = 50;
            int filled = done * width / total;
            Console.Write("\r[");
            Console.Write(new string('#', filled));
            Console.Write(new string('·', width - filled));
            Console.Write($"] {done}/{total}");
        }
    }
}
