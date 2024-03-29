using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineSimulationLibrary.Models;
using EngineSimulationLibrary.Models.Engines;

using static ConsoleEngineTests.ConsoleUtils;

namespace ConsoleEngineTests.Services
{
    public static class TestsService
    {
        public static void ChooseTestToRun(TestStand testStand, IEngine testEngine)
        {
            List<(string Name, Action Execute)> testOptions = [
                ("Overheat test", () => ShowOverheatTest(testStand, testEngine))
                ];

            if (testEngine is InternalCombustionEngine ICS)
                testOptions.Add(("Power limit test", () => ShowPowerLimitTest(testStand, ICS)));

            testOptions.Add(("Back", () => { }));

            if (testStand.Logger != null)
            {
                Console.WriteLine();
                Console.WriteLine();
            }
            ShowMenu($@"Tests available {(testStand.Logger == null ? "" : $"\n(Logs could after be found at this location: {Program.LogsLocation})")}", 
                     testOptions);
        }

        private static void ShowOverheatTest(TestStand testStand, IEngine testEngine)
        {
            Console.Clear();

            List<(string Name, Action Execute)> testOptions = [
                ("Run test", () => StartOverheatTest(testStand, testEngine)),
                ("Back", () => ChooseTestToRun(testStand, testEngine))
                ];

            ShowMenu(@"Test of overheat starts engine, monitors its temperature
until it is overheated and then returns seconds amount it took to overheat", testOptions);
        }

        private static void ShowPowerLimitTest(TestStand testStand, InternalCombustionEngine testEngine)
        {
            Console.Clear();

            List<(string Name, Action Execute)> testOptions = [
                ("Run test", () => StartPowerLimitTest(testStand, testEngine)),
                ("Back", () => ChooseTestToRun(testStand, testEngine))
                ];

            ShowMenu(@"Test of maximum power starts engine, monitors its metrics until it stops to accelerate and then
returns maximum power that was reached and torque and crankshaft speed at that moment", testOptions);
        }

        private static void StartOverheatTest(TestStand testStand, IEngine testEngine)
        {
            var task = testStand.GetSecondsAmountNeededForOverheatAsync(testEngine);
            task.Wait();
            var result = task.Result;
            Console.Clear();
            Console.WriteLine($"It took {result} seconds for engine to overheat");
            Console.WriteLine();
            Console.WriteLine("Press any button to continue..");
            Console.ReadKey();
        }

        private static void StartPowerLimitTest(TestStand testStand, InternalCombustionEngine testEngine)
        {
            Console.Clear();
            var task = testStand.GetMetricsWhenMaxPowerWhileAcceleratingAsync(testEngine);
            task.Wait(); 
            var (maxPower, maxTorque, maxSpeed) = task.Result; 
            Console.WriteLine($"Power maximum was {maxPower:0.00} while\nTorque was {maxTorque:0.00} and\nSpeed was {maxSpeed:0.00}");
            Console.WriteLine();
            Console.WriteLine("Press any button to continue..");
            Console.ReadKey();
        }

    }
}
