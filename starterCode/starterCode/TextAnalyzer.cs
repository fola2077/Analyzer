using System;
using System.Linq;

namespace starterCode
{
    public class TextAnalyzer
    {
        /* ───── 1. ASCII banners ─────────────────────────────────────── */
        private const string BannerA = @"
╔═══════════════════════════════════════╗
║         WELCOME TO TEXT ANALYZER      ║
╚═══════════════════════════════════════╝";

        private const string BannerB = @"
 _____         _        _                _                 
|_   _|__  ___| |_ __ _| | ___   __ _   / \   __ _  ___ ___
  | |/ _ \/ __| __/ _` | |/ _ \ / _` | / _ \ / _` |/ __/ _ \
  | |  __/\__ \ || (_| | | (_) | (_| |/ ___ \ (_| | (_|  __/
  |_|\___||___/\__\__,_|_|\___/ \__, /_/   \_\__,_|\___\___|
                               |___/                        ";

        private const string BannerC = @"
════════════════════════════════════════════════════════════
                ★  TEXT  ANALYZER  ★
════════════════════════════════════════════════════════════";

        private static readonly string[] Banners = { BannerA, BannerB, BannerC };

        /* ───── 2. Data field ─────────────────────────────────────────── */
        private BinarySearchTree<WordInfo>? _bst;

        /* ───── 3. Public entry point ─────────────────────────────────── */
        public void Run()
        {
            ShowBanner();
            LoadFile();

            while (true)
            {
                ShowMenu();
                switch (Console.ReadLine()?.Trim())
                {
                    case "0": return;
                    case "1": _bst!.PreOrder().ToList().ForEach(Console.WriteLine); break;
                    case "2": Console.WriteLine($"Number of unique words: {_bst!.Count}"); break;
                    case "3": ListAlphabetical(); break;
                    case "4": ShowLongest(); break;
                    case "5": ShowMostFrequent(); break;
                    case "6": ShowLineNumbers(); break;
                    case "7": ShowWordFrequency(); break;
                    case "8": LoadFile(); break;
                    default:  Console.WriteLine("✖  Please enter a choice from 0‑8."); break;
                }
            }
        }

        /* ───── 4. UI helpers ─────────────────────────────────────────── */

        private static void ShowBanner()
        {
            Console.Clear();
            Console.WriteLine("Choose a banner style: 1‑Simple  2‑Figlet  3‑Double‑Line (Enter for 1)");
            string pick = Console.ReadLine()?.Trim() ?? "";
            int i = pick switch { "2" => 1, "3" => 2, _ => 0 };   // safe index
            Console.Clear();
            Console.WriteLine(Banners[i]);
        }

        private static void ShowMenu()
        {
            Console.WriteLine(@"
────────────────────────────────────────────────────────────
  1  Display every word (pre‑order, unsorted)     
  2  Show HOW MANY unique words are stored       
  3  List words ALPHABETICALLY (choose A→Z / Z→A)
  4  Show the LONGEST word                       
  5  Show the MOST‑FREQUENT word                 
  6  Show LINE NUMBERS for a given word          
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
                string path = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(path)) path = "mobydick.txt";

                try
                {
                    _bst = FileParser.Parse(path);
                    Console.WriteLine($"✓  Loaded '{path}' : {_bst.Count} unique words.\n");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✖  {ex.Message}");
                }
            }
        }

        /* ───── 5. Feature handlers ──────────────────────────────────── */

        private void ListAlphabetical()
        {
            Console.Write("List A→Z or Z→A? (A/Z): ");
            string choice = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "a";
            bool desc = choice == "z";
            var seq = desc ? _bst!.InOrder().Reverse() : _bst!.InOrder();
            foreach (var w in seq) Console.WriteLine(w);
        }

        private void ShowLongest() =>
            Console.WriteLine(_bst!.InOrder()
                                   .OrderByDescending(w => w.Word.Length)
                                   .First());

        private void ShowMostFrequent() =>
            Console.WriteLine(_bst!.InOrder()
                                   .OrderByDescending(w => w.Count)
                                   .First());

        private void ShowLineNumbers()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("No word entered."); return; }

            string w = input.ToLowerInvariant();
            var probe = new WordInfo(w, 0);
            if (_bst!.TryGet(probe, out var info))
                Console.WriteLine($"{w} appears on line(s): {string.Join(", ", info.LineNumbers)}");
            else
                Console.WriteLine("Word not found.");
        }

        private void ShowWordFrequency()
        {
            Console.Write("Enter word: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("No word entered."); return; }

            string w = input.ToLowerInvariant();
            var probe = new WordInfo(w, 0);
            if (_bst!.TryGet(probe, out var info))
                Console.WriteLine($"{w} appears {info.Count} time(s).");
            else
                Console.WriteLine("Word not found.");
        }
    }
}
