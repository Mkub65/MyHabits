using SQLite;
using System;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace FirstApp.Model
{
    public class HabitPlan
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Habit))]
        public int HabitId  { get; set; }

        public string HabitPlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
