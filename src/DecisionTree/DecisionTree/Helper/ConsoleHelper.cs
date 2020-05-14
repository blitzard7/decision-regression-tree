using System;

namespace DecisionTree.Helper
{
    public static class ConsoleHelper
    {
        public static void WriteLine(string input, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine(input);
            Console.ResetColor();
        }

        public static void Write(string input, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.Write(input);
            Console.ResetColor();
        }

        public static void WriteTreeEntry(string feature, string featureValue)
        {
            Write($"{featureValue} -> ");
            Write(feature, ConsoleColor.Cyan);
        }
    }
}
