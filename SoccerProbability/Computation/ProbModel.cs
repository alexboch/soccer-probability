using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    static class ProbModel
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
        /// <param name="mean">Средняя интенсивность</param>
        /// <param name="minutes">Кол-во минут, в течении которых может наступить k событий</param>
        /// <returns></returns>
        private static double Pk(int k, double mean, int minutes)
        {
            var l = mean * minutes;
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
            var meanHost = input.MeanHost;
            var meanGuest = input.MeanGuest;
            if (firstGoalIndex < goalsBefore.Length)
            {

                for (int i = firstGoalIndex; i < goalsBefore.Length; i++)
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
            int goalsRemain = interval.Length;
            //Вероятности выигрышей (с закрытием интервала)
            double hostsWinProb = 0;
            double guestsWinProb = 0;
            //Вероятность ничьей с закрытием интервала
            double drawProb = 0;
            for (int k = 0; k <= goalsRemain; k++)
            {
                //Если интервал закрыт, произойдут все голы
                int k1 = goalsRemain - k;
                //todo: Оптимизировать вычисление вероятностей
                //Вероятность, что хозяева забьют k раз
                var pk = Pk(k, meanHost, minutesLeft);
                //Вероятность, что гости забьют k1 раз
                var pk1 = Pk(k1, meanGuest, minutesLeft);
                //Вероятность совместного наступления
                var p = pk * pk1;
                var hostsTotalGoals = hostsGoalsBeforeInInterval + k;
                var guestsTotalGoals = guestGoalsBeforeInInterval + k1;

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
            //Через обратную вероятность
            var notFinishedProb = 1 - (hostsWinProb + guestsWinProb + drawProb);
            return new ResultProbs(hostsWinProb, guestsWinProb, drawProb, notFinishedProb);
        }
    }
}
