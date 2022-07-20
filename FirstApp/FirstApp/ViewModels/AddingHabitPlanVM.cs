using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class AddingHabitPlanVM
    {
        public Habit SelectedHabit { get; set; }
        public string HabitPlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime MinStartDate { get; set; }
        public Command AddHabitPlanCommand { get; set; }
        public AddingHabitPlanVM()
        {
            AddHabitPlanCommand = new Command(AddHabitPlan);
        }

        public void AddHabitPlan()
        {
            {
                HabitPlan habitPlan = new HabitPlan()
                {
                    HabitId = SelectedHabit.Id,
                    HabitPlanName = HabitPlanName,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };

                if (habitPlan.StartDate > habitPlan.EndDate)
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Start Date can't be greater then End Date, Change Start Date and Save Again", "Ok");
                }
                else
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
                    {
                        conn.CreateTable<HabitPlan>();
                        var rows = conn.Insert(habitPlan);

                        if (habitPlan.StartDate < DateTime.Now)
                        {
                            conn.CreateTable<HabitTracker>();
                            foreach (DateTime day in EachCalendarDay(habitPlan.StartDate, DateTime.Now))
                            {
                                HabitTracker tracker = new HabitTracker()
                                {
                                    UpdateDate = day.Date,
                                    Status = "NotDone",
                                    HabitPlanId = habitPlan.Id,
                                };
                                conn.Insert(tracker);
                            }
                        }
                        if (rows > 0)
                        {
                            App.Current.MainPage.DisplayAlert("Success", "Habit plan succesfully added", "Ok");
                            App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            App.Current.MainPage.DisplayAlert("Failure", "Habit plan failed to added", "Ok");
                        }
                    }
                }
            }
        }
        public IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
