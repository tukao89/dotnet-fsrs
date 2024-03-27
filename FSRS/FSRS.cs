using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public class FSRS
    {
        public Params P { get; set; }

        public FSRS()
        {
            P = new Params();
        }

        public Dictionary<Rating, SchedulingInfo> Repeat(Card card, DateTime now)
        {
            // This function should implement the logic of repeating a card based on its current state and the current time.
            // It should return a dictionary mapping from the rating to the scheduling info for each possible rating.
            card = card.DeepCopy(); // Tạo một bản sao của thẻ
            if (card.State == State.New)
            {
                card.ElapsedDays = 0;
            }
            else
            {
                card.ElapsedDays = Convert.ToInt32((now - card.LastReview).TotalDays);
            }
            card.LastReview = now;
            card.Reps += 1;

            SchedulingCards s = new SchedulingCards(card);
            s.UpdateState(card.State);

            if (card.State == State.New)
            {
                InitDs(s);
                s.Again.Due = now.AddMinutes(1);
                s.Hard.Due = now.AddMinutes(5);
                s.Good.Due = now.AddMinutes(10);
                int easyInterval = NextInterval(s.Easy.Stability);
                s.Easy.ScheduledDays = easyInterval;
                s.Easy.Due = now.AddDays(easyInterval);
            }
            else if (card.State == State.Learning || card.State == State.Relearning)
            {
                int hardInterval = 0;
                int goodInterval = NextInterval(s.Good.Stability);
                int easyInterval = Math.Max(NextInterval(s.Easy.Stability), goodInterval + 1);
                s.Schedule(now, hardInterval, goodInterval, easyInterval);
            }
            else if (card.State == State.Review)
            {
                int interval = (int)card.ElapsedDays;
                double lastD = card.Difficulty;
                double lastS = card.Stability;
                double retrievability = Math.Pow(1 + interval / (9 * lastS), -1);
                NextDs(s, lastD, lastS, retrievability);
                int hardInterval = NextInterval(s.Hard.Stability);
                int goodInterval = NextInterval(s.Good.Stability);
                hardInterval = Math.Min(hardInterval, goodInterval);
                goodInterval = Math.Max(goodInterval, hardInterval + 1);
                int easyInterval = Math.Max(NextInterval(s.Easy.Stability), goodInterval + 1);
                s.Schedule(now, hardInterval, goodInterval, easyInterval);
            }

            return s.RecordLog(card, now);
        }

        public void InitDs(SchedulingCards s)
        {
            // This function should initialize the difficulty and stability of the scheduling cards based on the parameters of the FSRS.
            s.Again.Difficulty = InitDifficulty(Rating.Again);
            s.Again.Stability = InitStability(Rating.Again);
            s.Hard.Difficulty = InitDifficulty(Rating.Hard);
            s.Hard.Stability = InitStability(Rating.Hard);
            s.Good.Difficulty = InitDifficulty(Rating.Good);
            s.Good.Stability = InitStability(Rating.Good);
            s.Easy.Difficulty = InitDifficulty(Rating.Easy);
            s.Easy.Stability = InitStability(Rating.Easy);
        }

        public void NextDs(SchedulingCards s, double lastD, double lastS, double retrievability)
        {
            s.Again.Difficulty = this.NextDifficulty(lastD, Rating.Again);
            s.Again.Stability = this.NextForgetStability(
              lastD,
              lastS,
              retrievability
            );
            s.Hard.Difficulty = this.NextDifficulty(lastD, Rating.Hard);
            s.Hard.Stability = this.NextRecallStability(
              lastD,
              lastS,
              retrievability,
              Rating.Hard
            );
            s.Good.Difficulty = this.NextDifficulty(lastD, Rating.Good);
            s.Good.Stability = this.NextRecallStability(
              lastD,
              lastS,
              retrievability,
              Rating.Good
            );
            s.Easy.Difficulty = this.NextDifficulty(lastD, Rating.Easy);
            s.Easy.Stability = this.NextRecallStability(
              lastD,
              lastS,
              retrievability,
              Rating.Easy
            );
        }

        public double InitStability(Rating r)
        {
            // This function should calculate the initial stability based on the rating.
            // It should return the initial stability as a double.
            return Math.Max(this.P.W[(int)r - 1], 0.1);
        }

        public double InitDifficulty(Rating r)
        {
            // This function should calculate the initial difficulty based on the rating.
            // It should return the initial difficulty as a double.
            return Math.Min(Math.Max(this.P.W[4] - this.P.W[5] * ((int)r - 3), 1), 10);
        }

        public int NextInterval(double s)
        {
            // This function should calculate the next interval based on the stability.
            // It should return the next interval as an integer.
            double interval = s * 9 * (1 / P.RequestRetention - 1);
            return Math.Min(Math.Max((int)Math.Round(interval), 1), P.MaximumInterval);
        }

        public double NextDifficulty(double d, Rating r)
        {
            // This function should calculate the next difficulty based on the current difficulty and rating.
            // It should return the next difficulty as a double.
            double next_d = d - this.P.W[6] * ((int)r - 3);
            return Math.Min(Math.Max(this.MeanReversion(this.P.W[4], next_d), 1), 10);
        }

        public double MeanReversion(double init, double current)
        {
            // This function should calculate the mean reversion based on the initial and current values.
            // It should return the mean reversion as a double.
            return this.P.W[7] * init + (1 - this.P.W[7]) * current;
        }

        public double NextRecallStability(double d, double s, double r, Rating rating)
        {
            // This function should calculate the next recall stability based on the current difficulty, stability, recall, and rating.
            // It should return the next recall stability as a double.
            double hardPenalty = rating == Rating.Hard ? this.P.W[15] : 1;
            double easyBonus = rating == Rating.Easy ? this.P.W[16] : 1;
            return (
              s * (1 + Math.Exp(this.P.W[8]) * (11 - d) * Math.Pow(s, -this.P.W[9]) * (Math.Exp((1 - r) * this.P.W[10]) - 1) * hardPenalty * easyBonus)
            );
        }

        public double NextForgetStability(double d, double s, double r)
        {
            // This function should calculate the next forget stability based on the current difficulty, stability, and recall.
            // It should return the next forget stability as a double.
            return (this.P.W[11] * Math.Pow(d, -this.P.W[12]) * (Math.Pow(s + 1, this.P.W[13]) - 1) * Math.Exp((1 - r) * this.P.W[14]));
        }
    }
}
