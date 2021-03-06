﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.DataPreprocessing
{
    public static class StatsData
    {
        public static readonly (int, int, double)[] MatchStats =
        {
            (0, 0, 4.33),
            (0, 1, 6.05),
            (0, 2, 4.24),
            (0, 3, 1.98),
            (0, 4, 0.7),
            (0, 5, 0.19),
            (1, 0, 7.57),
            (1, 1, 10.59),
            (1, 2, 7.41),
            (1, 3, 3.46),
            (1, 4, 1.21),
            (1, 5, 0.34),
            (2, 0, 6.62),
            (2, 1, 9.27),
            (2, 2, 6.48),
            (2, 3, 3.03),
            (2, 4, 1.06),
            (2, 5, 0.29),
            (3, 0, 3.86),
            (3, 1, 5.41),
            (3, 2, 3.78),
            (3, 3, 1.76),
            (3, 4, 0.62),
            (3, 5, 0.17),
            (4, 0, 1.68),
            (4, 1, 2.36),
            (4, 2, 1.65),
            (4, 3, 0.78),
            (4, 4, 0.27),
            (5, 0, 0.59),
            (5, 1, 0.83),
            (5, 2, 0.57),
            (5, 3, 0.27),
            (6, 0, 0.17),
            (6, 1, 0.24),
            (6, 2, 0.17)
        };

        public const double MeanHostGoals = 1.7262 / Constants.MinutesPerMatch;
        public const double MeanGuestGoals = 1.3758 / Constants.MinutesPerMatch;


    }
}
