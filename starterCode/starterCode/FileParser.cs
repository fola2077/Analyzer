using System;
using System.IO;
using System.Text.RegularExpressions;

namespace starterCode
{
    public static class FileParser
    {
        private static readonly char[] DELIMS =
            { ' ', '\t', ',', '"', ':', ';', '?', '!', '-', '.', '\'', '*', '—','–' };

        private static readonly Regex RX =
            new(@"\b(?:[a-z]{2,}|[ai])\b", RegexOptions.IgnoreCase);

        public static BinarySearchTree<WordInfo> Parse(string path)
        {
            var tree=new BinarySearchTree<WordInfo>();
            string[] lines=File.ReadAllLines(path);

            for(int i=0;i<lines.Length;i++)
            {
                foreach(string raw in lines[i].Split(DELIMS,StringSplitOptions.RemoveEmptyEntries))
                {
                    string w=raw.ToLowerInvariant();
                    if(!RX.IsMatch(w)) continue;

                    var probe=new WordInfo(w,i+1);
                    if(tree.TryGet(probe,out var existing)) existing.AddOccurrence(i+1);
                    else tree.Insert(probe);
                }
            }
            return tree;
        }
    }
}
