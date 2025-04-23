using System;
using System.IO;
using System.Text.RegularExpressions;

namespace starterCode
{
    public static class FileParser
    {
        private static readonly char[] DELIMS =
            { ' ', '\t', ',', '"', ':', ';', '?', '!', '-', '.', '\'', '*', '—','–', '(', ')', '[', ']', '{', '}', '/', '\\' };

        private static readonly Regex RX =
            new(@"\b(?:[a-z]{2,}|[ai])\b", RegexOptions.IgnoreCase);


        public static BinarySearchTree<WordInfo> ParseWithProgress(string path)
        {
            var tree  = new BinarySearchTree<WordInfo>();
            string[] lines = File.ReadAllLines(path);
            int total = lines.Length;

            for (int i = 0; i < total; i++)
            {
                ParseLineInto(tree, lines[i], i + 1);
                if (i % 100 == 0) ShowBar(i, total);
            }
            ShowBar(total, total);
            Console.WriteLine();           // newline after bar
            return tree;
        }

        // helper pulled out of previous Parse()
        public static void ParseLineInto(BinarySearchTree<WordInfo> tree,
                                         string line, int lineNumber)
        {
            foreach (string raw in line.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = raw.ToLowerInvariant();
                if (!RX.IsMatch(w)) continue;

                var probe = new WordInfo(w, lineNumber);
                if (tree.TryGet(probe, out var existing))
                    existing.AddOccurrence(lineNumber);
                else
                    tree.Insert(probe);
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