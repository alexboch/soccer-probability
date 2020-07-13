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
        private static (double, double) Pk(int k, double l)
        {
            var exp = Math.Exp(-l);
            var lPow = Math.Pow(l, k);
            var f = Fact(k);
            return (lPow * exp, f);
            //return lPow / f * exp;
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

            //Расчет вероятности того, что хозяева забьют k голов, при этом гости забьют k1 голов
            int goalsRemain = interval.To - goalsBefore.Length;
            //Вероятности выигрышей (с закрытием интервала)
            double hostsWinProb = 0;
            double guestsWinProb = 0;
            //Вероятность ничьей с закрытием интервала
            double drawProb = 0;
            var l1 = meanHost * minutesLeft;
            var l2 = meanGuest * minutesLeft;

            if (goalsRemain > 0)
            {
                double pk1 = 0;
                double pk2 = 0;

                double num1 = 1;
                double den1 = 1;
                double num2 = 1;
                double den2 = 1;
                for (int i = 0; i <= goalsRemain; i++)
                {
                    int hostsTotalGoals = hostsGoalsBeforeInInterval;
                    int guestsTotalGoals = guestGoalsBeforeInInterval;
                    //Если интервал закрыт, будут забиты все голы
                    int newGuestGoals = goalsRemain - i;

                    int prevGuestGoals = newGuestGoals + 1;
                    if (i <= 1 || newGuestGoals <= 1)
                    {
                        //Вероятность, что хозяева забьют k1 раз
                        (num1, den1) = Pk(i, l1);
                        //Вероятность, что гости забьют k2 раз
                        (num2, den2) = Pk(newGuestGoals, l2);
                    }
                    else
                    {
                        num1 *= l1;
                        den1 *= i;
                        num2 *= prevGuestGoals;
                        den2 *= l2;
                    }

                    pk1 = num1 / den1;
                    pk2 = num2 / den2;
                    //Вероятность совместного наступления
                    var p = pk1 * pk2;
                    hostsTotalGoals += i;
                    guestsTotalGoals += newGuestGoals;
                    if (hostsTotalGoals > guestsTotalGoals)
                    {
                        //Если хозяева выиграют
                        hostsWinProb += p;
                    }
                    if (guestsTotalGoals > hostsTotalGoals)
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
            //Через обратную вероятность
            var notFinishedProb = 1 - (hostsWinProb + guestsWinProb + drawProb);
            return new ResultProbs(hostsWinProb, guestsWinProb, drawProb, notFinishedProb);
        }
    }
}
