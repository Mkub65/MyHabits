using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class AddingPageVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Command AddHabitCommand { get; set; }
        public AddingPageVM()
        {
            AddHabitCommand = new Command(AddHabit);
        }
        public void AddHabit()
        {
            Habit habit = new Habit()
            {
                Name = Name,
                Description = Description,
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<Habit>();
                var rows = conn.Insert(habit);

                if (rows > 0)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Habit succesfully added", "Ok");
                    App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "Habit failed to added", "Ok");
                }
            }
        }
        public bool ValidateHabit()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Description);
        }
    }
}
