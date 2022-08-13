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
    public partial class NewMainPage : ContentPage
    {
        public NewMainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                var dict = new Dictionary<string, string>();
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
                    if (selectedTrackerStatus.Any())
                    {
                        dict.Add(pair.HabitPlanName, selectedTrackerStatus[0].Status.ToString());
                    }
                    else
                    {
                        dict.Add(pair.HabitPlanName, "Not Set");
                    }
                }
                ProductsListView.ItemsSource = dict;
            }
        }
        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var dict = new Dictionary<string, string>();
            var DataPickerDate = e.NewDate.Date;
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from n in habitPlans
                             where n.StartDate <= DataPickerDate && n.EndDate >= DataPickerDate
                             select n).ToList();
                conn.CreateTable<HabitTracker>();
                var Trackers = conn.Table<HabitTracker>();
                foreach (var pair in Plans)
                {
                    var selectedTrackerStatus = (from n in Trackers where n.UpdateDate == DataPickerDate && n.HabitPlanId == pair.Id select n).ToList();
                    if (selectedTrackerStatus.Count != 0 && selectedTrackerStatus[0].UpdateDate == DataPickerDate)
                    {
                        dict.Add(pair.HabitPlanName, selectedTrackerStatus[0].Status.ToString());
                    }
                    else
                    {
                        dict.Add(pair.HabitPlanName, "Not Set");
                    }
                }
                ProductsListView.ItemsSource = dict;
            }
        }
        private void SetStatus_Clicked(object sender, EventArgs e)
        {
            var allHabitForDate = ProductsListView.ItemsSource;
            foreach(var pair in allHabitForDate)
            {
                App.Current.MainPage.DisplayAlert("siema", "siema", "string");
            }
        }

    }
}