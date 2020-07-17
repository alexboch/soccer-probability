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
        /// Биномиальный коэффициент
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static double C(int n, int k)
        {
            return Fact(n) / (Fact(n - k) * Fact(k));
        }

        /// <summary>
        /// функция распредения биномиального распределения
        /// </summary>
        /// <param name="y"></param>
        /// <param name="n">Общее кол-во испытаний</param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static double CDFBinom(int y, int n, double p)
        {
            var sum = 0d;
            var q = 1 - p;
            for (int k = 0; k <= y; k++)
            {
                var s = C(n, k) * Math.Pow(p, k) * Math.Pow(q, n - k);
                sum += s;
            }

            return sum;
        }

        private static double ProbBinom(int n, int k, double p)
        {
            var q = 1 - p;
            return C(n, k) * Math.Pow(p, k) * Math.Pow(q, n - k);
        }

        /// <summary>
        /// Вероятность, что событие наступит k раз, для простейшего потока событий
        /// </summary>
        /// <param name="k"></param>
        /// <param name="meanByMinutes">Средняя интенсивность</param>
        /// <param name="minutes">Кол-во минут, в течении которых может наступить k событий</param>
        /// <returns>Кортеж, содержащий числитель и знаменатель</returns>
        private static double ProbPoisson(int k, double l)
        {
            var exp = Math.Exp(-l);
            var lPow = Math.Pow(l, k);
            var f = Fact(k);
            return lPow / f * exp;
        }

        /// <summary>
        /// Функция распределения для распределения Пуассона
        /// </summary>
        /// <returns></returns>
        public static double CDFPoisson(int k, double l)
        {
            var sum = 0d;
            for (int i = 0; i <= k; i++)
            {
                sum += Math.Pow(l, i) / Fact(i);
            }

            return Math.Exp(-l) * sum;
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
                
                var l1 = meanHost * minutesTillEnd;
                var l2 = meanGuest * minutesTillEnd;
                notFinishedProb = CDFPoisson(goalsRemain - 1, l1 + l2);
                var ps = l1 + l2;
                var pHost = l1 / ps;
                var pGuest = l2 / ps;
                var finishedProb = 1 - notFinishedProb;
                
                for (int hostGoals = 0; hostGoals <= goalsRemain; hostGoals++)
                {
                    var p = ProbBinom(goalsRemain, hostGoals, pHost);
                    var guestGoals = goalsRemain - hostGoals;
                    var totalHostGoals = hostGoals + hostsGoalsBeforeInInterval;
                    var totalGuestGoals = guestGoals + guestGoalsBeforeInInterval;
                    if (totalHostGoals > totalGuestGoals)
                    {
                        hostsWinProb += p;
                    }
                    else if (totalGuestGoals > totalHostGoals)
                    {
                        guestsWinProb += p;
                    }
                    else
                    {
                        drawProb += p;
                    }

                }

                hostsWinProb *= finishedProb;
                guestsWinProb *= finishedProb;
                drawProb *= finishedProb;
                //hostsWinProb = pHost * finishedProb;
                //guestsWinProb = pGuest * finishedProb;

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
