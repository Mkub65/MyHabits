using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstApp.Model;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HabitPlanPage : ContentPage
    {
        readonly Habit selectedHabit;
        public HabitPlanPage(Habit selectedHabit)
        {
            InitializeComponent();
            this.selectedHabit = selectedHabit;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>();
                var Plans = (from h in habitPlans
                             where h.HabitId == selectedHabit.Id
                             select h).ToList();
                habitPlansListView.ItemsSource = Plans;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddingHabitPlanPage(selectedHabit));
        }

        private void habitPlansListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedHabitPlan = habitPlansListView.SelectedItem as HabitPlan;

            Navigation.PushAsync(new DetailHabitPlanPage(selectedHabitPlan));
        }

        private void deleteHabitButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                // Need to delete Habit Trackers for each Habit Plan which is going to be delete
                conn.CreateTable<HabitPlan>();
                var rowsToDelete = conn.Table<HabitPlan>().Where(x => x.HabitId == selectedHabit.Id).ToList();
                foreach (var row in rowsToDelete)
                {
                    var trackersToDelete = conn.Table<HabitTracker>().Where(x => x.HabitPlanId == row.Id).ToList();
                    for (int i = 0; i < trackersToDelete.Count; i++)
                    {
                        conn.Delete(rowsToDelete[i]);
                    }
                }

                for (int i = 0; i < rowsToDelete.Count; i++)
                {
                    conn.Delete(rowsToDelete[i]);
                }
                conn.CreateTable<Habit>();
                int rows = conn.Delete(selectedHabit);

                if (rows > 0)
                {
                    DisplayAlert("Success", "Habit succesfully deleted", "Ok");
                    Navigation.PopAsync(IsEnabled);
                }
                else
                {
                    DisplayAlert("Failure", "Habit failed to be deleted", "Ok");
                }
            }
        }
    }
}