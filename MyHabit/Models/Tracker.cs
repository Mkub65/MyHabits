using System;
using System.Collections.Generic;

namespace MyHabit.Models
{
    public class Tracker
    {
        private readonly Dictionary<DateTime, HabitStatus> tracker = new Dictionary<DateTime, HabitStatus>();

        public void SetStatus(DateTime date, HabitStatus status)
        {
            if (tracker.ContainsKey(date))
            {
                tracker.Remove(date);
            }

            tracker[date] = status;
        }

        public HabitStatus GetStatus(DateTime date)
        {
            if (tracker.ContainsKey(date))
            {
                return tracker[date];
            }

            return HabitStatus.None;
        }
    }
}