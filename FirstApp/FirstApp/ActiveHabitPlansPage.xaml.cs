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
    public partial class ActiveHabitPlansPage : ContentPage
    {
        public ActiveHabitPlansPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                var now = DateTime.Now;
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from n in habitPlans
                             where n.StartDate <= now && n.EndDate > now select n).ToList();
                acitveHabitPlansListView.ItemsSource = Plans;
            }
        }

        private async void acitveHabitPlansListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var habitPlan = (HabitPlan)e.Item;
            string choosedStatus = await DisplayActionSheet("Choose Status:", "Cancel", null, "None", "Done", "notDone","Ignore");

            if(choosedStatus != "Cancel")
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
                        await DisplayAlert("Success", "Status succesfully set", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Failure", "Status failed to added", "Ok");
                    }
                }
            }

        }
    }
}