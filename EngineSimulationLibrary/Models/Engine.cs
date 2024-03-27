using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineSimulationLibrary.Models
{
    public class Engine(double torqueInertia,
                        List<(double Torque, double CrankshaftSpeed)> fragmentLinearDependency,
                        ushort overheatTemperature,
                        double heatingSpeedToTorqueRatio,
                        double heatingSpeedToCrankshafRotationSpeedRatio,
                        double coolingToTemperatureRatio,
                        short ambientTemperature)
    {
        #region Properties
        // I (Момент инерции двигателя)
        double TorqueInertia { get; set; } = torqueInertia;

        // M, V (Крутящий элемент, скорость вращения коленвала). HashSet because values can't repeat
        List<(double Torque, double CrankshaftSpeed)> FragmentLinearDependency { get; set; } = fragmentLinearDependency;

        // Hm (Коэффициент зависимости скорости нагрева от крутящего момента)
        double heatingSpeedToTorqueRatio { get; set; } = heatingSpeedToTorqueRatio;

        // Hv (Коэффициент зависимости скорости нагрева от скорости вращения коленвала)
        double heatingSpeedToCrankshafRotationSpeedRatio { get; set; } = heatingSpeedToCrankshafRotationSpeedRatio;

        // C (Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды)
        double coolingToTemperatureRatio { get; set; } = coolingToTemperatureRatio;

        // Температура перегрева. ushort because values can't be < 0 ("Overheat") and greater than 65535
        ushort OverheatTemperature { get; set; } = overheatTemperature;

        // Текущая температура
        short CurrentTemperature { get; set; } = ambientTemperature;
        #endregion

        #region Methods
        // Скорость нагрева двигателя в определенное время: Vh = M × Hh + V^2 × Hv 
        public double GetHeatSpeed(int secondsOfRunning)
        {
            (double currentTorque, double currentCrankshaftSpeed) = FragmentLinearDependency[secondsOfRunning];

            double heatingSpeed = currentTorque * heatingSpeedToTorqueRatio + 
                                  Math.Pow(currentCrankshaftSpeed, 2) * heatingSpeedToCrankshafRotationSpeedRatio;

            return heatingSpeed;
        }

        // Скорость охлаждения двигателя: Vc = C × (Tсреды - Тдвигателя)
        public double GetCoolingSpeed(double ambientTemperature) => coolingToTemperatureRatio * (ambientTemperature - CurrentTemperature);
        #endregion
    }
}
