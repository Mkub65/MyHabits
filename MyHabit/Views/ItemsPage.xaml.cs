using MyHabit.Models;
using MyHabit.ViewModels;
using MyHabit.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHabit.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();

            var habitStatesStrings = new List<string>(Enum.GetNames(typeof(HabitStatus)));

           // var stringr = _viewModel.Items[0].Habits.ToArray()[0];

            //var picker = new Picker { Title = "Select state", TitleColor = Color.Red };
            //picker.ItemsSource = habitStatesStrings;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}