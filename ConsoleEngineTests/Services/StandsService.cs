using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineSimulationLibrary.Models;

using Serilog;

using static ConsoleEngineTests.ConsoleUtils;

namespace ConsoleEngineTests.Services
{
    public static class StandsService
    {
        public static TestStand GetNewTestStand(ILogger? logger = null)
        {
            Console.WriteLine("To create new test stand please enter its ambient temperature:");
            Console.WriteLine();

            short ambientTemperature = ReadShort();
            Console.WriteLine();

            TestStand testStand = new(ambientTemperature, logger);

            Console.WriteLine($"Test stand successfully created!");
            Console.WriteLine();

            return testStand;
        }

        public static void ChangeAmbientTemperature(TestStand testStand)
        {
            Console.Clear();
            Console.WriteLine($"Current temperature: {testStand.AmbientTemperature:0.00}");
            Console.WriteLine();
            Console.WriteLine("Please enter new ambient temperature:");
            testStand.AmbientTemperature = ReadShort();
        }
    }
}
