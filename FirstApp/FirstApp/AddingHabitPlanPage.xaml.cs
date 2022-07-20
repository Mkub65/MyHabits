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
    public partial class AddingHabitPlanPage : ContentPage
    {
        private readonly AddingHabitPlanVM vm;
        public AddingHabitPlanPage(Habit habit)
        {
            InitializeComponent();
            vm = Resources["vm"] as AddingHabitPlanVM;
            vm.SelectedHabit = habit;
            BindingContext = new AddingHabitPlanVM();
        }
    }
}