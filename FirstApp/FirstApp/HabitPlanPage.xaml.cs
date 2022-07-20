using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstApp.Model;
using FirstApp.ViewModels;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HabitPlanPage : ContentPage
    {
        private HabitPlanPageVM vm;
        public HabitPlanPage(Habit selectedHabit)
        {
            InitializeComponent();
            vm = Resources["vm"] as HabitPlanPageVM;
            vm.SelectedHabit = selectedHabit;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetSelectedHabitPlans();
        }
    }
}