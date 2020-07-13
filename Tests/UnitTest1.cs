using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoccerProbability;
using SoccerProbability.Computation;
using SoccerProbability.DataPreprocessing;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {

        private const int MaxGoals = 100;
        private const int MinGoals = 1;
        private const double Delta = 0.001;

        private static readonly GoalType[] EmptyGoals = { };

        private void TestResultProbsSum(ResultProbs res)
        {
            var sum = res.HostsWonProb + res.GuestsWonProb + res.DrawProb + res.NotFinishedProb;
            Assert.AreEqual(sum, 1, 0.001);
        }

        [TestMethod]
        public void TestIsFullEventsGroup()
        {

            //Для i голов проверить, что суммарная вероятность равна 1
            for (int i = MinGoals; i <= MaxGoals; i++)
            {
                var interval = new GoalsInterval(1, i);
                var inputData = new InputData(Constants.MinutesPerMatch, EmptyGoals, interval, Data.MeanHostGoals,
                    Data.MeanGuestGoals);
                var res = ProbModel.ComputeProbs(inputData);
                TestResultProbsSum(res);
            }
        }

        /// <summary>
        /// Проверка вычисления вероятности, когда в интервале используется 
        /// </summary>
        [TestMethod]
        public void TestSingleGoalInterval()
        {
            for (int i = MinGoals; i <= MaxGoals; i++)
            {
                var interval = new GoalsInterval(i);
                var inputData = new InputData(Constants.MinutesPerMatch, EmptyGoals, interval, Data.MeanHostGoals,
                    Data.MeanGuestGoals);
                var res = ProbModel.ComputeProbs(inputData);

            }
        }

        /// <summary>
        /// Проверка для уже совершенных голов
        /// </summary>
        [TestMethod]
        public void TestGoalsThatDone()
        {
            for (int numGoals = MinGoals; numGoals <= MaxGoals; numGoals++)
            {
                //Интервал включает все уже совершенные голы
                var interval = new GoalsInterval(1, numGoals);
                var goalsList = new List<GoalType>();
                for (int hostGoalsCount = 1; hostGoalsCount <= interval.Length; hostGoalsCount++)
                {
                    for (int k = 1; k <= hostGoalsCount; k++)
                    {
                        goalsList.Add(GoalType.Host);
                    }
                   
                    int guestGoalsCount = interval.Length - hostGoalsCount;
                    for (int i = 0; i < guestGoalsCount; i++)
                    {
                        goalsList.Add(GoalType.Guest);
                    }
                    var hostsWonProbExpected = hostGoalsCount > guestGoalsCount? 1d : 0d;
                    var guestsWonProbExpected = guestGoalsCount > hostGoalsCount ? 1d : 0d;
                    var drawProbExpected = 1 - hostsWonProbExpected - guestsWonProbExpected;
                    var inputData = new InputData(Constants.MinutesPerMatch, goalsList, interval, Data.MeanHostGoals,
                        Data.MeanGuestGoals);
                    var probsResult = ProbModel.ComputeProbs(inputData);
                    var guestsWonProb = probsResult.GuestsWonProb;
                    var hostsWonProb = probsResult.HostsWonProb;
                    var drawProb = probsResult.DrawProb;
                    Assert.AreEqual(hostsWonProbExpected, hostsWonProb, Delta);
                    Assert.AreEqual(guestsWonProbExpected, guestsWonProb, Delta);
                    Assert.AreEqual(drawProbExpected, drawProb, Delta);
                }

              
            }

        }

    }
}
