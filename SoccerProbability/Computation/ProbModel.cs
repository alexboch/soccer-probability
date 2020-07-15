using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    public static class ProbModel
    {

        /// <summary>
        /// Вычисляет факториал
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private static double Fact(int k)
        {
            double f = 1;
            for (int i = 2; i <= k; i++)
            {
                f *= i;
            }

            return f;
        }

        /// <summary>
        /// Вероятность, что событие наступит k раз, для простейшего потока событий
        /// </summary>
        /// <param name="k"></param>
        /// <param name="meanByMinutes">Средняя интенсивность</param>
        /// <param name="minutes">Кол-во минут, в течении которых может наступить k событий</param>
        /// <returns>Кортеж, содержащий числитель и знаменатель</returns>
        private static double Pk(int k, double l)
        {
            var exp = Math.Exp(-l);
            var lPow = Math.Pow(l, k);
            var f = Fact(k);
            return lPow / f * exp;
        }


        public static ResultProbs ComputeProbs(InputData input)
        {
            var goalsBefore = input.Goals;
            int minutesLeft = input.MinutesTillEnd;
            var interval = input.Interval;
            int firstGoalIndex = interval.From - 1;
            //Голов хозяев, сделанных в рассматриваемом интервале
            int hostsGoalsBeforeInInterval = 0;
            int guestGoalsBeforeInInterval = 0;
            //Средняя интенсивность потока событий
            var meanHost = input.MeanIntensityHost;
            var meanGuest = input.MeanIntensityGuest;
            int maxIndex = interval.To < goalsBefore.Length ? interval.To : goalsBefore.Length;
            //Если некоторые голы уже произошли
            if (firstGoalIndex < goalsBefore.Length)
            {
                //Цикл от первого гола до последнего гола из уже произошедших
                for (int i = firstGoalIndex; i < maxIndex; i++)
                {
                    if (goalsBefore[i] == GoalType.Host)
                    {
                        hostsGoalsBeforeInInterval++;
                    }
                    else
                    {
                        guestGoalsBeforeInInterval++;
                    }
                }
            }

            int goalsRemain = interval.To - goalsBefore.Length;
            //Вероятности выигрышей (с закрытием интервала)
            double hostsWinProb = 0;
            double guestsWinProb = 0;
            //Вероятность ничьей с закрытием интервала
            double drawProb = 0;
            var l1 = meanHost * minutesLeft;
            var l2 = meanGuest * minutesLeft;
            var notFinishedProb = 0d;
            if (goalsRemain > 0)
            {
                for (int i = 0; i <= goalsRemain; i++)
                {
                    int hostsTotalGoals = hostsGoalsBeforeInInterval + i;
                    for (int newGuestGoals = goalsRemain - i; newGuestGoals >= 0; newGuestGoals--)
                    {
                        int guestsTotalGoals = guestGoalsBeforeInInterval + newGuestGoals;
                        //Вероятность, что хозяева забьют k1 раз
                        double pk1 = Pk(i, l1); 
                        //Вероятность, что гости забьют k2 раз
                        double pk2 = Pk(newGuestGoals, l2); 
                        //Вероятность совместного наступления
                        var p = pk1 * pk2;
                        //Если интервал закрывается
                        if (newGuestGoals + i == goalsRemain)
                        {
                            if (hostsTotalGoals > guestsTotalGoals)
                            {
                                //Если хозяева выиграют
                                hostsWinProb += p;
                            }
                            else if (guestsTotalGoals > hostsTotalGoals)
                            {
                                //Если гости выиграют
                                guestsWinProb += p;
                            }
                            else
                            {
                                //Ничья
                                drawProb += p;
                            }
                        }
                        else
                        {
                            //Интервал не завершен
                            notFinishedProb += p;
                        }
                    }
                }
            }
            else
            {
                if (guestGoalsBeforeInInterval == hostsGoalsBeforeInInterval)
                {
                    //Если кол-во голов одинаковое, то ничья
                    guestsWinProb = 0;
                    hostsWinProb = 0;
                    drawProb = 1;
                }
                else if(guestGoalsBeforeInInterval > hostsGoalsBeforeInInterval)
                {
                    //Выиграли гости
                    guestsWinProb = 1;
                    hostsWinProb = 0;
                    drawProb = 0;
                }
                else
                {
                    //Выиграли хозяева
                    guestsWinProb = 0;
                    hostsWinProb = 1;
                    drawProb = 0;
                }
            }
            return new ResultProbs(hostsWinProb, guestsWinProb, drawProb, notFinishedProb);
        }
    }
}
