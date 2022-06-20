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
    public partial class DetailHabitPlanPage : ContentPage
    {
        readonly HabitPlan habitPlan;
        public DetailHabitPlanPage(HabitPlan habitPlan)
        {
            InitializeComponent();

            this.habitPlan = habitPlan;
            habitPlanName.Text = habitPlan.HabitPlanName;
            habitPlanStartDate.Text = habitPlan.StartDate.ToString();
            habitPlanEndDate.Text = habitPlan.EndDate.ToString();
        }

        private void DeleteHabitPlan_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var rowsToDelete = conn.Table<HabitTracker>().Where(x => x.HabitPlanId == habitPlan.Id).ToList();
                for (int i = 0; i < rowsToDelete.Count; i++)
                {
                    conn.Delete(rowsToDelete[i]);
                }
                conn.CreateTable<HabitPlan>();
                int deletedRows = conn.Delete(habitPlan);

                if (deletedRows > 0)
                {

                    DisplayAlert("Success", "Habit plan succesfully deleted", "Ok");
                    Navigation.PopAsync(IsEnabled);
                }
                else
                {
                    DisplayAlert("Failure", "Habit plan failed to be deleted", "Ok");
                }
            }
        }

        private void UpdateHabitPlan_Clicked(object sender, EventArgs e)
        {
            habitPlan.HabitPlanName = habitPlanName.Text;
            habitPlan.StartDate = Convert.ToDateTime(habitPlanStartDate.Text);
            habitPlan.EndDate = Convert.ToDateTime(habitPlanEndDate.Text);

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                int updatedRows = conn.Update(habitPlan);

                if (updatedRows > 0)
                {
                    DisplayAlert("Success", "Habit plan succesfully updated", "Ok");
                    Navigation.PopAsync(IsEnabled);
                }
                else
                {
                    DisplayAlert("Failure", "Habit plan failed to be updated", "Ok");
                }
            }
        }
    }
}