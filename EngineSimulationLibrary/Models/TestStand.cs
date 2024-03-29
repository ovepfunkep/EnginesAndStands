using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineSimulationLibrary.Models.Engines;
using Microsoft.Extensions.Logging;

namespace EngineSimulationLibrary.Models
{
    public class TestStand(short ambientTemperature, ILogger? logger = null)
    {
        // Температура среды
        public short AmbientTemperature { get; set; } = ambientTemperature;

        public ILogger? Logger { get; set; } = logger;

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
                    Logger?.LogInformation(@"Current time step: {secondsRunning}
Engine's temperature = {currentTemperature}",
                                          clonedEngine.SecondsRunning,
                                          clonedEngine.CurrentTemperature);
                }
            });

            return clonedEngine.SecondsRunning;
        }

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
                    Logger?.LogInformation(@"Current time step: {secondsRunning}
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
