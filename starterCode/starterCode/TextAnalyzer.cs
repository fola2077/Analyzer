using System;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace starterCode
{
    public class TextAnalyzer
    {
        /* ───── 1. Single banner with border ───────────────────────── */
    private const string Banner = @"
    ╔════════════════════════════════════════════════════════════╗
    ║                                                            ║
    ║          ★  T  E  X  T     A  N  A  L  Y  Z  E  R  ★      ║
    ║                                                            ║
    ╚════════════════════════════════════════════════════════════╝";


        /* ───── 2. Theme & colour helpers ──────────────────────────── */
        private static readonly ConsoleColor Accent = ConsoleColor.Cyan;

        private static void WriteAccent(string text)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = Accent;
            Console.Write(text);
            Console.ForegroundColor = old;
        }

        private static void WriteOk(string text)  =>
            WriteWith(text, ConsoleColor.Green);
        private static void WriteErr(string text) =>
            WriteWith(text, ConsoleColor.Red);

        private static void WriteWith(string text, ConsoleColor fg)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = fg;
            Console.Write(text);
            Console.ForegroundColor = old;
        }

        /* ───── 3. Data field ───────────────────────────────────────── */
        private BinarySearchTree<WordInfo>? _bst;

        /* ───── 4. Public entry point ──────────────────────────────── */
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
                    case "1": _bst!.PreOrder().ToList().ForEach(Console.WriteLine); break;
                    case "2": WriteAccent($"Unique words: {_bst!.Count}\n"); break;
                    case "3": ListAlphabetical(); break;
                    case "4": ShowLongest(); break;
                    case "5": ShowMostFrequent(); break;
                    case "6": ShowLineNumbers(); break;
                    case "7": ShowWordFrequency(); break;
                    case "8": LoadFile(); break;
                    case "9": ExportCsv(); break;
                    default : WriteErr("✖  Please enter a choice from 0-9.\n"); break;
                }
            }
        }

        /* ───── 5. UI helpers ───────────────────────────────────────── */
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
  9  EXPORT all stats to CSV                     
  0  Exit program                                
────────────────────────────────────────────────────────────");
            Console.Write("Your choice: ");
        }

        /* ───── 6. File loading with progress & timing ─────────────── */
        private void LoadFile()
        {
            while (true)
            {
                Console.Write("\nEnter path to .txt file (Enter for default 'mobydick.txt'): ");
                string path = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(path)) path = "mobydick.txt";

                try
                {
                    var sw = Stopwatch.StartNew();
                    _bst = FileParser.ParseWithProgress(path);
                    sw.Stop();
                    WriteOk($"\n✓  Loaded '{path}' ");
                    WriteAccent($"({_bst.Count} unique, {sw.ElapsedMilliseconds} ms)\n\n");
                    return;
                }
                catch (Exception ex)
                {
                    WriteErr($"✖  {ex.Message}\n");
                }
            }
        }

        /* ───── 7. Feature handlers ─────────────────────────────────── */

        private void ListAlphabetical()
        {
            Console.Write("List A→Z or Z→A? (A/Z): ");
            bool desc = (Console.ReadLine()?.Trim().ToLowerInvariant() ?? "a") == "z";
            var seq = desc ? _bst!.InOrder().Reverse() : _bst!.InOrder();

            foreach (var w in seq)
                Console.WriteLine(w);

        }

        private void ShowLongest() =>
            WriteAccent(_bst!.InOrder()
                             .OrderByDescending(w => w.Word.Length)
                             .First() + "\n");

        private void ShowMostFrequent() =>
            WriteAccent(_bst!.InOrder()
                             .OrderByDescending(w => w.Count)
                             .First() + "\n");

        private void ShowLineNumbers()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { WriteErr("No word entered.\n"); return; }

            string w = input.ToLowerInvariant();
            var probe = new WordInfo(w, 0);
            if (_bst!.TryGet(probe, out var info))
                WriteAccent($"{w} appears on line(s): {string.Join(", ", info.LineNumbers)}\n");
            else
                WriteErr("Word not found.\n");
        }

        private void ShowWordFrequency()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { WriteErr("No word entered.\n"); return; }

            string w = input.ToLowerInvariant();
            var probe = new WordInfo(w, 0);
            if (_bst!.TryGet(probe, out var info))
                WriteAccent($"{w} appears {info.Count} time(s).\n");
            else
                WriteErr("Word not found.\n");
        }

        private void ExportCsv()
        {
            const string FileName = "word_stats.csv";
            File.WriteAllLines(FileName,
                _bst!.InOrder().Select(w => $"{w.Word},{w.Count}"));
            WriteOk($"✓  CSV exported to {FileName}\n");
        }
    }
}
