using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using EngineSimulationLibrary.Models.Engines;

using WPFEngineTests.ViewModels;

using static WPFEngineTests.TestsHelper;

namespace WPFEngineTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void cbEngineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.AvailableTests.Clear();

            var selectedEngine = ((EngineViewModel)((ComboBox)sender).SelectedItem).Engine;

            List<TestViewModel> availableTests = [
                new("Heat test", () => GetOverheatTestResultsAsync(_viewModel.TestStand, selectedEngine))
                ];

            if (selectedEngine is InternalCombustionEngine ICE)
                availableTests.Add(new("Power test", () => GetPowerTestResultsAsync(_viewModel.TestStand, ICE)));

            availableTests.ForEach(_viewModel.AvailableTests.Add);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.TestStand.model.Logger = _viewModel.Logger;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.TestStand.model.Logger = null;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbAvailableTests.SelectedItem != null)
                tBlockResults.Text = await ((TestViewModel)cbAvailableTests.SelectedItem).Execute.Invoke();
        }
    }
}