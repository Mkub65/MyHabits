using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class HabitPlanPageVM
    {
        public Command DeleteCommand { get; set; }
        public Command AddHabitPlanCommand { get; set; }
        private Habit selectedHabit;
        public Habit SelectedHabit
        {
            get { return selectedHabit; }
            set { selectedHabit = value;}
        }
        private HabitPlan selectedHabitPlan;
        public HabitPlan SelectedHabitPlan
        {
            get { return selectedHabitPlan; }
            set
            {
                selectedHabitPlan = value;
                if(selectedHabitPlan != null)
                {
                    App.Current.MainPage.Navigation.PushAsync(new DetailHabitPlanPage(selectedHabitPlan));
                }
            }
        }
        public ObservableCollection<HabitPlan> HabitPlans { get; set; }
        public HabitPlanPageVM()
        {
            HabitPlans = new ObservableCollection<HabitPlan>();
            DeleteCommand = new Command(Delete);
            AddHabitPlanCommand = new Command(AddingHabitPlan);
        }
        public void GetSelectedHabitPlans()
        {
            HabitPlans.Clear();
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from h in habitPlans
                             where h.HabitId == SelectedHabit.Id
                             select h).ToList();
                foreach(var plan in Plans)
                {
                    HabitPlans.Add(plan);
                }
            }
        }
        private void Delete()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var rowsToDelete = conn.Table<HabitPlan>().Where(x => x.HabitId == SelectedHabit.Id).ToList();
                foreach (var row in rowsToDelete)
                {
                    var trackersToDelete = conn.Table<HabitTracker>().Where(x => x.HabitPlanId == row.Id).ToList();
                    for (int i = 0; i < trackersToDelete.Count; i++)
                    {
                        conn.Delete(trackersToDelete[i]);
                    }
                }
                for (int i = 0; i < rowsToDelete.Count; i++)
                {
                    conn.Delete(rowsToDelete[i]);
                }
                conn.CreateTable<Habit>();
                int rows = conn.Delete(SelectedHabit);

                if (rows > 0)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Habit succesfully deleted", "Ok");
                    App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Habit failed to be deleted", "Ok");
                }
            }
        }
        private void AddingHabitPlan()
        {
            if (selectedHabit != null)
            {
                App.Current.MainPage.Navigation.PushAsync(new AddingHabitPlanPage(selectedHabit));
            }
        }
    }
}
