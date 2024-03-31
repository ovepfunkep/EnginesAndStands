using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using EngineSimulationLibrary.Models.Engines;

namespace WPFEngineTests.ViewModels
{
    public class EngineViewModel : ObservableObject
    {
        public string Name { get; set; }
        public IEngine Engine { get; set; }

        public EngineViewModel(string name, IEngine engine)
        {
            Name = name;
            Engine = engine;
        }
    }
}
