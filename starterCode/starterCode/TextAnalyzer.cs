using System;
using System.Linq;

namespace starterCode
{
    public class TextAnalyzer
    {
        private BinarySearchTree<WordInfo>? _bst;

        public void Run()
        {
            LoadFile();
            while(true)
            {
                Console.WriteLine("\n1 Words(any) 2 Unique 3 Alpha 4 Longest 5 MostFreq");
                Console.WriteLine("6 LinesFor  7 FreqFor 8 LoadFile 0 Exit");
                Console.Write("Choice: ");
                switch(Console.ReadLine())
                {
                    case "0":return;
                    case "1": _bst!.PreOrder().ToList().ForEach(Console.WriteLine); break;
                    case "2": Console.WriteLine($"Unique: {_bst!.Count}"); break;
                    case "3": Alpha(); break;
                    case "4": Longest(); break;
                    case "5": MostFreq(); break;
                    case "6": LinesFor(); break;
                    case "7": FreqFor(); break;
                    case "8": LoadFile(); break;
                    default: Console.WriteLine("Bad choice"); break;
                }
            }
        }

        private void LoadFile()
        {
            while(true)
            {
                Console.Write("Text file (blank=mobydick.txt): ");
                string f=Console.ReadLine();
                if(string.IsNullOrWhiteSpace(f)) f="mobydick.txt";
                try { _bst=FileParser.Parse(f); Console.WriteLine($"Loaded {f} ({_bst.Count} unique)"); return; }
                catch(Exception ex){Console.WriteLine(ex.Message);}
            }
        }

        private void Alpha()
        {
            Console.Write("A‑Z or Z‑A? (A/Z): ");
            bool desc=Console.ReadLine()?.Trim().Equals("z",StringComparison.OrdinalIgnoreCase)??false;
            var seq=desc?_bst!.InOrder().Reverse():_bst!.InOrder();
            foreach(var w in seq) Console.WriteLine(w);
        }
        private void Longest()   => Console.WriteLine(_bst!.InOrder().OrderByDescending(w=>w.Word.Length).First());
        private void MostFreq()  => Console.WriteLine(_bst!.InOrder().OrderByDescending(w=>w.Count).First());
        private void LinesFor()
        {
            Console.Write("Word: "); string w=Console.ReadLine().ToLowerInvariant();
            var probe=new WordInfo(w,0);
            if(_bst!.TryGet(probe,out var info)) Console.WriteLine(string.Join(", ",info.LineNumbers));
            else Console.WriteLine("Not found.");
        }
        private void FreqFor()
        {
            Console.Write("Word: "); string w=Console.ReadLine().ToLowerInvariant();
            var probe=new WordInfo(w,0);
            if(_bst!.TryGet(probe,out var info)) Console.WriteLine(info.Count);
            else Console.WriteLine("Not found.");
        }
    }
}
