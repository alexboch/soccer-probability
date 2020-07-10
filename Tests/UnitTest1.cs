using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoccerProbability;
using SoccerProbability.Computation;
using SoccerProbability.DataPreprocessing;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIsFullEventsGroup()
        {
            
            //Для i голов проверить, что суммарная вероятность равна 1
            for (int i = 1; i < 100; i++)
            {
                var interval = new GoalsInterval(1, i);
                var inputData = new InputData(Constants.MinutesPerMatch, new GoalType[] { }, interval, Data.MeanHostGoals,
                    Data.MeanGuestGoals);
                var res = ProbModel.ComputeProbs(inputData);
                var sum = res.HostsWonProb + res.GuestsWonProb + res.DrawProb + res.NotFinishedProb;
                Assert.AreEqual(sum, 1, 0.001);
            }
        }

    }
}
