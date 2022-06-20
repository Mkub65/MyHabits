using FirstApp.Model;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<Habit>();
                var habits = conn.Table<Habit>().ToList();
                habitListView.ItemsSource = habits;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddingPage());
        }

        private void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedHabit = habitListView.SelectedItem as Habit;

            if (selectedHabit != null)
            {
                Navigation.PushAsync(new HabitPlanPage(selectedHabit));
            }

        }
    }
}