namespace FSRS
{
    public class ReviewLog
    {
        public Rating Rating { get; set; }
        public int ScheduledDays { get; set; }
        public int ElapsedDays { get; set; }
        public DateTime Review { get; set; }
        public State State { get; set; }

        public ReviewLog(Rating rating, int scheduledDays, int elapsedDays, DateTime review, State state)
        {
            Rating = rating;
            ScheduledDays = scheduledDays;
            ElapsedDays = elapsedDays;
            Review = review;
            State = state;
        }
    }
}
