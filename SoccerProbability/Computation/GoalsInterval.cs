using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    public class GoalsInterval
    {

        public int From { get; private set; }
        public int To { get; private set; }

        public int Length => To - From + 1 ;

        public GoalsInterval(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}
