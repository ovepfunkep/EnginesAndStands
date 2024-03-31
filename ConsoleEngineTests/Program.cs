using EngineSimulationLibrary.Models;
using EngineSimulationLibrary.Models.Engines;
using static ConsoleEngineTests.ConsoleUtils;
using static ConsoleEngineTests.Services.TestsService;
using static ConsoleEngineTests.Services.StandsService;
using Serilog;

internal class Program
{
    public static readonly string LogsLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EnginesAndStands\\logs.txt";

    private static void Main()
    {
        Console.WriteLine("Welcome to the Engine Simulations!");
        Console.WriteLine();

        #region Input data
        double torqueInertia = 10;
        List<double> crankshaftSpeedDependencyPart = [20, 75, 100, 105, 75, 0];
        List<double> torqueDependencyPart = [0, 75, 150, 200, 250, 300];
        ushort overheatTemperature = 110;
        double heatingSpeedToTorqueRatio = 0.01;
        double heatingSpeedToCrankshafRotationSpeedRatio = 0.0001;
        double coolingToTemperatureRatio = 0.1;
        #endregion

        if (File.Exists(LogsLocation)) 
            File.Delete(LogsLocation);

        using var logger = new LoggerConfiguration()
            .WriteTo
            .File(LogsLocation)
            .CreateLogger();

        TestStand testStand = GetNewTestStand();
        Console.WriteLine("Press any button to continue...");
        Console.ReadKey();
        Console.Clear();

        IEngine testEngine = GetNewInternalCombustionEngine();

        List<(string Name, Action Execute)> enginesOptions = [
            ("Internal combustion engine", () => testEngine = GetNewInternalCombustionEngine())
            ];

        List<(string Name, Action Execute)> mainMenuOptions = [
            ($"Switch engine", () => ShowMenu("Engine selection", enginesOptions)),
            ($"Switch ambient temperature", () => ChangeAmbientTemperature(testStand)),
            ("Show tests", () => ChooseTestToRun(testStand, testEngine)),
            ("Exit", () => Environment.Exit(0))
            ];

        mainMenuOptions.Insert(mainMenuOptions.Count - 1,
                               testStand.Logger == null ? ("Add logger", AddLoggerMenuItemChosen) :
                                                          ("Remove logger", RemoveLoggerMenuItemChosen));

        while (true) ShowMenu("Main menu", mainMenuOptions);

        InternalCombustionEngine GetNewInternalCombustionEngine() => new(torqueInertia,
torqueDependencyPart.Zip(crankshaftSpeedDependencyPart).ToList(),
overheatTemperature,
heatingSpeedToTorqueRatio,
heatingSpeedToCrankshafRotationSpeedRatio,
coolingToTemperatureRatio,
testStand.AmbientTemperature);

        void AddLoggerMenuItemChosen()
        {
            testStand.Logger = logger;
            mainMenuOptions.Remove(mainMenuOptions.Find(o => o.Name == "Add logger"));
            mainMenuOptions.Insert(mainMenuOptions.Count - 1, ("Remove logger", RemoveLoggerMenuItemChosen));
        }

        void RemoveLoggerMenuItemChosen()
        {
            testStand.Logger = null;
            mainMenuOptions.Remove(mainMenuOptions.Find(o => o.Name == "Remove logger"));
            mainMenuOptions.Insert(mainMenuOptions.Count - 1, ("Add logger", AddLoggerMenuItemChosen));
        }
    }
}
