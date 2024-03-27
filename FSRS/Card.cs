using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public class Card
    {
        public DateTime Due { get; set; }
        public double Stability { get; set; }
        public double Difficulty { get; set; }
        public int ElapsedDays { get; set; }
        public int ScheduledDays { get; set; }
        public int Reps { get; set; }
        public int Lapses { get; set; }
        public State State { get; set; }
        public DateTime LastReview { get; set; }

        public Card()
        {
            Due = DateTime.Now;
            Stability = 0;
            Difficulty = 0;
            ElapsedDays = 0;
            ScheduledDays = 0;
            Reps = 0;
            Lapses = 0;
            State = State.New;
            LastReview = DateTime.Now;
        }
    }
}
