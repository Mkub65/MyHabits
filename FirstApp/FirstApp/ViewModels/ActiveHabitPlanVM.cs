using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FirstApp.ViewModels
{
    public class ActiveHabitPlanVM
    {
        public ObservableCollection<HabitPlan> ActiveHabitPlans { get; set; }
        private HabitPlan _selectedHabitPlan;
        public HabitPlan SelectedHabitPlan
        {
            get { return _selectedHabitPlan; }
            set
            {
                if (value != null)
                {
                    _selectedHabitPlan = value;
                    SelectStatus();
                }
            }
        }
        public ActiveHabitPlanVM()
        {
            ActiveHabitPlans = new ObservableCollection<HabitPlan>();
        }
        public void GetActiveHabitPlans()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                var now = DateTime.Now;
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from n in habitPlans
                             where n.StartDate <= now && n.EndDate >= now
                             select n).ToList();
                foreach(var plan in Plans)
                {
                    ActiveHabitPlans.Add(plan);
                }
            }
        }
        public async void SelectStatus()
        {
            var habitPlan = SelectedHabitPlan;
            string choosedStatus = await App.Current.MainPage.DisplayActionSheet("Choose Status: ", "Cancel", null, "None", "Done", "notDone", "Ignore");

            if (choosedStatus != "Cancel")
            {
                DateTime dateTime = DateTime.Now;
                HabitTracker status = new HabitTracker()
                {
                    UpdateDate = dateTime,
                    Status = choosedStatus,
                    HabitPlanId = habitPlan.Id,
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
                {
                    conn.CreateTable<HabitTracker>();
                    var rows = conn.Insert(status);

                    if (rows > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Status succesfully set", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Failure", "Status failed to added", "Ok");
                    }
                }
            }
        }
    }
}
