// TextAnalyzer.cs
using System;
using System.Linq;

namespace TextAnalyser
{
    public class TextAnalyzer
    {
        private static readonly string Banner =
        @"
        ╔════════════════════════════════════════════════════════════╗
        ║                                                            ║
        ║          ★  T  E  X  T     A  N  A  L  Y  Z  E  R  ★       ║
        ║                                                            ║
        ╚════════════════════════════════════════════════════════════╝"
        .Replace("\r", "").Replace("\n    ", "\n");

        private static readonly ConsoleColor Accent = ConsoleColor.Cyan;

        private static void WriteAccent(string text)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = Accent;
            Console.Write(text);
            Console.ForegroundColor = old;
        }

        private static void WriteOk(string text) => WriteWith(text, ConsoleColor.Green);
        private static void WriteErr(string text) => WriteWith(text, ConsoleColor.Red);

        private static void WriteWith(string text, ConsoleColor fg)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = fg;
            Console.Write(text);
            Console.ForegroundColor = old;
        }

        private BinaryTree? _tree;

        public void Run()
        {
            ShowBanner();
            LoadFile();

            while (true)
            {
                ShowMenu();
                switch (Console.ReadLine()?.Trim().ToLowerInvariant())
                {
                    case "0": return;
                    case "1": foreach (var word in _tree!.PreOrder()) Console.WriteLine(word); break;
                    case "2": WriteAccent($"Unique words: {_tree!.Count}\n"); break;
                    case "3": ListAlphabetical(); break;
                    case "4": ShowLongest(); break;
                    case "5": ShowMostFrequent(); break;
                    case "6": ShowLineNumbers(); break;
                    case "7": ShowWordFrequency(); break;
                    case "8": LoadFile(); break;
                    default: WriteErr("✖  Please enter a choice from 0-8.\n"); break;
                }
            }
        }

        private static void ShowBanner()
        {
            Console.Clear();
            WriteAccent(Banner);
            Console.WriteLine();
        }

        private static void ShowMenu()
        {
            Console.WriteLine(@"
────────────────────────────────────────────────────────────
  1  Display every word (pre-order, unsorted)     
  2  Show HOW MANY unique words are stored       
  3  List words ALPHABETICALLY (paged)           
  4  Show the LONGEST word                       
  5  Show the MOST-FREQUENT word                 
  6  Show LINE NUMBERS for a given word          
  7  Show FREQUENCY for a given word             
  8  Load a DIFFERENT text file                  
  0  Exit program                                
────────────────────────────────────────────────────────────");
            Console.Write("Your choice: ");
        }

        private void LoadFile()
        {
            while (true)
            {
                Console.Write("\nEnter path to .txt file (Enter for default 'mobydick.txt'): ");
                string? input = Console.ReadLine();
                string path = string.IsNullOrWhiteSpace(input) ? "mobydick.txt" : input.Trim();

                try
                {
                    _tree = FileParser.ParseWithProgress(path);
                    WriteOk($"\n✓  Loaded '{path}' ");
                    WriteAccent($"({_tree.Count} unique words)\n\n");
                    return;
                }
                catch (Exception ex)
                {
                    WriteErr($"✖  {ex.Message}\n");
                }
            }
        }

        private void ListAlphabetical()
        {
            Console.Write("List A→Z or Z→A? (A/Z): ");
            bool desc = (Console.ReadLine()?.Trim().ToLowerInvariant() ?? "a") == "z";
            var list = _tree!.InOrder().ToList();
            if (desc) list.Reverse();
            foreach (var word in list) Console.WriteLine(word);
        }

        private void ShowLongest() =>
            WriteAccent(_tree!.FindLongestWord() + "\n");

        private void ShowMostFrequent() =>
            WriteAccent(_tree!.FindMostFrequent() + "\n");

        private void ShowLineNumbers()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { WriteErr("No word entered.\n"); return; }

            var info = _tree!.Search(input.ToLowerInvariant());
            if (info != null)
                WriteAccent($"{info.Word} appears on line(s): {string.Join(", ", info.LineNumbers)}\n");
            else
                WriteErr("Word not found.\n");
        }

        private void ShowWordFrequency()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { WriteErr("No word entered.\n"); return; }

            var info = _tree!.Search(input.ToLowerInvariant());
            if (info != null)
                WriteAccent($"{info.Word} appears {info.Frequency} time(s).\n");
            else
                WriteErr("Word not found.\n");
        }
    }
}
