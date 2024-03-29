namespace EngineSimulationLibrary.Models.Engines
{
    public interface IEngine : ICloneable
    {
        double CurrentPower { get; }
        double CurrentTemperature { get; }
        bool Enabled { get; set; }
        double OverheatTemperature { get; set; }
        int SecondsRunning { get; }

        void EvaluateOneSecondOfWorking(double ambientTemperature);
        void Reset(double ambientTemperature);
    }
}