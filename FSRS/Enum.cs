using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public enum State
    {
        New = 0,
        Learning = 1,
        Review = 2,
        Relearning = 3,
    }

    public enum Rating
    {
        Again = 1,
        Hard = 2,
        Good = 3,
        Easy = 4,
    }
    internal class Enum
    {
    }
}
