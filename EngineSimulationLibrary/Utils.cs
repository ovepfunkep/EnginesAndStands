using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineSimulationLibrary
{
    public static class Utils
    {
        public static double GetYByLinearDependency(double currentX,
                                                           double prevPointX,
                                                           double prevPointY,
                                                           double nextPointX,
                                                           double nextPointY)
        {
            var totalXBetweenPoints = nextPointX - prevPointX;
            var totalYBetweenPoints = nextPointY - prevPointY;
            var ratio = (currentX - prevPointX) / totalXBetweenPoints;
            var currentY = prevPointY + ratio * totalYBetweenPoints;
            return currentY;
        }
    }
}
