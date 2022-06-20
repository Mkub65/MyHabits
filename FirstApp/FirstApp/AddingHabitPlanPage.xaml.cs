using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingHabitPlanPage : ContentPage
    {
        private readonly Habit habit;
        public AddingHabitPlanPage(Habit habit)
        {
            InitializeComponent();
            this.habit = habit;
        }

        private void AddHabitPlan_Clicked(object sender, EventArgs e)
        {
            HabitPlan habitPlan = new HabitPlan()
            {
                HabitId = habit.Id,
                HabitPlanName = habitPlanName.Text,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
            };

            if (habitPlan.StartDate > habitPlan.EndDate)
            {
                DisplayAlert("Failure","Start Date can't be greater then End Date, Change Start Date and Save Again","Ok");
            }
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
                {
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
                    conn.CreateTable<HabitPlan>();
                    var rows = conn.Insert(habitPlan);

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Habit plan succesfully added", "Ok");
                        Navigation.PopAsync(IsEnabled);
                    }
                    else
                    {
                        DisplayAlert("Failure", "Habit plan failed to added", "Ok");
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