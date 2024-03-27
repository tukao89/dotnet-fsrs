using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public class SchedulingCards
    {
        public Card Again { get; set; }
        public Card Hard { get; set; }
        public Card Good { get; set; }
        public Card Easy { get; set; }

        public SchedulingCards(Card card)
        {
            Again = new Card();
            Hard = new Card();
            Good = new Card();
            Easy = new Card();
        }

        public void UpdateState(State state)
        {
            if (state == State.New)
            {
                Again.State = State.Learning;
                Hard.State = State.Learning;
                Good.State = State.Learning;
                Easy.State = State.Review;
            }
            else if (state == State.Learning || state == State.Relearning)
            {
                Again.State = state;
                Hard.State = state;
                Good.State = State.Review;
                Easy.State = State.Review;
            }
            else if (state == State.Review)
            {
                Again.State = State.Relearning;
                Hard.State = State.Review;
                Good.State = State.Review;
                Easy.State = State.Review;
                Again.Lapses += 1;
            }
        }

        public void Schedule(DateTime now, int hardInterval, int goodInterval, int easyInterval)
        {
            Again.ScheduledDays = 0;
            Hard.ScheduledDays = hardInterval;
            Good.ScheduledDays = goodInterval;
            Easy.ScheduledDays = easyInterval;

            Again.Due = now.AddMinutes(5);
            if (hardInterval > 0)
            {
                Hard.Due = now.AddDays(hardInterval);
            }
            else
            {
                Hard.Due = now.AddMinutes(10);
            }
            Good.Due = now.AddDays(goodInterval);
            Easy.Due = now.AddDays(easyInterval);
        }

        public Dictionary<Rating, SchedulingInfo> RecordLog(Card card, DateTime now)
        {
            return new Dictionary<Rating, SchedulingInfo>
        {
            { Rating.Again, new SchedulingInfo(Again, new ReviewLog(Rating.Again, Again.ScheduledDays, card.ElapsedDays, now, card.State)) },
            { Rating.Hard, new SchedulingInfo(Hard, new ReviewLog(Rating.Hard, Hard.ScheduledDays, card.ElapsedDays, now, card.State)) },
            { Rating.Good, new SchedulingInfo(Good, new ReviewLog(Rating.Good, Good.ScheduledDays, card.ElapsedDays, now, card.State)) },
            { Rating.Easy, new SchedulingInfo(Easy, new ReviewLog(Rating.Easy, Easy.ScheduledDays, card.ElapsedDays, now, card.State)) },
        };
        }
    }
}
