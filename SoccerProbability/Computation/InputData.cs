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
        public double MeanHost { get; private set; }

        /// <summary>
        /// Средняя интенсивность голов гостей
        /// </summary>
        public double MeanGuest { get; private set; }

        public InputData(int minutesTillEnd, IEnumerable<GoalType> goals, GoalsInterval interval, double meanHost, double meanGuest)
        {
            MinutesTillEnd = minutesTillEnd;
            Goals = goals.ToArray();
            Interval = interval;
            MeanHost = meanHost;
            MeanGuest = meanGuest;
        }
    }
}
