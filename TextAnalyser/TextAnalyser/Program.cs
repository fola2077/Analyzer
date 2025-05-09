// Program.cs
using System;

namespace TextAnalyser
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
