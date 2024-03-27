using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public class SchedulingInfo
    {
        public Card Card { get; set; }
        public ReviewLog ReviewLog { get; set; }

        public SchedulingInfo(Card card, ReviewLog reviewLog)
        {
            Card = card;
            ReviewLog = reviewLog;
        }
    }

}
