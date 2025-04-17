// Program.cs
using System;                 // optional, but conventional
using starterCode;            // gives easy access to TextAnalyzer

namespace starterCode
{
    internal static class Program
    {
        // Entry‑point method – must be inside a class/struct and inside a namespace block.
        private static void Main(string[] args)
        {
            new TextAnalyzer().Run();
        }
    }
}

