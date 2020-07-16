using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    public static class MonteCarlo
    {
        public static ResultProbs Generate(InputData inputData, int numMatches)
        {
            var meanIntensityHost = inputData.MeanIntensityHost;
            var meanIntensityGuest = inputData.MeanIntensityGuest;
            var rand = new Random();
            int numHostsWin = 0;
            int numGuestsWin = 0;
            int numNotFinished = 0;
            int numDraws = 0;
            int goalsRemain = inputData.GoalsRemain;
            for (int i = 0; i < numMatches; i++)
            {
                var numGoalsHost = inputData.Goals.Count(g => g is GoalType.Host);
                var numGoalsGuest = inputData.Goals.Count(g => g is GoalType.Guest);
                if (goalsRemain > 0)
                {
                    for (int j = 0; j < inputData.MinutesTillEnd; j++)
                    {
                        var r1 = rand.NextDouble();
                        var r2 = rand.NextDouble();

                        if (r1 <= meanIntensityHost)
                        {
                            //Гол хозяев
                            numGoalsHost++;
                            if (numGoalsHost + numGoalsGuest == goalsRemain)
                            {
                                break;
                            }
                        }

                        if (r2 <= meanIntensityGuest)
                        {
                            //Гол гостей
                            numGoalsGuest++;
                            if (numGoalsHost + numGoalsGuest == goalsRemain)
                            {
                                break;
                            }
                        }
                    }
                }

                if (numGoalsHost + numGoalsGuest < goalsRemain)
                {
                    numNotFinished++;
                }
                else
                {
                    if (numGoalsHost > numGoalsGuest)
                    {
                        numHostsWin++;
                    }

                    if (numGoalsGuest > numGoalsHost)
                    {
                        numGuestsWin++;
                    }

                    if (numGoalsGuest == numGoalsHost)
                    {
                        numDraws++;
                    }
                }

            }

            var hostsWonFrac = (double)numHostsWin / numMatches;
            var guestsWonFrac = (double)numGuestsWin / numMatches;
            var drawFrac = (double)numDraws / numMatches;
            var notFinishedFrac = (double) numNotFinished / numMatches;
            var res = new ResultProbs(hostsWonFrac, guestsWonFrac, drawFrac, notFinishedFrac);
            return res;
        }


    }
}
