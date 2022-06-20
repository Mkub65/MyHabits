using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPage : ContentPage
    {
        public AddingPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Habit habit = new Habit()
            {
                Name = name.Text,
                Description = description.Text,
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<Habit>();
                var rows = conn.Insert(habit);

                if (rows > 0)
                {
                    DisplayAlert("Success", "Habit succesfully added", "Ok");
                }
                else
                {
                    DisplayAlert("Failure", "Habit failed to added", "Ok");
                }
            }
        }
    }
}