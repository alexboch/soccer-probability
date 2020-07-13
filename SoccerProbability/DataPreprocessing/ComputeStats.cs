using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.DataPreprocessing
{
    public static class ComputeStats
    {


        private static Dictionary<int, double> MeanByValues(Dictionary<int, double> sumGoalsResult)
        {
            var means = new Dictionary<int, double>();
            foreach(var kvp in sumGoalsResult)
            {
                means[kvp.Key] = sumGoalsResult[kvp.Key] * kvp.Key;
            }

            return means;
        }

        public static (double, double) ComputeMeans(IEnumerable<(int, int, double)> goalStats)
        {
            var goalsList = goalStats.ToList();
            var minGoals = goalsList.Min(s => s.Item1);
            var maxGoals = goalsList.Max(s => s.Item1);
            var sumMeanHost = new Dictionary<int, double>();
            var sumMeanGuest = new Dictionary<int, double>();
            for (int i = minGoals; i <= maxGoals; i++)
            {
                var i1 = i;
                var sumByHostGoals = goalsList.Where(g => g.Item1 == i1).Sum(g => g.Item3);
                sumMeanHost[i1] = sumByHostGoals / 100.0;
            }

            for (int i = minGoals; i <= maxGoals; i++)
            {
                var i1 = i;
                var sumByGuestGoals = goalsList.Where(g => g.Item2 == i1).Sum(g => g.Item3);
                sumMeanGuest[i1] = sumByGuestGoals / 100.0;
            }

            var hostSums = MeanByValues(sumMeanHost);
            var guestSums = MeanByValues(sumMeanGuest);
            var meanGuest = guestSums.Values.Sum() / Constants.MinutesPerMatch;
            var meanHost = hostSums.Values.Sum() / Constants.MinutesPerMatch;
            return (meanHost, meanGuest);
        }
    }
}
