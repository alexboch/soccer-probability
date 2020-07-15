using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    /// <summary>
    /// Входные данные
    /// </summary>
    public class InputData
    {
        /// <summary>
        /// Минут до конца матча
        /// </summary>
        public int MinutesTillEnd
        {
            get; private set;
        }

        /// <summary>
        /// Забитые на текущий момент голы в порядке их реализации
        /// </summary>
        public GoalType[] Goals { get; private set; }

        /// <summary>
        /// Интервал голов
        /// </summary>
        public GoalsInterval Interval { get; private set; }

        /// <summary>
        /// Средняя интенсивность голов хозяев
        /// </summary>
        public double MeanIntensityHost { get; private set; }

        /// <summary>
        /// Средняя интенсивность голов гостей
        /// </summary>
        public double MeanIntensityGuest { get; private set; }

        public int GoalsRemain => Interval.To - Goals.Length;

        public InputData(int minutesTillEnd, IEnumerable<GoalType> goals, GoalsInterval interval, double meanIntensityHost, double meanIntensityGuest)
        {
            MinutesTillEnd = minutesTillEnd;
            Goals = goals.ToArray();
            Interval = interval;
            MeanIntensityHost = meanIntensityHost;
            MeanIntensityGuest = meanIntensityGuest;
        }
    }
}
