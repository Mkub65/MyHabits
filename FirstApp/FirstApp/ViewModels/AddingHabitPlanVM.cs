using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
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
            HabitPlan habitPlan = new HabitPlan()
            {
                HabitId = SelectedHabit.Id,
                HabitPlanName = HabitPlanName,
                StartDate = StartDate,
                EndDate = EndDate,
            };
            if(CheckIfHabitPlanWithSameNameExist(habitPlan, SelectedHabit))
            {
                if (CheckIfHabitCollidate(habitPlan, SelectedHabit))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
                    {
                        conn.CreateTable<HabitPlan>();
                        var rows = conn.Insert(habitPlan);

                        conn.CreateTable<HabitTracker>();
                        foreach (DateTime day in EachCalendarDay(habitPlan.StartDate, habitPlan.EndDate))
                        {
                            if (day < DateTime.Now)
                            {
                                HabitTracker trackerForPastDate = new HabitTracker()
                                {
                                    UpdateDate = day.Date,
                                    Status = "NotDone",
                                    HabitPlanId = habitPlan.Id,
                                };
                                conn.Insert(trackerForPastDate);
                            }
                            else
                            {
                                HabitTracker trackerForFutureDate = new HabitTracker()
                                {
                                    UpdateDate = day.Date,
                                    Status = "NotSet",
                                    HabitPlanId = habitPlan.Id,
                                };
                                conn.Insert(trackerForFutureDate);
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
                            App.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Habit Plan for this time span already exist, try diffrent Start Date or End Date", "Ok");
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Failure", "Habit Plan with same name already exists, try diffrent Name", "Ok");
            }
        }
        public IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date < endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
        public bool CheckIfHabitCollidate(HabitPlan NewHabitPlan, Habit Habit)
        {
            int collidatePlans = 0;
            using(SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var allHabitPlans = conn.Table<HabitPlan>();
                var selectedHabitPlans = (from n in allHabitPlans where n.HabitId == Habit.Id select n).ToList();
                for(int i = 0; i < selectedHabitPlans.Count; i++)
                {
                    if (NewHabitPlan.StartDate.Date == selectedHabitPlans[i].StartDate.Date && NewHabitPlan.EndDate.Date == selectedHabitPlans[i].EndDate.Date)
                    {
                        collidatePlans++;
                    }
                } 
            }
            if(collidatePlans > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckIfHabitPlanWithSameNameExist(HabitPlan NewHabitPlan, Habit Habit)
        {
            int habitPlansWithSameName = 0;
            using(SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var allHabitPlans = conn.Table<HabitPlan>();
                var selectedHabitPlans = (from n in allHabitPlans where n.HabitId == Habit.Id select n).ToList();
                for (int i = 0; i < selectedHabitPlans.Count; i++)
                {
                    if(NewHabitPlan.HabitPlanName == selectedHabitPlans[i].HabitPlanName)
                    {
                        habitPlansWithSameName++;
                    }
                }
            }
            if(habitPlansWithSameName > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
