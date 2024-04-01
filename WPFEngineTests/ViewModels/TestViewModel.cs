using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using EngineSimulationLibrary.Models.Engines;

namespace WPFEngineTests.ViewModels
{
    public class TestViewModel : ObservableObject
    {
        public string Name { get; set; }
        public Func<Task<string>> Execute { get; set; }

        public TestViewModel(string name, Func<Task<string>> execute)
        {
            Name = name;
            Execute = execute;
        }
    }
}
