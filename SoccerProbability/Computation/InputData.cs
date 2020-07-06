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
    class InputData
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

        public double MeanIntensity { get; private set; }
    }
}
