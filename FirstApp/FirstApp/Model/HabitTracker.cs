using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace FirstApp.Model
{
    internal class HabitTracker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Status { get; set; }

        [ForeignKey(typeof(HabitPlan))]
        public int HabitPlanId { get; set; }
    }
}
