using System;
using System.Text;

namespace HospitalManagementSystem.Utilities
{
    /// <summary>
    /// Helper class for console operations
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Read password from console with masking
        /// </summary>
        public static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Ignore any key that isn't a character or Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }

        /// <summary>
        /// Display a header with formatting
        /// </summary>
        public static void DisplayHeader(string title)
        {
            Console.Clear();
            Console.WriteLine("\n" + new string('=', title.Length + 10));
            Console.WriteLine($"     {title}     ");
            Console.WriteLine(new string('=', title.Length + 10) + "\n");
        }

        /// <summary>
        /// Display an error message in red
        /// </summary>
        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Display a success message in green
        /// </summary>
        public static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Display a warning message in yellow
        /// </summary>
        public static void DisplayWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Pause and wait for user input
        /// </summary>
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}