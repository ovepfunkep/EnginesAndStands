using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EngineSimulationLibrary.Models.Engines;

using Serilog;

using WPFEngineTests.Models;

namespace WPFEngineTests.ViewModels
{
    public class MainWindowViewModel
    {
        public static string LogsLocation { get; set; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EnginesAndStands\\logs.txt";

        public ILogger Logger { get; set; }

        public ObservableTestStand TestStand { get; set; }

        public ObservableCollection<EngineViewModel> Engines { get; set; } = [];

        public ObservableCollection<TestViewModel> AvailableTests { get; set; } = [];

        public MainWindowViewModel()
        {
            if (File.Exists(LogsLocation)) File.Delete(LogsLocation);

            Logger = new LoggerConfiguration()
                .WriteTo
                .File(LogsLocation)
                .CreateLogger();

            TestStand = new(0, Logger);

            #region Input data
            double torqueInertia = 10;
            List<double> crankshaftSpeedDependencyPart = [20, 75, 100, 105, 75, 0];
            List<double> torqueDependencyPart = [0, 75, 150, 200, 250, 300];
            ushort overheatTemperature = 110;
            double heatingSpeedToTorqueRatio = 0.01;
            double heatingSpeedToCrankshafRotationSpeedRatio = 0.0001;
            double coolingToTemperatureRatio = 0.1;
            #endregion

            Engines.Add(new EngineViewModel("Internal combustion engine", new InternalCombustionEngine(torqueInertia,
torqueDependencyPart.Zip(crankshaftSpeedDependencyPart).ToList(),
overheatTemperature,
heatingSpeedToTorqueRatio,
heatingSpeedToCrankshafRotationSpeedRatio,
coolingToTemperatureRatio,
Convert.ToDouble(TestStand.AmbientTemperature))));
        }
    }
}
