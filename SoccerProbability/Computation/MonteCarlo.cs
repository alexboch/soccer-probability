using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    public static class MonteCarlo
    {
        public static async Task<ResultProbs> Generate(InputData inputData, int numMatches)
        {
            var generatedRes = await Task.Run(() =>
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
                    int numGoalsHost = 0;
                    int numGoalsGuest = 0;
                    for (int l = inputData.Interval.From; l <= inputData.Interval.To; l++)
                    {
                        if (l > inputData.Goals.Length)
                        {
                            break;
                        }

                        if (inputData.Goals[l - 1] is GoalType.Host)
                        {
                            numGoalsHost++;
                        }
                        else
                        {
                            numGoalsGuest++;
                        }
                    }

                    if (goalsRemain > 0)
                    {
                        int goalsCounter = goalsRemain;
                        for (int j = 0; j < inputData.MinutesTillEnd; j++)
                        {
                            var r1 = rand.NextDouble();
                            var r2 = rand.NextDouble();

                            if (r1 <= meanIntensityHost)
                            {
                                //Гол хозяев
                                numGoalsHost++;
                                goalsCounter--;
                            }

                            if (goalsCounter == 0)
                            {
                                break;
                            }

                            if (r2 <= meanIntensityGuest)
                            {
                                //Гол гостей
                                numGoalsGuest++;
                                goalsCounter--;
                            }

                            if (goalsCounter == 0)
                            {
                                break;
                            }

                        }
                    }

                    if (numGoalsHost + numGoalsGuest < inputData.Interval.Length)
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

                var hostsWonFrac = (double) numHostsWin / numMatches;
                var guestsWonFrac = (double) numGuestsWin / numMatches;
                var drawFrac = (double) numDraws / numMatches;
                var notFinishedFrac = (double) numNotFinished / numMatches;
                var res = new ResultProbs(hostsWonFrac, guestsWonFrac, drawFrac, notFinishedFrac);
                return res;
            });
            return generatedRes;
        }


    }
}
