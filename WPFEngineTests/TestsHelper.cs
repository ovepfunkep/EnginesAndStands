using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFEngineTests.Models;
using WPFEngineTests.ViewModels;
using EngineSimulationLibrary.Models;
using EngineSimulationLibrary.Models.Engines;

namespace WPFEngineTests
{
    public static class TestsHelper
    {
        public async static Task<string> GetOverheatTestResultsAsync(ObservableTestStand testStand, IEngine engine)
        {
            var result = testStand.model.GetSecondsAmountNeededForOverheatAsync(engine);
            return $"It took {await result} second to overheat!";
        }

        public async static Task<string> GetPowerTestResultsAsync(ObservableTestStand testStand, InternalCombustionEngine engine)
        {
            var task = testStand.model.GetMetricsWhenMaxPowerWhileAcceleratingAsync(engine);
            var (maxPower, maxTorque, maxSpeed) = await task; 
            return $"Power maximum was {maxPower:0.00} while\nTorque was {maxTorque:0.00} and\nSpeed was {maxSpeed:0.00}";
        }
    }
}
