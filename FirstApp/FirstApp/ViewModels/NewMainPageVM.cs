using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FirstApp.ViewModels
{
    public class NewMainPageVM
    {
        public Dictionary<string,string> ActiveHabitPlans { get; set; }
        public NewMainPageVM()
        {
            ActiveHabitPlans = new Dictionary<string, string>();
        }
        public void GetHabitPlans()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                var now = DateTime.Now.Date;
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from n in habitPlans
                             where n.StartDate <= now && n.EndDate > now
                             select n).ToList();
                conn.CreateTable<HabitTracker>();
                var Trackers = conn.Table<HabitTracker>();
                foreach (var pair in Plans)
                {
                    var selectedTrackerStatus = (from n in Trackers where n.UpdateDate == now && n.HabitPlanId == pair.Id select n).ToList();
                    if (selectedTrackerStatus.Count > 0)
                    {
                        ActiveHabitPlans.Add(pair.HabitPlanName, selectedTrackerStatus[0].Status.ToString());
                    }
                    else
                    {
                        ActiveHabitPlans.Add(pair.HabitPlanName, "Not Set");
                    }
                }
            }
        }
    }
}
