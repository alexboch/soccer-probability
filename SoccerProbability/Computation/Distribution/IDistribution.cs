using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation.Distribution
{
    interface IDistribution
    {
        /// <summary>
        /// Вероятность наступления k событий
        /// </summary>
        /// <param name="k">Число событий</param>
        /// <returns></returns>
        double ComputeProb(int k);
    }
}
