using FirstApp.Model;
using FirstApp.ViewModels;
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
        private ActiveHabitPlanVM vm;
        public ActiveHabitPlansPage()
        {
            InitializeComponent();
            vm = Resources["vm"] as ActiveHabitPlanVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetActiveHabitPlans();
        }
    }
}