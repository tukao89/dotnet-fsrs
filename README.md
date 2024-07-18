# FSRS for .Net C#

This is C# version of https://github.com/open-spaced-repetition/fsrs.js

Package that implements the Free Spaced Repetition Scheduler algorithm. It helps developers apply FSRS in their flashcard apps.

# How to use #

```
var fsrs = new FSRS();
var card = new Card();
fsrs.P.RequestRetention = 0.9;
fsrs.P.MaximumInterval = 36500;
fsrs.P.W = new double[] { 0.4, 0.6, 2.4, 5.8, 4.93, 0.94, 0.86, 0.01, 1.49, 0.14, 0.94, 2.18, 0.05, 0.34, 1.26, 0.29, 2.61 };

var now = DateTime.Now;
var schedulingCards = fsrs.Repeat(card, now);

//Update the card after rating `Good`:
card = schedulingCards[Rating.Good].Card;

var due = card.Due;
var state = card.State;

var review_log = schedulingCards[Rating.Good].ReviewLog;
```
