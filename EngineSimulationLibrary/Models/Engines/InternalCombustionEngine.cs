using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineSimulationLibrary.Utils;

namespace EngineSimulationLibrary.Models.Engines
{
    public class InternalCombustionEngine : IEngine
    {
        #region public properties
        // I (Момент инерции двигателя)
        public double TorqueInertia { get; set; }

        // C (Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды)
        public double CoolingToTemperatureRatio { get; set; }

        // M, V (Крутящий элемент, скорость вращения коленвала)
        public List<(double Torque, double CrankshaftSpeed)> FragmentLinearDependency { get; set; }

        // Температура перегрева
        public double OverheatTemperature { get; set; }

        // Hm (Коэффициент зависимости скорости нагрева от крутящего момента)
        public double HeatingSpeedToTorqueRatio { get; set; }

        // Hv (Коэффициент зависимости скорости нагрева от скорости вращения коленвала)
        public double HeatingSpeedToCrankshafRotationSpeedRatio { get; set; }

        // Состояние активности
        private bool _enabled = false;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (value == false)
                {
                    SecondsRunning = 0;
                    CurrentCrankshaftSpeed = 0;
                }
            }
        }

        // Количество секунд в включенном состоянии
        public int SecondsRunning { get; private set; }

        // Текущая температура
        public double CurrentTemperature { get; private set; }

        // V (Текущая скорость коленвала)
        private double _currentCrankshaftSpeed;
        public double CurrentCrankshaftSpeed
        {
            get => _currentCrankshaftSpeed;
            private set
            {
                if (_currentCrankshaftSpeed != value)
                {
                    _currentCrankshaftSpeed = value;

                    (double nearestTorqueBelow,
                     double nearestCrankshaftSpeedBelow,
                     double nearestTorqueAbove,
                     double nearestCrankshaftSpeedAbove) = FragmentLinearDependency.FindNearestPoints(CurrentCrankshaftSpeed);

                    CurrentTorque = GetYByLinearDependency(CurrentCrankshaftSpeed,
                                                                  nearestTorqueBelow,
                                                                  nearestCrankshaftSpeedBelow,
                                                                  nearestTorqueAbove,
                                                                  nearestCrankshaftSpeedAbove);

                    CurrentPower = GetEnginePower();
                }
            }
        }

        // M (Текущий крутящий момент)
        public double CurrentTorque { get; private set; }

        // N (Мощность двигателя внутреннего сгорания)
        public double CurrentPower { get; private set; } = 0;
        #endregion

        #region public methods
        /// <summary>
        /// Evaluates one second of engine work (or staying still)
        /// </summary>
        /// <param name="ambientTemperature"></param>
        public void EvaluateOneSecondOfWorking(double ambientTemperature)
        {
            CurrentTemperature += GetHeatSpeed() - GetCoolingSpeed(ambientTemperature);
            CurrentCrankshaftSpeed += GetCrankshaftAccelerationSpeed();
            SecondsRunning++;
        }

        /// <summary>
        /// Resets engine properties
        /// </summary>
        /// <param name="ambientTemperature"></param>
        public void Reset(double ambientTemperature)
        {
            CurrentTemperature = ambientTemperature;
            Enabled = false;
        }

        public object Clone() => new InternalCombustionEngine(TorqueInertia,
                                                              FragmentLinearDependency,
                                                              OverheatTemperature,
                                                              HeatingSpeedToTorqueRatio,
                                                              HeatingSpeedToCrankshafRotationSpeedRatio,
                                                              CoolingToTemperatureRatio,
                                                              CurrentTemperature);
        #endregion

        public InternalCombustionEngine(double torqueInertia,
                      List<(double Torque, double CrankshaftSpeed)> fragmentLinearDependency,
                      double overheatTemperature,
                      double heatingSpeedToTorqueRatio,
                      double heatingSpeedToCrankshafRotationSpeedRatio,
                      double coolingToTemperatureRatio,
                      double currentTemperature)
        {
            CoolingToTemperatureRatio = coolingToTemperatureRatio;
            OverheatTemperature = overheatTemperature;
            CurrentTemperature = currentTemperature;
            TorqueInertia = torqueInertia;
            FragmentLinearDependency = fragmentLinearDependency;
            HeatingSpeedToTorqueRatio = heatingSpeedToTorqueRatio;
            HeatingSpeedToCrankshafRotationSpeedRatio = heatingSpeedToCrankshafRotationSpeedRatio;
            CurrentTorque = fragmentLinearDependency.First(d => d.CrankshaftSpeed == 0).Torque;
        }

        #region private methods
        // Скорость нагрева двигателя в определенное время: Vh = M × Hh + V^2 × Hv 
        private double GetHeatSpeed()
        {
            if (_enabled)
            {
                double heatingSpeed = CurrentTorque * HeatingSpeedToTorqueRatio +
                                      Math.Pow(CurrentCrankshaftSpeed, 2) * HeatingSpeedToCrankshafRotationSpeedRatio;

                return heatingSpeed;
            }
            else return 0;
        }

        // Скорость охлаждения двигателя: Vc = C × (Tсреды - Тдвигателя)
        private double GetCoolingSpeed(double ambientTemperature) => CoolingToTemperatureRatio * (ambientTemperature - CurrentTemperature);

        // Найти ускорение коленвала
        private double GetCrankshaftAccelerationSpeed() => CurrentTorque / TorqueInertia;

        private double GetEnginePower() => CurrentTorque * CurrentCrankshaftSpeed / 1000;
        #endregion
    }
}
