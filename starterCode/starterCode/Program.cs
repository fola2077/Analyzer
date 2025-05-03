// Program.cs
using System;

namespace starterCode
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new TextAnalyzer().Run();
        }
    }
}
