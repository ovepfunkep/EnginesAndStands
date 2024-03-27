using ConsoleEngineTests;
using EngineSimulationLibrary.Models;

#region Input data
double torqueInertia = 10;
List<double> torqueDependencyPart = [20, 75, 100, 105, 75, 0];
List<double> crankshaftSpeedDependencyPart = [0, 75, 150, 200, 250, 300];
ushort overheatTemperature = 110;
double heatingSpeedToTorqueRatio = 0.01;
double heatingSpeedToCrankshafRotationSpeedRatio = 0.0001;
double coolingToTemperatureRatio = 0.1;
short ambientTemperature = Utils.ReadShort("Ambient temperature");
#endregion

TestStand testStand = new(torqueInertia,
                                  torqueDependencyPart,
                                  crankshaftSpeedDependencyPart,
                                  overheatTemperature,
                                  heatingSpeedToTorqueRatio,
                                  heatingSpeedToCrankshafRotationSpeedRatio,
                                  coolingToTemperatureRatio,
                                  ambientTemperature);