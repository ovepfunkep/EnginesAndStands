using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineSimulationLibrary
{
    public static class Extensions
    {
        public static (double V1, double M1, double V2, double M2) FindNearestPoints(this List<(double V, double M)> data, double targetV)
        {
            var sortedData = data.OrderBy(pair => pair.V).ToList();

            int index = 0;
            double count = sortedData.Count;

            for (int i = 0; i < count; i++)
            {
                if (sortedData[i].V == targetV)
                {
                    index = i;
                    break;
                }
                else if (sortedData[i].V < targetV)
                {
                    index = i;
                }
                else
                {
                    break;
                }
            }

            double V1 = sortedData[index].V;
            double M1 = sortedData[index].M;
            double V2 = index + 1 < count ? sortedData[index + 1].V : V1;
            double M2 = index + 1 < count ? sortedData[index + 1].M : M1;

            return (V1, M1, V2, M2);
        }
    }
}
