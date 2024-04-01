using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngineTests
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// Function reads value from console, parses it to double and returns.
        /// </summary>
        /// <param name="variableName">Variable name that will be asked</param>
        /// <returns>Desirable double value</returns>
        public static short ReadShort()
        {
            short result;
            while (!short.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Input value must be a fractional number!");
                Console.WriteLine("Please try again:");
            }
            return result;
        }

        /// <summary>
        /// Function helps to get exact value of possible values
        /// from console input
        /// </summary>
        /// <param name="possibleValues">Possible input values</param>
        /// <returns></returns>
        public static string? ReadExact(params string[] possibleValues)
        {
            string? input = Console.ReadLine();

            while (!possibleValues.Any(s => s == input?.ToLower()))
                input = Console.ReadLine();

            return input;
        }

        /// <summary>
        /// Function reads user mind and helps to understand whether
        /// user wants something or not.
        /// </summary>
        /// <returns>true if wants, otherwise false</returns>
        public static bool DoesUserWantsSomething()
        {
            var userInput = Console.ReadLine();
            return new List<string>() { "yes", "y", "+", "yep", "ye", "sure", "да", "1" }.Any(s => s == userInput?.ToLower());
        }

        /// <summary>
        /// Function helps manage many menus such as main, engine selection etc.
        /// </summary>
        /// <param name="menuName">Menu name to display at the top of the menu</param>
        /// <param name="options">Menu options in format Name - Action</param>
        public static void ShowMenu(string menuName, List<(string Name, Action Execute)> options)
        {
            Console.Clear();
            Console.WriteLine(menuName);
            Console.WriteLine();
            Console.WriteLine(string.Join('\n', options.Select((s, i) => $"{i}. {s.Name}")));

            var optionsIndexes = options.Select((s, i) => i.ToString()).ToArray();

            var selectedItem = ReadExact(optionsIndexes);

            options[Convert.ToInt32(selectedItem)].Execute();

            Console.Clear();
        }
    }
}
