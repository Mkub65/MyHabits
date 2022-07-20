using FirstApp.Model;
using Microcharts;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class DetailsHabitPlanVM
    {
        public Command UpdateCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public HabitPlan HabitPlan { get; set; }
        public string HabitPlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        Dictionary<string, int> dict;
        public DetailsHabitPlanVM()
        {
            UpdateCommand = new Command(Update);
            DeleteCommand = new Command(Delete);
        }
        private void Update()
        {
            HabitPlan.HabitPlanName = HabitPlanName;
            HabitPlan.StartDate = Convert.ToDateTime(StartDate);
            HabitPlan.EndDate = Convert.ToDateTime(EndDate);

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                int updatedRows = conn.Update(HabitPlan);

                if (updatedRows > 0)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Habit plan succesfully updated", "Ok");
                    App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Habit plan failed to be updated", "Ok");
                }
            }
        }
        private void Delete()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var rowsToDelete = conn.Table<HabitTracker>().Where(x => x.HabitPlanId == HabitPlan.Id).ToList();
                for (int i = 0; i < rowsToDelete.Count; i++)
                {
                    conn.Delete(rowsToDelete[i]);
                }
                conn.CreateTable<HabitPlan>();
                int deletedRows = conn.Delete(HabitPlan);

                if (deletedRows > 0)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Habit plan succesfully deleted", "Ok");
                    App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Habit plan failed to be deleted", "Ok");
                }
            }
        }
        /*private void CreateData()
        {
            dict = new Dictionary<string, int>();
            List<string> statusVariants = new List<string> { "None", "Done", "NotDone", "Ignore" };
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var statusTable = conn.Table<HabitTracker>().Where(i => i.HabitPlanId == HabitPlan.Id);
                foreach (string variant in statusVariants)
                {
                    var statusList = statusTable.Where(n => n.Status == variant).Count();
                    dict.Add(variant, statusList);
                }
            }

        }
        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(212)
            {
                Label = "UWP",
                ValueLabel = "112",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(248)
            {
                Label = "Android",
                ValueLabel = "648",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(128)
            {
                Label = "iOS",
                ValueLabel = "428",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(514)
            {
                Label = "Forms",
                ValueLabel = "214",
                Color = SKColor.Parse("#3498db")
            }
        };*/
    }
}
