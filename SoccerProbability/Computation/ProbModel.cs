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
            int minutesTillEnd = input.MinutesTillEnd;
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
           
            var notFinishedProb = 0d;
            if (goalsRemain > 0)
            {
                var prevGuestsWonProbInMinute = 0d;
                var prevHostsWonProbInMinute = 0d;
                var prevDrawProbInMinute = 0d;
                var prevNotFinishedProbInMinute = 0d;
                for (int minutes = 0; minutes < minutesTillEnd; minutes++)
                {
                    var l1 = meanHost * minutes;
                    var l2 = meanGuest * minutes;
                    var guestsWonProbInMinute = 0d;
                    var hostsWonProbInMinute = 0d;
                    var drawProbInMinute = 0d;
                    var notFinishedProbInMinute = 0d;

                 
                    for (int newHostGoals = 0; newHostGoals <= goalsRemain; newHostGoals++)
                    {
                        int hostsTotalGoals = hostsGoalsBeforeInInterval + newHostGoals;
                        for (int newGuestGoals = goalsRemain - newHostGoals; newGuestGoals >= 0; newGuestGoals--)
                        {
                            int guestsTotalGoals = guestGoalsBeforeInInterval + newGuestGoals;
                            //Вероятность, что хозяева забьют k1 раз
                            double pk1 = Pk(newHostGoals, l1);
                            //Вероятность, что гости забьют k2 раз
                            double pk2 = Pk(newGuestGoals, l2);
                            //Вероятность совместного наступления
                            var p = pk1 * pk2;
                            //Если интервал закрывается
                            if (newGuestGoals + newHostGoals == goalsRemain)
                            {
                                if (hostsTotalGoals > guestsTotalGoals)
                                {
                                    //Если хозяева выиграют
                                    //hostsWinProb += p;
                                    hostsWonProbInMinute += p;
                                }
                                else if (guestsTotalGoals > hostsTotalGoals)
                                {
                                    //Если гости выиграют
                                    //guestsWinProb += p;
                                    guestsWonProbInMinute += p;
                                }
                                else
                                {
                                    //Ничья
                                    //drawProb += p;
                                    drawProbInMinute += p;
                                }
                            }
                            else
                            {
                                //Интервал не завершен
                                //notFinishedProb += p;
                                notFinishedProbInMinute += p;
                            }
                        }
                    }
                    //Вероятность через столько минут минус вероятность за предыдущие минуты
                    hostsWonProbInMinute -= prevHostsWonProbInMinute;
                    guestsWonProbInMinute -= prevGuestsWonProbInMinute;
                    drawProbInMinute -= prevDrawProbInMinute;
                    notFinishedProbInMinute -= prevNotFinishedProbInMinute;

                    hostsWinProb += hostsWonProbInMinute;
                    guestsWinProb += guestsWonProbInMinute;
                    drawProb += drawProbInMinute;
                    notFinishedProb += notFinishedProbInMinute;

                    prevGuestsWonProbInMinute = guestsWonProbInMinute;
                    prevHostsWonProbInMinute = hostsWonProbInMinute;
                    prevDrawProbInMinute = drawProbInMinute;
                    prevNotFinishedProbInMinute = notFinishedProbInMinute;
                }
            }
            else
            {
                //Если все голы 
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
