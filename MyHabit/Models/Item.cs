using System;
using System.Collections.Generic;

namespace MyHabit.Models
{
    public class Item
    {
        public Item()
        {
            StartDate = DateTime.Now;
            Tracker = new Tracker();
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Archieved { get; set; }
        public Tracker Tracker { get; }
        public string DayHabitStatus { get; set; }
        public string[] Habits
        {
            get
            {
                return Enum.GetNames(typeof(HabitStatus));
            }
        }
    }
}