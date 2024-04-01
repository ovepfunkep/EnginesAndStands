using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineSimulationLibrary.Models.Engines;

using Serilog;
namespace EngineSimulationLibrary.Models
{
    public class TestStand
    {
        // Температура среды
        public double AmbientTemperature { get; set; }

        public ILogger? Logger { get; set; }

        /// <summary>
        /// Create TestStand instance
        /// </summary>
        /// <param name="ambientTemperature">Ambient test stand temperature</param>
        /// <param name="logger">Logger or null</param>
        public TestStand(double ambientTemperature, ILogger? logger = null)
        {
            AmbientTemperature = ambientTemperature;
            Logger = logger;
        }

        /// <summary>
        /// Test calculates and displays the time in seconds that will pass from 
        /// internal combustion engine start until it overheats.
        /// </summary>
        /// <param name="testEngine">Testing engine</param>
        /// <returns>Task with int</returns>
        public async Task<int> GetSecondsAmountNeededForOverheatAsync(IEngine testEngine)
        {
            // Thread safety
            var clonedEngine = (IEngine)testEngine.Clone();

            clonedEngine.Reset(AmbientTemperature);
            clonedEngine.Enabled = true;

            await Task.Run(() =>
            {
                while (clonedEngine.CurrentTemperature < clonedEngine.OverheatTemperature)
                {
                    clonedEngine.EvaluateOneSecondOfWorking(AmbientTemperature);
                    Logger?.Information(@"Current time step: {secondsRunning}
Engine's temperature = {currentTemperature}",
                                          clonedEngine.SecondsRunning,
                                          clonedEngine.CurrentTemperature);
                }
            });

            return clonedEngine.SecondsRunning;
        }

        /// <summary>
        /// The test calculates and outputs the maximum engine power per kW 
        /// as well as the Crankshaft rotation speed to radian/sec 
        /// at which this maximum power is achieved.
        /// </summary>
        /// <param name="testEngine">Testing engine</param>
        /// <returns>Task with tuple with output data</returns>
        public async Task<(double maxPower, double maxTorque, double maxSpeed)>
            GetMetricsWhenMaxPowerWhileAcceleratingAsync(InternalCombustionEngine testEngine)
        {
            // Thread safety
            var clonedEngine = (InternalCombustionEngine)testEngine.Clone();

            double maxEnginePower = 0;
            double torqueOnMaxPower = 0;
            double crankshaftSpeedOnMaxPower = 0;
            double previousCrankshaftSpeed;

            testEngine.Reset(AmbientTemperature);

            await Task.Run(() =>
            {
                do
                {
                    previousCrankshaftSpeed = testEngine.CurrentCrankshaftSpeed;
                    testEngine.EvaluateOneSecondOfWorking(AmbientTemperature);
                    if (testEngine.CurrentPower > maxEnginePower)
                    {
                        maxEnginePower = testEngine.CurrentPower;
                        torqueOnMaxPower = testEngine.CurrentTorque;
                        crankshaftSpeedOnMaxPower = testEngine.CurrentCrankshaftSpeed;
                    }
                    Logger?.Information(@"Current time step: {secondsRunning}
Engine's current power = {currentPower}",
                                          testEngine.SecondsRunning,
                                          testEngine.CurrentPower);
                }
                while (testEngine.CurrentCrankshaftSpeed > previousCrankshaftSpeed);
            });

            return (maxEnginePower, torqueOnMaxPower, crankshaftSpeedOnMaxPower);
        }
    }
}
