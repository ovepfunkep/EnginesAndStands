using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using EngineSimulationLibrary.Models;

using Serilog;

namespace WPFEngineTests.Models
{
    public class ObservableTestStand(double ambientTemperature, ILogger logger) : ObservableObject
    {
        public string AmbientTemperature
        {
            get => model.AmbientTemperature.ToString();
            set
            {
                if (double.TryParse(value.ToString(), out double newValue))
                    model.AmbientTemperature = newValue;

                OnPropertyChanged(nameof(AmbientTemperature));
            }
        }

        public readonly TestStand model = new(ambientTemperature, logger);
    }
}
