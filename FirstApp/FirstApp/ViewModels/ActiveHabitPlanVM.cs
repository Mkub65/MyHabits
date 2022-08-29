using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class ActiveHabitPlanVM : INotifyPropertyChanged
    {
        public ObservableCollection<HabitPlan> ActiveHabitPlans { get; set; }
        public Command CopyCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        HabitPlan _selectedHabitPlan;
        public HabitPlan SelectedHabitPlan
        {
            get => _selectedHabitPlan;
            set
            {
                if (value != null)
                {
                    SelectStatus(value);
                    value = null;
                }
                _selectedHabitPlan = value;
                OnPropertyChanged();
            }
        }
        public ActiveHabitPlanVM()
        {
            ActiveHabitPlans = new ObservableCollection<HabitPlan>();
            CopyCommand = new Command(CopyHabitPlansForNewPeriod);
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
        public async void SelectStatus(HabitPlan Habit)
        {
            var habitPlan = Habit;
            string choosedStatus = await App.Current.MainPage.DisplayActionSheet("Choose Status: ", "Cancel", null, "None", "Done", "notDone", "Ignore");

            if (choosedStatus != "Cancel")
            {
                DateTime dateTime = DateTime.Now.Date;
                HabitTracker status = new HabitTracker()
                {
                    UpdateDate = dateTime,
                    Status = choosedStatus,
                    HabitPlanId = habitPlan.Id,
                };
                CheckIfTrackerExist(status);
            }
        }
        private void CheckIfTrackerExist(HabitTracker Tracker)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var trackerTable = conn.Table<HabitTracker>();
                var trackersForSelectedDate = (from n in trackerTable where n.UpdateDate == Tracker.UpdateDate.Date && n.HabitPlanId == Tracker.HabitPlanId select n).ToList();
                if (trackersForSelectedDate.Count == 0)
                {
                    CreateTracker(Tracker);
                }
                else
                {
                    var TrackerToUpdate = trackersForSelectedDate[0];
                    UpdateTracker(Tracker, TrackerToUpdate);
                }
            }
        }
        private async void CreateTracker(HabitTracker Tracker)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var rows = conn.Insert(Tracker);

                if (rows > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Status succesfully set for Today", "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failure", "Status failed to added for Today", "Ok");
                }
            }
        }
        private async void UpdateTracker(HabitTracker Tracker, HabitTracker TrackerToUpdate)
        {
            var oldTracker = TrackerToUpdate;
            oldTracker.Status = Tracker.Status;
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                int updatedRows = conn.Update(oldTracker);
                if (updatedRows > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Status For Today Changed", "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failure", "Status For Today failed to Change", "Ok");
                }
            }
        }
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private async void CopyHabitPlansForNewPeriod()
        {
            var promptData = await App.Current.MainPage.DisplayPromptAsync("Need Information", "Put the End date for new habits","OK","Cancel","01/01/2022");
            if(promptData != null)
            {
                var newEndDate = DateTime.ParseExact(promptData, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var habitPlans = ActiveHabitPlans;
                List<HabitPlan> newPlans = new List<HabitPlan>();
                foreach (var habit in habitPlans)
                {
                    newPlans.Add(new HabitPlan
                    {
                        HabitId = habit.HabitId,
                        HabitPlanName = habit.HabitPlanName,
                        StartDate = habit.EndDate.AddDays(1),
                        EndDate = newEndDate
                    });
                }
                using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
                {
                    conn.CreateTable<HabitPlan>();
                    var rows = 0;
                    foreach (var habitPlan in newPlans)
                    {
                        conn.Insert(habitPlan);
                        rows++;
                    }
                    if (rows > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Habit plans created for new period", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Failure", "Habit plans failed to create", "Ok");
                    }
                }
            }
        }
    }
}