// Program.cs
using System;
using System.Text;
using starterCode;            

namespace starterCode
{
    internal static class Program
    {
        // Entry‑point method – must be inside a class/struct and inside a namespace block.
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            new TextAnalyzer().Run();
        }
    }
}

