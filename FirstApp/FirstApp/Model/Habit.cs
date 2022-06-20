using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace FirstApp.Model
{
    public class Habit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [OneToMany]
        public List<HabitPlan> HabitPlans { get; set; }
    }
}
