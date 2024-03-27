using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineSimulationLibrary.Models
{
    public class TestStand
    {
        private Engine TestingEngine { get; set; }
        // Температура среды
        public double AmbientTemperature { get; set; }

        public TestStand(double torqueInertia,
                         List<double> torqueDependencyPart,
                         List<double> crankshaftSpeedDependencyPart,
                         ushort overheatTemperature,
                         double heatingSpeedToTorqueRatio,
                         double heatingSpeedToCrankshafRotationSpeedRatio,
                         double coolingToTemperatureRatio,
                         short ambientTemperature)
        {
            var fragmentLinearDependency = torqueDependencyPart.Zip(crankshaftSpeedDependencyPart).ToList();
            TestingEngine = new Engine(torqueInertia, 
                                       fragmentLinearDependency, 
                                       overheatTemperature, 
                                       heatingSpeedToTorqueRatio,
                                       heatingSpeedToCrankshafRotationSpeedRatio,
                                       coolingToTemperatureRatio,
                                       ambientTemperature);
            AmbientTemperature = ambientTemperature;
        }
    }
}
