using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    /// <summary>
    /// Содержит вероятности исходов
    /// </summary>
    class ResultProbs
    {
        /// <summary>
        /// Вероятность победы хозяев на заданном интервале голов,
        /// </summary>
        public double HostsWonProb { get; private set; }
        /// <summary>
        /// Вероятность победы гостей на заданном интервале голов,
        /// </summary>
        public double GuestsWonProb { get; private set; }
        /// <summary>
        /// Вероятность ничьи
        /// </summary>
        public double DrawProb { get; private set; }
        /// <summary>
        /// Вероятность что интервал не будет закрыт
        /// </summary>
        public double NotFinishedProb { get; private set; }

        public ResultProbs(double hostsWonProb, double guestsWonProb, double drawProb, double notFinishedProb)
        {
            HostsWonProb = hostsWonProb;
            GuestsWonProb = guestsWonProb;
            DrawProb = drawProb;
            NotFinishedProb = notFinishedProb;

        }

    }
}
