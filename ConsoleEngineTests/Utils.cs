using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngineTests
{
    public static class Utils
    {
        /// <summary>
        /// Function reads value from console, parses it to double and returns.
        /// </summary>
        /// <param name="variableName">Variable name that will be asked</param>
        /// <returns>Desirable double value</returns>
        public static short ReadShort(string variableName)
        {
            short result;
            Console.Write($"{variableName} = ");
            while (!short.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("\nInput value must be a fractional number!\nPlease try again..\n");
                Console.Write("Enter ambient tempreture: ");
            }
            return result;
        }
    }
}
