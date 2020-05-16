using System;

namespace DecisionTree.Helper
{
    /// <summary>
    /// Represents the ConsoleHelper class.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Writes the given input with the given color and appends a new line.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="fontColor">The color.</param>
        public static void WriteLine(string input, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine(input);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes the given input with the given color.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="fontColor">The color.</param>
        public static void Write(string input, ConsoleColor fontColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = fontColor;
            Console.Write(input);
            Console.ResetColor();
        }
    }
}
