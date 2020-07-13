using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerProbability.Computation
{
    public class GoalsInterval
    {

        private int _from;
        public int From
        {
            get => _from;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                _from = value;
            }
        }

        private int _to;

        public int To
        {
            get => _to;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                _to = value;

            }
        }

        //public int Length => To - From + 1 ;

        public int Length
        {
            get
            {
                if (To - From > 0)
                {
                    return To - From + 1;
                }

                return 1;
            }
        }

        public GoalsInterval(int goalNumber)
        {
            From = goalNumber;
            To = goalNumber;
        }

        public GoalsInterval(int from, int to)
        {
            if (from > to)
            {
                throw new ArgumentException("Номер гола конца интервала должна быть больше или равен номеру гола начала интервала");
            }

            From = from;
            To = to;
        }
    }
}
